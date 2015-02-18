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

        private List<T> _population = new List<T>();
        private List<T> _leased = new List<T>();

        public Int32 Count
        {
            get { return _population.Count; }
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
                _population.Add(item);
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
                if (!_isLeased(itemPredicate) && _exists(itemPredicate))
                {
                    var item = _population.FirstOrDefault(_ => itemPredicate(_));
                    _syncLock.EnterWriteLock();
                    try
                    {
                        _leased.Add(item);
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

        public void Return(T item)
        {
            Func<T, Boolean> predicate = (i) => i.Equals(item);

            _syncLock.EnterUpgradeableReadLock();
            try
            {
                if (_isLeased(predicate) && _exists(predicate))
                {
                    var leased = _leased.FirstOrDefault(predicate);
                    _syncLock.EnterWriteLock();
                    try
                    {
                        _leased.Remove(leased);
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

        public bool _isLeased(Func<T, Boolean> itemPredicate)
        {
            return _leased.Any(_ => itemPredicate(_));
        }

        public bool _exists(Func<T, Boolean> itemPredicate)
        {
            return _population.Any(_ => itemPredicate(_));
        }
    }
}
