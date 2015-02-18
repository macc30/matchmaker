using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Core.Probability
{
    public class ExponentialDistribution : ProbabilityDistribution
    {
        public ExponentialDistribution(double lambda)
        {
            this.Lambda = lambda;
        }

        public Double Lambda { get; set; }

        public override double GetValue()
        {
            var u = _getUniformValue();
            return Math.Log(1.0 - u) / (-1.0 * Lambda);
        }
    }
}
