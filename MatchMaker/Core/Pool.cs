using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MatchMaker.Core
{
    public class PoolLease<T> : IDisposable
    {
        private Pool<T> _pool;

        public PoolLease(T item, Pool<T> pool)
        {
            this.Item = item;
            this._pool = pool;
        }
        public T Item { get; private set; }

        public void Dispose()
        {
            _pool.Return(Item);
        }
    }

    public class Pool<T>
    {
        private ReaderWriterLockSlim _syncLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private Random _randomProvider = new Random();

        private List<T> _available = new List<T>();
        private List<T> _leased = new List<T>();

        public Int32 Count
        {
            get
            {
                return AvailableCount + LeasedCount;
            }
        }

        public Int32 AvailableCount
        {
            get { return _available.Count; }
        }

        public Int32 LeasedCount
        {
            get { return _leased.Count; }
        }

        public void Add(T item)
        {
            _syncLock.EnterWriteLock();
            try
            {
                _available.Add(item);
            }
            finally
            {
                _syncLock.ExitWriteLock();
            }
        }

        public PoolLease<T> Lease(Func<T, Boolean> itemPredicate)
        {
            _syncLock.EnterUpgradeableReadLock();
            try
            {
                if (AvailableCount == 0)
                {
                    return null;
                }

                if (!IsLeased(itemPredicate) && IsAvailable(itemPredicate))
                {
                    var item = _available.FirstOrDefault(_ => itemPredicate(_));
                    _syncLock.EnterWriteLock();
                    try
                    {
                        _leased.Add(item);
                        _available.Remove(item);
                    }
                    finally
                    {
                        _syncLock.ExitWriteLock();
                    }
                    return new PoolLease<T>(item, this);
                }
            }
            finally
            {
                _syncLock.ExitUpgradeableReadLock();
            }
            return null;
        }

        public PoolLease<T> LeaseRandom()
        {
            _syncLock.EnterUpgradeableReadLock();
            try
            {
                if (AvailableCount == 0)
                {
                    return null;
                }

                _syncLock.EnterWriteLock();
                try
                {
                    var random_index = _randomProvider.Next(_available.Count);
                    var item = _available[random_index];
                    _available.Remove(item);
                    _leased.Add(item);

                    return new PoolLease<T>(item, this);
                }
                finally
                {
                    _syncLock.ExitWriteLock();
                }
            }
            finally
            {
                _syncLock.ExitUpgradeableReadLock();
            }
        }

        public void Return(T item)
        {
            Func<T, Boolean> predicate = (i) => i.Equals(item);

            _syncLock.EnterUpgradeableReadLock();
            try
            {
                if (IsLeased(predicate))
                {
                    var leased = _leased.FirstOrDefault(predicate);
                    _syncLock.EnterWriteLock();
                    try
                    {
                        _leased.Remove(leased);
                        _available.Add(leased);
                    }
                    finally
                    {
                        _syncLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _syncLock.ExitUpgradeableReadLock();
            }
        }

        public bool IsLeased(Func<T, Boolean> itemPredicate)
        {
            return _leased.Any(_ => itemPredicate(_));
        }

        public bool IsAvailable(Func<T, Boolean> itemPredicate)
        {
            return _available.Any(_ => itemPredicate(_)); 
        }
    }
}
