using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models
{
    public class Team : IDisposable
    {
        public Team()
        {
            this.Members = new List<PlayerTankSelection>();
        }

        public List<PlayerTankSelection> Members { get; set; }

        public Int32 TopTier { get { return Members.Max(_ => _.Tank.Tier); } }
        public Int32 BottomTier { get { return Members.Min(_ => _.Tank.Tier); } }

        public Int32 HeavyCount { get { return Members.Where(_ => _.Tank.TankClass == TankClass.HeavyTank).Count(); } }
        public Int32 MediumCount { get { return Members.Where(_ => _.Tank.TankClass == TankClass.MediumTank).Count(); } }
        public Int32 LightCount { get { return Members.Where(_ => _.Tank.TankClass == TankClass.LightTank).Count(); } }
        public Int32 TankDestroyerCount { get { return Members.Where(_ => _.Tank.TankClass == TankClass.TankDestroyer).Count(); } }
        public Int32 ArtilleryCount { get { return Members.Where(_ => _.Tank.TankClass == TankClass.Artillery).Count(); } }

        public void SavePlayedMatchForMembers(Match match)
        {
            foreach (var member in Members)
            {
                member.Player.Item.MatchesPlayed.Add(match);
            }
        }

        public override string ToString()
        {
            var strings = new List<string>();
            foreach (var member in Members.OrderByDescending(_ => (int)_.Tank.TankClass))
            {
                switch (member.Tank.TankClass)
                {
                    case TankClass.HeavyTank:
                        strings.Add(String.Format("H{0}", member.Tank.Tier));
                        break;
                    case TankClass.MediumTank:
                        strings.Add(String.Format("M{0}", member.Tank.Tier));
                        break;
                    case TankClass.TankDestroyer:
                        strings.Add(String.Format("TD{0}", member.Tank.Tier));
                        break;
                    case TankClass.LightTank:
                        strings.Add(String.Format("L{0}", member.Tank.Tier));
                        break;
                    case TankClass.Artillery:
                        strings.Add(String.Format("A{0}", member.Tank.Tier));
                        break;
                }
            }

            return String.Join(",", strings);
        }

        public void Dispose()
        {
            foreach (var player in Members)
            {
                player.Dispose();
            }
        }
    }
}
