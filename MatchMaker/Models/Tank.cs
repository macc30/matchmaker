using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models
{
    [Serializable]
    public class Tank
    {
        public Tank()
        {
            this.Id = System.Guid.NewGuid();
        }

        public Guid Id { get; set;  }

        public Int32 WG_ID { get; set; }

        public TankClass? TankClass { get; set; }

        public Nation? Nation { get; set; }

        public String Name { get; set; }

        public Int32 Tier { get; set; }

        public Boolean PreferentialMM { get; set; }

        public Int32 Health { get; set; }

        public Double Popularity { get; set; }

        public Double ExpectedDamage { get; set; }
    }
}
