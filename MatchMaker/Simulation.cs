using MatchMaker.Core;
using MatchMaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.Core.Probability;
using System.IO;
using MatchMaker.Algorithms;

namespace MatchMaker
{
    public class SimulationResults
    {
        public SimulationResults()
        {
            this.MatchTierDistribution = new Dictionary<int, int>();

            for (var tier = 1; tier <= 10; tier++)
            {
                this.MatchTierDistribution[tier] = 0;
            }
        }

        public double WaitTimeAverage { get; set; }
        public double WaitTimeStandardDeviation { get; set; }

        public double QueueDepthAverage { get; set; }

        public double TierPlacementAverage { get; set; }
        public double TierPlacementAverageLights { get; set; }

        public double MatchLengthAverage { get; set; }

        public Dictionary<Int32, Int32> MatchTierDistribution { get; set; }

        public double HeavyCountAverage { get; set; }
        public double MediumCountAverage { get; set; }
        public double LightCountAverage { get; set; }
        public double TankDestroyerCountAverage { get; set; }
        public double ArtilleryCountAverage { get; set; }
    }

    public class Simulation<T> where T : MatchMakerBase, new()
    {
        private static Random _randomProvider = null;
        public static Random RandomProvider
        {
            get
            {
                if (null == _randomProvider)
                {
                    _randomProvider = new Random();
                }
                return _randomProvider;
            }
        }

        public Simulation(Int32 playerPopulationCount, Int32 maximumMatchCount)
        {
            this.PlayerPopulationCount = playerPopulationCount;
            this.MaximumMatchCount = maximumMatchCount;
            this.Verbocity = LogVerbocity.Informational;
        }

        public enum LogVerbocity
        {
            Off = 0,
            Informational = 1,
            Verbose = 2
        }

        public LogVerbocity Verbocity { get; set; }

        private Stream _logStream = null;
        public Stream LogStream
        {
            get
            {
                return _logStream;
            }
            set
            {
                _logStream = value;
            }
        }

        private StreamWriter _logWriter = null;

        public void Log(String message)
        {
            if(null != _logStream)
            {
                if (null == _logWriter)
                {
                    _logWriter = new StreamWriter(_logStream);
                }

                _logWriter.WriteLine(String.Format("{0}: {1}", _clock.ClockFormat(), message));
                _logWriter.Flush();
            }
        }

        private long _clock = 0;
        private long _step = 500; //500 ms

        private int _matchesPlayedCount = 0;

        public Boolean RealTime { get; set; }

        public double AveragePlayersPerMinute { get; private set; }
        public int PlayerPopulationCount { get; private set; }
        public int MaximumMatchCount { get; private set; }

        private List<Match> _inProgress = new List<Match>();
        private Pool<Player> _playerPool = new Pool<Player>();

        private MatchMakerBase _match_maker = null;

        //stat keeping ...
        private List<Int32> _queueDepths = new List<Int32>();
        private List<Match> _matchesPlayed = new List<Match>();


        private void _fillPlayerPool()
        {
            PlayerPopulationCount.Times(() =>
            {
                _playerPool.Add(Player.Create());
            });
        }

        private void _queueArrivals()
        {
            var average_time_between_arrivals = (1.0 / 16.0) * 1000; //every 16th of a second

            var exp = new ExponentialDistribution(1.0 / average_time_between_arrivals); //TODO: going to need to be determined by the # of players, the average length of game etc.

            var arrival_time = _clock;
            while (arrival_time < _clock + _step)
            {
                var time_offset = exp.GetValue();
                arrival_time = arrival_time + (long)Math.Ceiling(time_offset);

                if (arrival_time < _clock + _step)
                {
                    var player_lease = _playerPool.LeaseRandom();
                    if (player_lease != null)
                    {
                        var entry = new PlayerTankSelection() { ArrivalTime = arrival_time };
                        entry.Player = player_lease;
                        entry.Tank = TankDb.Current.Tanks.WeightedRandomSelection(_ => _.Popularity, randomProvider: _randomProvider);

                        //figure out what tank to randomly take as well ...
                        _match_maker.QueuePlayer(entry);

                        if (Verbocity == LogVerbocity.Verbose)
                        {
                            Log(String.Format("Queued => {0} {1} ({2})", entry.Tank.TankClass, entry.Tank.Name, entry.Tank.Tier));
                        }
                    }
                }
            }
        }

