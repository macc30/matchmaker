using MatchMaker.Core;
using MatchMaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.Core.Probability;
using System.IO;

namespace MatchMaker
{
    public class Simulation
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
        }

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

                _logWriter.WriteLine(String.Format("{0,22} :: {1}", _formatClockTime(_clock), message));
                _logWriter.Flush();
            }
        }

        public static double MS_PER_SECOND = 1000.0;
        public static double MS_PER_MINUTE = 60.0 * 1000.0;
        public static double MS_PER_HOUR = 60.0 * 60.0 * 1000.0;

        private static string _formatClockTime(long clock_value)
        {
            if (clock_value < MS_PER_SECOND)
            {
                return String.Format("{0} ms", clock_value);
            }
            else if (clock_value < MS_PER_MINUTE)
            {
                return String.Format("{0}.{1} sec", Math.Floor((double)clock_value / 1000.0), clock_value % 1000);
            }
            else if (clock_value < MS_PER_HOUR)
            {
                var minutes = Math.Floor((clock_value) / MS_PER_MINUTE);
                var minutes_in_ms = minutes * MS_PER_MINUTE;
                var seconds = Math.Floor((clock_value - minutes_in_ms) / MS_PER_SECOND);
                var seconds_in_ms = seconds * MS_PER_SECOND;
                var ms = clock_value - minutes_in_ms - seconds_in_ms;
                return String.Format("{0} min {1}.{2} sec", minutes, seconds, ms);
            }
            else
            {
                var hours = Math.Floor((double)clock_value / MS_PER_HOUR);
                var hours_in_ms = hours * MS_PER_HOUR;
                var minutes = Math.Floor((clock_value - hours_in_ms) / MS_PER_MINUTE);
                var minutes_in_ms = minutes * MS_PER_MINUTE;
                var seconds = Math.Floor((clock_value - hours_in_ms - minutes_in_ms) / MS_PER_SECOND);
                var seconds_in_ms = seconds * MS_PER_SECOND;
                var ms = clock_value - hours_in_ms - minutes_in_ms - seconds_in_ms;

                return String.Format("{0} hr {1} min {2}.{3} sec", hours, minutes, seconds, ms);
            }
        }

        private long _clock = 0;
        private long _step = 500; //500 ms

        private int _matchesPlayed = 0;

        public double AveragePlayersPerMinute { get; private set; }
        public int PlayerPopulationCount { get; private set; }
        public int MaximumMatchCount { get; private set; }

        private List<Match> _inProgress = new List<Match>();
        private Pool<Player> _playerPool = new Pool<Player>();

        private MatchMaker _match_maker = new MatchMaker();

        private void _fillPlayerPool()
        {
            PlayerPopulationCount.Times(() =>
            {
                _playerPool.Add(Player.Create());
            });
        }

        private void _queueArrivals()
        {
            var exp = new ExponentialDistribution(0.001); //TODO: going to need to be determined by the # of players, the average length of game etc.

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
                        entry.Tank = TankDb.Current.RandomTank();

                        //figure out what tank to randomly take as well ...
                        _match_maker.QueuePlayer(entry);
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
    
            _match_maker = new MatchMaker();

            _fillPlayerPool();

            while (_matchesPlayed < MaximumMatchCount)
            {
                //schedule player arrivals
                _queueArrivals();

                var newMatch = _match_maker.TryFormMatch();

                if (newMatch != null)
                {
                    newMatch.Start(_clock);
                }

                foreach (var inProgress in _inProgress)
                {
                    if(inProgress.CanEnd(_clock))
                    {
                        //compile stats etc.
                    }
                }

                _clock = _clock + _step;
            }
        }
    }
}
