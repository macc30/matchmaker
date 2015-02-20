using MatchMaker.Core;
using MatchMaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.Core.Probability;

namespace MatchMaker
{
    public class Simulation
    {
        public Simulation(Int32 playerPopulationCount, Int32 maximumMatchCount, Double playersPerMinute)
        {
            this.PlayerPopulationCount = playerPopulationCount;
            this.MaximumMatchCount = maximumMatchCount;
            this.PlayersPerMinute = playersPerMinute;
        }

        private long _clock = 0;
        private long _step = 500; //500 ms

        private int _matchesPlayed = 0;

        public double PlayersPerMinute { get; private set; }
        public int PlayerPopulationCount { get; private set; }
        public int MaximumMatchCount { get; private set; }

        private List<Match> _inProgress = new List<Match>();
        private Pool<Player> _playerPool = new Pool<Player>();

        private void _fillPlayerPool()
        {
            PlayerPopulationCount.Times(() =>
            {
                _playerPool.Add(Player.Create());
            });
        }

        private void _queueArrivals()
        {
            var exp = new ExponentialDistribution(1.0);

            var arrival_time = _clock;
            while (arrival_time < _clock + _step)
            {

            }
        }

        public void Start()
        {
            if (_clock != 0)
            {
                return;
            }

            var match_maker = new MatchMaker();

            _fillPlayerPool();

            while (_matchesPlayed < MaximumMatchCount)
            {
                //schedule player arrivals
                _queueArrivals();

                var newMatches = match_maker.TryFormMatches();

                foreach (var match in newMatches)
                {
                    //start matches ...
                }

                foreach (var inProgress in _inProgress)
                {
                    //check to see if the match is done ...
                    //if so, release the players back to the pool
                    //record stats.
                }

                _clock = _clock + _step;
            }
        }
    }
}
