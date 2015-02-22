using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models
{
    public class Team : IDisposable
    {
        public List<PlayerTankSelection> Members { get; set; }

        public Int32 HeavyCount { get { return Members.Where(_ => _.Tank.TankClass == TankClass.HeavyTank).Count(); } }
        public Int32 MediumCount { get { return Members.Where(_ => _.Tank.TankClass == TankClass.MediumTank).Count(); } }
        public Int32 LightCount { get { return Members.Where(_ => _.Tank.TankClass == TankClass.LightTank).Count(); } }
        public Int32 TankDestroyerCount { get { return Members.Where(_ => _.Tank.TankClass == TankClass.TankDestroyer).Count(); } }
        public Int32 SPGCount { get { return Members.Where(_ => _.Tank.TankClass == TankClass.Artillery).Count(); } }

        public void Dispose()
        {
            foreach (var player in Members)
            {
                player.Dispose();
            }
        }
    }
}
