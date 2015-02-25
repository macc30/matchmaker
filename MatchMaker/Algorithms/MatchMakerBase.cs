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

        protected bool IsComplete(Match match)
        {
            return match.TotalPlayers == 30;
        }
    }
}