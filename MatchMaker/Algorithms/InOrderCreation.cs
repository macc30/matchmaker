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
    }
}
