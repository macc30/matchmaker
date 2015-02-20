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
        public const Int32 MatchTimeStdDev = (int)Math.Sqrt(5 * 60 * 1000);

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

        private long _start_time;
        private long _end_time;

        public void Start(long start_time)
        {
            var normal = new NormalDistribution(7 * 60 * 1000, 547);
        }
    }
}
