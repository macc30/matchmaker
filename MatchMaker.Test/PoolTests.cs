using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

using MatchMaker.Core;
using MatchMaker.Core.Probability;

namespace MatchMaker.Test
{
    [TestFixture]
    public class PoolTests
    {
        [Test]
        public void LeaseRandom_Test()
        {
            var pool = new Pool<String>();
            pool.Add("Foo");
            pool.Add("Bar");
            pool.Add("Baz");
            pool.Add("Woo");
            pool.Add("Hello");
            pool.Add("World");

            Assert.IsTrue(pool.AvailableCount == 6);
            Assert.IsTrue(pool.Count == 6);
            Assert.IsTrue(pool.LeasedCount == 0);

            String item;
            using (var random = pool.LeaseRandom())
            {
                Assert.IsNotNull(random.Item);
                item = random.Item;

                Assert.IsTrue(pool.IsLeased(_ => _.Equals(item)));

                Assert.IsTrue(pool.Count == 6);
                Assert.IsTrue(pool.AvailableCount == 5);
                Assert.IsTrue(pool.LeasedCount == 1);
            }

            Assert.IsTrue(pool.Count == 6);
            Assert.IsTrue(pool.AvailableCount == 6);
            Assert.IsTrue(pool.LeasedCount == 0);

            Assert.IsFalse(pool.IsLeased(_ => _.Equals(item)));
        }
    }
}
