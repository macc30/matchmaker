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

        public Int32 HeavyCount { get { return Members.Where(_ => _.Tank.TankClass.TankClass == TankClasses.TankClassId.Heavy).Count(); } }
        public Int32 MediumCount { get { return Members.Where(_ => _.Tank.TankClass.TankClass == TankClasses.TankClassId.Medium).Count(); } }
        public Int32 LightCount { get { return Members.Where(_ => _.Tank.TankClass.TankClass == TankClasses.TankClassId.Light).Count(); } }
        public Int32 TankDestroyerCount { get { return Members.Where(_ => _.Tank.TankClass.TankClass == TankClasses.TankClassId.TankDestroyer).Count(); } }
        public Int32 SPGCount { get { return Members.Where(_ => _.Tank.TankClass.TankClass == TankClasses.TankClassId.SPG).Count(); } }

        public void Dispose()
        {
            foreach (var player in Members)
            {
                player.Dispose();
            }
        }
    }
}
