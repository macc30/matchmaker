using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.Models;
using MatchMaker.Core;

namespace MatchMaker.Algorithms
{
    public class InOrderCreation : MatchMakerBase
    {
        public override Match TryFormMatch()
        {
            if (this.QueueDepth < 30)
            {
                return null;
            }

            var added = new List<PlayerTankSelection>();

            var basis_player = _playerQueue.PeekFront();
            added.Add(basis_player);

            var match = new Match();
            match.Tier = basis_player.Tank.Tier;
            match.TeamA.Members.Add(basis_player);

            foreach (var additional_player in _playerQueue)
            {
                if (match.TotalPlayers < 30)
                {
                    if (match.TeamA.Members.Count < 15)
                    {
                        if (CanAddPlayer(match, match.TeamA, additional_player))
                        {
                            match.TeamA.Members.Add(additional_player);
                            added.Add(additional_player);
                        }
                    }
                    else if (CanAddPlayer(match, match.TeamB, additional_player))
                    {
                        match.TeamB.Members.Add(additional_player);
                        added.Add(additional_player);
                    }
                }
                else
                {
                    break; //whoops.
                }
            }

            if (IsComplete(match))
            {
                foreach (var to_remove in added)
                {
                    _playerQueue.Remove(to_remove);
                }

                return match;
            }
            else
            {
                var old_basis = _playerQueue.PopFront();
                _playerQueue.PushBack(old_basis);
                return null;
            }
        }

        protected bool CanAddPlayer(Match match, Team team, PlayerTankSelection player)
        {
            var team_tier = match.Tier;
            var player_tier = player.Tank.Tier;

            var tier_spread = Math.Abs(team_tier - player_tier);

            var largest_spread = Rules.Spreads.Values.Max();

            if (player.Tank.Tier > match.Tier)
            {
                return false;
            }

            if (tier_spread > largest_spread) //auto dq crazy spreads.
            {
                return false;
            }

            if (match.Tier < 3)
            {
                if (player.Tank.TankClass == TankClass.Artillery)
                {
                    return player.Tank.Tier < 3 && team.ArtilleryCount < 2;
                }
                else
                {
                    return player.Tank.Tier < 3;
                }
            }

            switch (player.Tank.TankClass)
            {
                case TankClass.LightTank:
                    {
                        var light_count = team.LightCount;
                        var spread_ok = tier_spread <= Rules.Spreads[TankClass.LightTank];
                        var count_ok = light_count <= Rules.MaximumPerTeam[TankClass.LightTank];
                        return spread_ok && count_ok;
                    }
                case TankClass.MediumTank:
                    {
                        var medium_count = team.MediumCount;
                        var spread_ok = tier_spread <= Rules.Spreads[TankClass.MediumTank];
                        var count_ok = medium_count <= Rules.MaximumPerTeam[TankClass.MediumTank];
                        return spread_ok && count_ok;
                    }
                case TankClass.HeavyTank:
                    {
                        var heavy_count = team.HeavyCount;
                        var spread_ok = tier_spread <= Rules.Spreads[TankClass.HeavyTank];
                        var count_ok = heavy_count <= Rules.MaximumPerTeam[TankClass.HeavyTank];
                        return spread_ok && count_ok;
                    }
                case TankClass.TankDestroyer:
                    {
                        var td_count = team.TankDestroyerCount;
                        var spread_ok = tier_spread <= Rules.Spreads[TankClass.TankDestroyer];
                        var count_ok = td_count <= Rules.MaximumPerTeam[TankClass.TankDestroyer];
                        return spread_ok && count_ok;
                    }
                case TankClass.Artillery:
                    {
                        var arty_count = team.ArtilleryCount;
                        var spread_ok = tier_spread <= Rules.Spreads[TankClass.Artillery];
                        var count_ok = arty_count <= Rules.MaximumPerTeam[TankClass.Artillery];
                        return spread_ok && count_ok;
                    }
            }

            return false;
        }
    }
}
