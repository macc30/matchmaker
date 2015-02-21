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
        public static double NominalSkillAverage = 1600;
        public static double NominalSkillStdDev = 22.36;

        public static Player Create()
        {
            var player = new Player();
            player.Id = System.Guid.NewGuid();
            var skill_normal = new NormalDistribution(NominalSkillAverage, NominalSkillStdDev);
            player.NominalSkill = skill_normal.GetValue();
            return player;
        }

        public Player()
        {
            this.MatchesPlayed = new List<Match>();
        }

        //these are pre-populated
        public Guid Id { get; set; }
        public Double NominalSkill { get; set; }

        //these are calculated either on the fly or after all matches are played
        public Int32 WN8 { get; set; }
        public Double WinRate { get; set; } 

        //log of matches played.
        public List<Match> MatchesPlayed { get; set;}
    }
}
