using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

using MatchMaker.Core;
using MatchMaker.Core.Probability;

namespace MatchMaker.Test
{
    [TestFixture]
    public class ProbabilityTests
    {
        [Test]
        public void NormalDistribution_Test()
        {
            var normal = new NormalDistribution(100, 10);

            var values = new List<Double>();

            100.Times(() =>
                {
                    values.Add(normal.GetValue());
                });

            var std_dev = values.StdDev();
            var average = values.Average();
            var average_delta = Math.Abs(average - 100.0);

            Assert.AreNotEqual(std_dev, 0.0);
            Assert.IsTrue(average_delta < 1.0);
        }

        [Test]
        public void ExponentialDistribution_Test()
        {
            var exp = new ExponentialDistribution(0.01);

            var values = new List<Double>();
            100.Times(() =>
                {
                    values.Add(exp.GetValue());
                });
                    
            Assert.IsNotEmpty(values);
        }
    }
}