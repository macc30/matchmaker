using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MatchMaker.Models.TankClasses;

namespace MatchMaker.Models
{
    public class Tank
    {
        public Guid Id { get; set;  }

        public ITankClass TankClass { get; set; }

        public Nation Nation { get; set; }

        public String Name { get; set; }

        public Int32 Tier { get; set; }

        public Double Popularity { get; set; }
        public Int32 ExpectedDamage { get; set; }
    }
}
