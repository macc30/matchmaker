using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models.TankClasses
{
    public class LightTank : ITankClass
    {
        public TankClassId TankClass
        {
            get { return TankClassId.Light; }
        }

        public int MaxPerMatch
        {
            get { return 5; }
        }

        public int MaxTierSpread
        {
            get { return 3; }
        }
    }
}