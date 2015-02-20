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
            var uniform1 = _getUniformValue();
            var uniform2 = _getUniformValue();

            var standard_normal = Math.Sqrt(-2.0 * Math.Log(uniform1)) * Math.Sin(2.0 * Math.PI * uniform2);

            return this.MeanValue + this.StandardDeviation * standard_normal;
        }
    }
}