using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Core.Probability
{
    public abstract class ProbabilityDistribution
    {
        private Random _randomSource = new Random();
        protected virtual double _getUniformValue()
        {
            return _randomSource.NextDouble();
        }

        protected virtual double _getValueBetween(double minValue, double maxValue)
        {
            var diff = maxValue - minValue;

            return (_getUniformValue() * diff) + minValue;
        }

        public abstract Double GetValue();
    }
}
