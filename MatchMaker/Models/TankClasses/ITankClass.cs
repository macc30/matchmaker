using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models.TankClasses
{
    public interface ITankClass
    {
        TankClassId TankClass { get; }
        int MaxPerMatch { get; }
        int MaxTierSpread { get; }
    }
}
