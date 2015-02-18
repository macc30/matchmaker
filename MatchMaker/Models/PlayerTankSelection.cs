using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models
{
    public class PlayerTankSelection
    {
        public Player Player { get; set; }
        public Tank Tank { get; set; }

        public Double Winrate { get; set; }
        public Double WN8 { get; set; }

        public Double ExpectedDamage { get; set; }
        public Double AverageKills { get; set; }
    }
}
