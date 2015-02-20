using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.Core.Probability;

namespace MatchMaker.Models
{
    public class Player
    {
        public static int NominalSkillAverage = 1600;
        public static int NominalSkillStdDev = 22.36;

        public static Player Create()
        {
            var player = new Player();

        }

        public Player()
        {
            var skill_normal = new NormalDistribution(NominalSkillAverage, NominalSkillStdDev);
            this.Id = System.Guid.NewGuid();
            this.NominalSkill = skill_normal.GetValue();

            this.MatchesPlayed = new List<Match>();
        }

        //these are pre-populated
        public Guid Id { get; set; }
        public Int32 NominalSkill { get; set; }

        //these are calculated either on the fly or after all matches are played
        public Int32 WN8 { get; set; }
        public Double WinRate { get; set; } 


        //log of matches played.
        public List<Match> MatchesPlayed { get; set;}
    }
}
