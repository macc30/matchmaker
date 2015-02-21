using MatchMaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models
{
    public class PlayerTankSelection : IDisposable
    {
        public PoolLease<Player> Player { get; set; }
        public Tank Tank { get; set; }

        public long ArrivalTime { get; set; }
        public long MatchStartTime { get; set; }
        public long FinishTime { get; set; }

        public Double Winrate { get; set; }
        public Double WN8 { get; set; }

        public Double ExpectedDamage { get; set; }
        public Double AverageKills { get; set; }


        public override string ToString()
        {
            if(Player != null && Tank != null)
            {
                return String.Format("{0} in {1} ({2})", this.Player.Item.Id.ToString("N"), this.Tank.Name, this.Tank.Tier);
            }
            return String.Empty;
        }

        public void Dispose()
        {
            if (null != Player)
            {
                Player.Dispose();
            }
        }
    }
}
