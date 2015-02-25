using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.Models;
using MatchMaker.Core;

namespace MatchMaker.Algorithms
{
    public class Wargaming : MatchMakerBase
    {
        private Dictionary<Int32, Double> _tier_weight = new Dictionary<int, Double>() {
            { 10, 100 },
            { 9, 60 },
            { 8, 40 },
            { 7, 27 },
            { 6, 18 },
            { 5, 12 },
            { 4, 8 },
            { 3, 5 },
            { 2, 3 },
            { 1, 2 },
        };

        private Dictionary<Int32, Double> _tier_weight_exceptions = new Dictionary<Int32, Double>()
        {
            {9761, 18}, //chaffee
            {52497, 7}, // pz 38h
            {5457, 32.4}, //comet
            {10017, 21.6} //sherman jumbo
        };

        private double _get_base_tier_weight(Tank tank)
        {
            if (_tier_weight_exceptions.ContainsKey(tank.WG_ID))
            {
                return _tier_weight_exceptions[tank.WG_ID];
            }
            else
            {
                return _tier_weight[tank.Tier];
            }
        }

        private double _compute_tank_weight(Tank tank) {
            var base_weight = _get_base_tier_weight(tank);

            if (tank.TankClass == TankClass.HeavyTank || tank.TankClass == TankClass.Artillery)
            {
                return base_weight * 1.2;
            }
            else
            {
                if (tank.TankClass == TankClass.MediumTank && tank.Tier >= 9)
                {
                    return base_weight * 1.2;
                }
                else if (tank.TankClass == TankClass.TankDestroyer && tank.Tier >= 8)
                {
                    return base_weight * 1.2;
                }
                else if (tank.TankClass == TankClass.LightTank && tank.Tier >= 6 && tank.Tier <= 8)
                {
                    return base_weight * 1.2;
                }

                return base_weight;
            }
        }
          
        private double _compute_total_team_weight(Team team)
        {
            var total = 0.0;
            foreach (var member in team.Members)
            {
                var tank_weight = _compute_tank_weight(member.Tank);
                total += tank_weight;
            }

            return total;
        }

        public override Match TryFormMatch()
        {
            if (this.QueueDepth < 30)
            {
                return null;
            }



            return null;
        }

        protected bool CanAddPlayer(Match match, Team team, PlayerTankSelection player)
        {

            return false;
        }
    }
}
