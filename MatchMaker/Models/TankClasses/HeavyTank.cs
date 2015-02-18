using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models.TankClasses
{
    public class HeavyTank : ITankClass
    {
        public TankClassId TankClass
        {
            get { return TankClassId.Heavy; }
        }

        public int MaxPerMatch
        {
            get { return 7; }
        }

        public int MaxTierSpread
        {
            get { return 2; }
        }
    }
}
