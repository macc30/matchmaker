using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models.Api
{
    public class ExpectedDamageResponse
    {
        public ExpectedDamageHeader header { get; set; }
        public List<ExpectedDamageEntry> data { get; set; }
    }

    public class ExpectedDamageHeader
    {
        public Int32 version { get; set; }
    }

    public class ExpectedDamageEntry
    {
        public Int32 IDNum { get; set; }
        public Double expFrag { get; set; }
        public Double expDamage { get; set; }
        public Double expSpot { get; set; }
        public Double expDef { get; set; }
        public Double expWinRate { get; set; }
    }
}
