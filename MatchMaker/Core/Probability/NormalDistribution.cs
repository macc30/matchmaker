using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Core.Probability
{
    public class NormalDistribution : ProbabilityDistribution
    {
        public NormalDistribution(double meanValue, double standardDeviation)
        {
            this.MeanValue = meanValue;
            this.StandardDeviation = standardDeviation;
        }

        public Double MeanValue { get; set; }
        public Double StandardDeviation { get; set; }

        public override double GetValue()
        {
            return StandardDeviation * _getValueBetween(1, 2.0 * MeanValue) + MeanValue;
        }
    }
}
