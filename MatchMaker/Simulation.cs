using MatchMaker.Core;
using MatchMaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker
{
    public class Simulation
    {
        public Simulation(Int32 playerPopulationCount, Int32 maximumMatchCount)
        {
            this.PlayerPopulationCount = playerPopulationCount;
            this.MaximumMatchCount = maximumMatchCount;
        }

        private long _clock = 0;
        private long _step = 500; //500 ms

        private int _matchesPlayed = 0;

        public int PlayerPopulationCount { get; private set; }
        public int MaximumMatchCount { get; private set; }

        private Pool<Player> _playerPool = new Pool<Player>();

        public void Start()
        {
            var match_maker = new MatchMaker();

            while (_matchesPlayed < MaximumMatchCount)
            {
                //schedule player arrivals



                var matches = match_maker.TryFormMatches();

                foreach (var match in matches)
                {
                    //record stats about match etc ...
                }

                _clock = _clock + _step;
            }
        }
    }
}
