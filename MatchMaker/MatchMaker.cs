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
        private ReaderWriterLockSlim _queueLock = new ReaderWriterLockSlim();
        private Queue<PlayerTankSelection> _playerQueue = new Queue<PlayerTankSelection>();

        public Int32 QueueDepth
        {
            get
            {
                _queueLock.EnterReadLock();
                try
                {
                    return _playerQueue.Count;
                }
                finally
                {
                    _queueLock.ExitReadLock();
                }
            }
        }
        public void QueuePlayer(PlayerTankSelection player)
        {
            _queueLock.EnterWriteLock();
            try
            {
                _playerQueue.Enqueue(player);
            }
            finally
            {
                _queueLock.ExitWriteLock();
            }
        }

        public IEnumerable<Match> TryFormMatches()
        {
            return null;
            //return new List<Match>();
        }

        private void _formMatch()
        {

        }
    }
}
