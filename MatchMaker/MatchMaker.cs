using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using MatchMaker.Models;

namespace MatchMaker
{
    public class MatchMaker
    {
        public static class Rules
        {
            public const Int32 MAX_HEAVY = 7;
            public const Int32 MAX_MEDIUM = 7;
            public const Int32 MAX_LIGHT = 4;
            public const Int32 MAX_TD = 7;
            public const Int32 MAX_ARTY = 3;

            public const Int32 MAX_TIER_SPREAD_LIGHT = 3;
            public const Int32 MAX_TIER_SPREAD_MEDIUM = 2;
            public const Int32 MAX_TIER_SPREAD_HEAVY = 2;
            public const Int32 MAX_TIER_SPREAD_TD = 2;
            public const Int32 MAX_TIER_SPREAD_ARTY = 2;
        }

        private List<PlayerTankSelection> _playerQueue = new List<PlayerTankSelection>();

        /// <summary>
        /// The method here is if a player has been in the queue > 30 secs, we bump them up to here.
        /// </summary>
        private List<PlayerTankSelection> _priorityQueue = new List<PlayerTankSelection>();

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

        public Match TryFormMatch()
        {
            if(QueueDepth < 30) //early elimination
            {
                return null;
            }

            var trial_player = _playerQueue.OrderBy(_ => System.Guid.NewGuid()).First();



            return match;
        }
    }
}
