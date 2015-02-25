using MatchMaker.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Test
{
    [TestFixture]
    public class ExtensionTests
    {
        public class WeightedObject
        {
            public String Name { get; set; }
            public Double Weight { get; set; }
        }

        [Test]
        public void WeightedAverage_Test()
        {
            var randomProvider = new Random();
            var objects = new List<WeightedObject>()
            {
                new WeightedObject() { Name = "Foo", Weight = 8 },
                new WeightedObject() { Name = "Bar", Weight = 4 },
                new WeightedObject() { Name = "Baz", Weight = 2 },
            };

            var strings = new List<String>();

            1000.Times(() =>
            {
                var weighted_selection = objects.WeightedRandomSelection(_ => _.Weight, randomProvider: randomProvider);
                strings.Add(weighted_selection.Name);
            });

            var foo_count = strings.Count(_ => _.Equals("Foo"));
            var bar_count = strings.Count(_ => _.Equals("Bar"));
            var baz_count = strings.Count(_ => _.Equals("Baz"));

            Assert.True(foo_count > bar_count && bar_count > baz_count);
        }
    }
}
