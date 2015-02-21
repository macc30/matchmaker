using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MatchMaker.Core.Probability;

namespace MatchMaker.Models
{
    public enum MatchOutcome 
    {
        TeamAWins = 1,
        TeamBWins = 2,
        Draw = 3,
    }

    public class Match
    {
        public const Int32 MatchTimeMean = 7 * 60 * 1000;
        public static Int32 MatchTimeStdDev { get { return (int)Math.Sqrt(5 * 60 * 1000); } }

        public Match()
        {
            this.Id = System.Guid.NewGuid();
            this.TeamA = new Team();
            this.TeamB = new Team();
        }

        public Int32 EllapsedTime { get; set; }

        public Guid Id { get; set; }
        public Int32 Tier { get; set; }

        public Team TeamA { get; set; }
        public Team TeamB { get; set; }

        public Int32 TeamAHealthPool { get; set; }
        public Int32 TeamBHealthPool { get; set; }

        public MatchOutcome? Outcome { get; set; }

        private long _start_time = 0;
        private long _end_time = 0;

        public void Start(long start_time)
        {
            var normal = new NormalDistribution(7 * 60 * 1000, 547);
            _start_time = start_time;
            _end_time = _start_time + (long)normal.GetValue(); //random match time ...
        }

        public bool TryEnd(long current_time)
        {
            if (current_time >= _end_time)
            {
                //figure out outcome?

                //drain both team pools.
                TeamA.Dispose();
                TeamB.Dispose();
                return true;
            }

            return false;
        }
    }
}
