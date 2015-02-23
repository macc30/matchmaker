using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using MatchMaker.Models;

namespace MatchMaker.Algorithms
{
    public abstract class MatchMakerBase
    {
        public static class Rules
        {
            public static Dictionary<TankClass, Int32> MaximumPerTeam = new Dictionary<TankClass, int>()
            {
                {TankClass.LightTank, 5},
                {TankClass.MediumTank, 7},
                {TankClass.HeavyTank, 7},
                {TankClass.Artillery, 3},
                {TankClass.TankDestroyer, 7},
            };

            public static Dictionary<TankClass, Int32> Spreads = new Dictionary<TankClass, int>()
            {
                {TankClass.LightTank, 3},
                {TankClass.MediumTank, 2},
                {TankClass.HeavyTank, 2},
                {TankClass.Artillery, 2},
                {TankClass.TankDestroyer, 2},
            };
        }

        protected List<PlayerTankSelection> _playerQueue = new List<PlayerTankSelection>();

        public IEnumerable<PlayerTankSelection> PlayerQueue
        {
            get
            {
                return _playerQueue;
            }
        }

        public Int32 QueueDepth
        {
            get
            {
                return _playerQueue.Count;
            }
        }
        public void QueuePlayer(PlayerTankSelection player)
        {
            _playerQueue.Add(player);
        }

        public abstract Match TryFormMatch();

        protected bool CanAddPlayer(Match match, Team team, PlayerTankSelection player)
        {
            var team_tier = match.Tier;
            var player_tier = player.Tank.Tier;

            var tier_spread = Math.Abs(team_tier - player_tier);

            var largest_spread = Rules.Spreads.Values.Max();

            if (tier_spread > largest_spread) //auto dq crazy spreads.
            {
                return false;
            }

            switch (player.Tank.TankClass)
            {
                case TankClass.LightTank:
                    {
                        var light_count = team.LightCount;
                        var spread_ok = tier_spread < Rules.Spreads[TankClass.LightTank];
                        var count_ok = light_count <= Rules.MaximumPerTeam[TankClass.LightTank];
                        return spread_ok && count_ok;
                    }
                case TankClass.MediumTank:
                    {
                        var medium_count = team.MediumCount;
                        var spread_ok = tier_spread < Rules.Spreads[TankClass.MediumTank];
                        var count_ok = medium_count <= Rules.MaximumPerTeam[TankClass.MediumTank];
                        return spread_ok && count_ok;
                    }
                case TankClass.HeavyTank:
                    {
                        var heavy_count = team.HeavyCount;
                        var spread_ok = tier_spread < Rules.Spreads[TankClass.HeavyTank];
                        var count_ok = heavy_count <= Rules.MaximumPerTeam[TankClass.HeavyTank];
                        return spread_ok && count_ok;
                    }
                case TankClass.TankDestroyer:
                    {
                        var td_count = team.TankDestroyerCount;
                        var spread_ok = tier_spread < Rules.Spreads[TankClass.TankDestroyer];
                        var count_ok = td_count <= Rules.MaximumPerTeam[TankClass.TankDestroyer];
                        return spread_ok && count_ok;
                    }
                case TankClass.Artillery:
                    {
                        var arty_count = team.ArtilleryCount;
                        var spread_ok = tier_spread < Rules.Spreads[TankClass.Artillery];
                        var count_ok = arty_count <= Rules.MaximumPerTeam[TankClass.Artillery];
                        return spread_ok && count_ok;
                    }
            }

            return false;
        }

        protected bool IsComplete(Match match)
        {
            return match.TotalPlayers == 30;
        }
    }
}