        public void Start()
        {
            if (_clock != 0)
            {
                return;
            }

            _match_maker = new T();

            _fillPlayerPool();

            while (_matchesPlayedCount < MaximumMatchCount)
            {
                _queueArrivals();

                if (_clock % 5000 == 0)
                {
                    var players_in_queue = _match_maker.QueueDepth;
                    _queueDepths.Add(players_in_queue);
                    if (_match_maker.PlayerQueue.Any())
                    {
                        var average_wait_msec = (long)_match_maker.PlayerQueue.Average(_ => Math.Abs(_.ArrivalTime - _clock));
                        if ((int)Verbocity >= 1)
                        {
                            Log(String.Format("Players in Queue: {0}, Average Wait Time: {1}, Available Player Pool Size: {2}", players_in_queue, average_wait_msec.ClockFormat(), _playerPool.AvailableCount));
                        }
                    }
                    else if ((int)Verbocity >= 1)
                    {
                        Log("Players in Queue: 0, Average Wait Time: N/A");
                    }
                }

                var newMatch = _match_maker.TryFormMatch();

                if (newMatch != null)
                {
                    newMatch.Start(_clock);
                    _inProgress.Add(newMatch);
                    if ((int)Verbocity >= 1)
                    {
                        Log("Match Started: " + newMatch.ToString());
                    }
                }

                var to_remove_from_inprogress = new List<Match>();
                foreach (var inProgress in _inProgress)
                {
                    if(inProgress.CanEnd(_clock))
                    {
                        inProgress.End(_clock);
                        to_remove_from_inprogress.Add(inProgress);
                        _matchesPlayed.Add(inProgress);
                        _matchesPlayedCount++;
                        if ((int)Verbocity >= 1)
                        {
                            Log("Match Complete");
                        }
                    }
                }

                foreach (var to_remove in to_remove_from_inprogress)
                {
                    _inProgress.Remove(to_remove);
                }

                if (RealTime)
                {
                    System.Threading.Thread.Sleep((int)_step);
                }

                _clock = _clock + _step;
            }
        }

        public SimulationResults CollectResults()
        {
            var results = new SimulationResults();

            var tier_placements = new List<Int32>();
            var tier_placements_lights = new List<Int32>();

            var match_lengths = new List<long>();

            var heavy_counts = new List<Int32>();
            var medium_counts = new List<Int32>();
            var light_counts = new List<Int32>();
            var tank_destroyer_counts = new List<Int32>();
            var artillery_counts = new List<Int32>();

            var wait_times = new List<long>();
            foreach (var match in _matchesPlayed)
            {
                results.MatchTierDistribution[match.Tier]++;

                heavy_counts.Add(match.TeamA.HeavyCount);
                heavy_counts.Add(match.TeamB.HeavyCount);

                medium_counts.Add(match.TeamA.MediumCount);
                medium_counts.Add(match.TeamB.MediumCount);

                light_counts.Add(match.TeamA.LightCount);
                light_counts.Add(match.TeamB.LightCount);

                tank_destroyer_counts.Add(match.TeamA.TankDestroyerCount);
                tank_destroyer_counts.Add(match.TeamB.TankDestroyerCount);

                artillery_counts.Add(match.TeamA.ArtilleryCount);
                artillery_counts.Add(match.TeamB.ArtilleryCount);

                match_lengths.Add(match.EllapsedTime);

                foreach (var player in match.TeamA.Members)
                {
                    var tier_placement = match.Tier - player.Tank.Tier;;
                    if (player.Tank.TankClass == TankClass.LightTank)
                    {
                        tier_placements_lights.Add(tier_placement);
                    }
                    else
                    {
                        tier_placements.Add(tier_placement);
                    }

                    var queue_time = Math.Abs(player.ArrivalTime - match.StartTime);
                    wait_times.Add(queue_time);
                }
                foreach (var player in match.TeamB.Members)
                {
                    var tier_placement = match.Tier - player.Tank.Tier;
                    if (player.Tank.TankClass == TankClass.LightTank)
                    {
                        tier_placements_lights.Add(tier_placement);
                    }
                    else
                    {
                        tier_placements.Add(tier_placement);
                    }

                    var queue_time = Math.Abs(player.ArrivalTime - player.MatchStartTime);
                    wait_times.Add(queue_time);
                }
            }

            results.TierPlacementAverage = tier_placements.Average();
            results.TierPlacementAverageLights = tier_placements_lights.Average();

            results.MatchLengthAverage = match_lengths.Average();

            results.WaitTimeStandardDeviation = wait_times.StdDev();
            results.WaitTimeAverage = wait_times.Average();

            results.QueueDepthAverage = _queueDepths.Average();

            results.HeavyCountAverage = heavy_counts.Average();
            results.MediumCountAverage = medium_counts.Average();
            results.LightCountAverage = light_counts.Average();
            results.TankDestroyerCountAverage = tank_destroyer_counts.Average();
            results.ArtilleryCountAverage = artillery_counts.Average();

            return results;
        }
    }
}
