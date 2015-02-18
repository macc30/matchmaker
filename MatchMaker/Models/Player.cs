using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public Int32 NominalSkill { get; set; }
        public Int32 WN8 { get; set; }
        public Double WinRate { get; set; } 
    }
}
