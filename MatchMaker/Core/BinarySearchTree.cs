using System;
using System.Collections.Generic;

namespace MatchMaker
{
    public class BinarySearchTree<T> : IEnumerable<T>
    {
        public BinarySearchTree() {}

        public BinarySearchTree(IEnumerable<T> values, Func<T, Double> ordinalSelector)
        {

        }

        private BinarySearchNode<T> _root = null;

        public void Add(T value, Double ordinal)
        {

        }

        public void Remove(T value)
        {

        }

        public T Seek(Double ordinal)
        {
            return default(T);
        }

        #region IEnumerable implementation

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class BinarySearchNode<T> : IComparable
    {
        public T Item { get; set; }
        public Double Ordinal { get; set; }

        public BinarySearchNode<T> LessThan { get; set; }
        public BinarySearchNode<T> GreaterThan { get; set; }

        #region IComparable implementation
        public int CompareTo(object obj)
        {
            var typed_obj = obj as BinarySearchNode<T>;
            if (typed_obj == null)
                throw new Exception("Incompatable ICompare Types.");

            if (typed_obj.Ordinal > this.Ordinal)
            {
                return 1;
            }
            else if (typed_obj.Ordinal < this.Ordinal)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}

