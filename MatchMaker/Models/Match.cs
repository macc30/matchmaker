using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models
{
    public class Match
    {
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
    }
}
