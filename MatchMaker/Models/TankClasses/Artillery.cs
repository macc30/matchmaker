using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models.TankClasses
{
    public class Artillery : ITankClass
    {
        public TankClassId TankClass
        {
            get { return TankClassId.SPG; }
        }

        public int MaxPerMatch
        {
            get { return 3; }
        }

        public int MaxTierSpread
        {
            get { return 2; }
        }
    }
}
