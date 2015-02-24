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

        public long EllapsedTime { get; set; }

        public Guid Id { get; set; }
        public Int32 Tier { get; set; }

        public Team TeamA { get; set; }
        public Team TeamB { get; set; }

        public Int32 TeamAHealthPool { get { return TeamA.Members.Sum(_ => _.Tank.Health); } }
        public Int32 TeamBHealthPool { get { return TeamB.Members.Sum(_ => _.Tank.Health); } }

        public Int32 TotalPlayers { get { return TeamA.Members.Count + TeamB.Members.Count; } }

        public MatchOutcome? Outcome { get; set; }

        private long _start_time = 0;
        public long StartTime
        {
            get { return _start_time; }
        }

        private long _end_time = 0;
        public long EndTime 
        {
            get { return _end_time; }
        }

        public void Start(long start_time)
        {
            var normal = new NormalDistribution(7 * (60.0 * 1000.0), 547);
            this.EllapsedTime = (long)normal.GetValue(); //random match time ...

            _start_time = start_time;
            _end_time = _start_time + this.EllapsedTime;
        }

        public bool CanEnd(long current_time)
        {
            return current_time >= _end_time;
        }

        public void End(long current_time)
        {
            _end_time = current_time;

            TeamA.SavePlayedMatchForMembers(this);
            TeamB.SavePlayedMatchForMembers(this);

            TeamA.Dispose();
            TeamB.Dispose();
        }

        public override string ToString()
        {
            var buffer = new System.Text.StringBuilder();
            buffer.Append(TeamA.ToString());
            buffer.Append(" vs. ");
            buffer.Append(TeamB.ToString());
            return buffer.ToString();   
        }
    }
}
