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
                {TankClass.HeavyTank, 8},
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

            public static Int32 MaxPerTier = 10;
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

        protected bool IsComplete(Match match)
        {
            return match.TotalPlayers == 30;
        }
    }
}