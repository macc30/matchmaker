using MatchMaker.Models;
using MatchMaker.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker
{
    public class TankDb
    {
        private static string TANK_DETAILS_ENDPOINT = "http://api.worldoftanks.com/wot/encyclopedia/tankinfo/?application_id=ab52f40d8f3675412cc856f86cea0bb1&tank_id={0}";
        private static string TANK_LIST_ENDPOINT = "http://api.worldoftanks.com/wot/encyclopedia/tanks/?language=en&application_id=ab52f40d8f3675412cc856f86cea0bb1";
        private static string EXPECTED_DMG_API_ENDPOINT = "http://www.wnefficiency.net/exp/expected_tank_values_18.json";

        private static object _currentLock = new object();
        private static TankDb _current = null;
        public static TankDb Current
        {
            get
            {
                if (_current == null)
                {
                    lock (_currentLock)
                    {
                        if (_current == null)
                        {
                            _current = new TankDb();
                        }
                    }
                }

                return _current;
            }
        }

        public List<Tank> Tanks { get; set; }

        public Tank RandomTank()
        {
            return Tanks.OrderBy(_ => System.Guid.NewGuid()).First();
        }


        public void InitializeFromWeb()
        {
            var tank_list_json = _fetchJSONText(TANK_LIST_ENDPOINT);
            var tank_lookup = _buildTankList(tank_list_json);
            var expected_damage_json = _fetchJSONText(EXPECTED_DMG_API_ENDPOINT);
            var expected_damage = _getExpectedDamage(expected_damage_json);

            foreach(var kvp in expected_damage)
            {
                if(tank_lookup.ContainsKey(kvp.Key))
                {
                    tank_lookup[kvp.Key].ExpectedDamage = kvp.Value.expDamage;
                }
            }

            this.Tanks = tank_lookup.Values.OrderBy(_ => _.Id).ToList();
        }

        public void SaveToFile(String filename)
        {
            var json_text = Newtonsoft.Json.JsonConvert.SerializeObject(this.Tanks);
            System.IO.File.WriteAllText(filename, json_text);
        }

        public void InitializeFromFile(String filename)
        {
            var json_text = System.IO.File.ReadAllText(filename);
            this.Tanks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Tank>>(json_text);
        }

        private Dictionary<Int32, Tank> _buildTankList(String json)
        {
            var list = new List<Tank>();
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<WGTankEncyclopediaResponse>(json);

            foreach (var tank_id in response.data.Keys)
            {
                var tank_data = response.data[tank_id];
                var tank = new Tank();

                tank.WG_ID = tank_data.tank_id;
                tank.Name = tank_data.short_name_i18n;
                tank.Tier = tank_data.level;
                tank.Health = _getTankHealth(tank.WG_ID);

                tank.TankClass = _marshallTankClass(tank_data.type);
                tank.Nation = _marshallNation(tank_data.nation);

                list.Add(tank);
            }

            return list.ToDictionary(_ => _.WG_ID);
        }

        private Dictionary<Int32, ExpectedDamageEntry> _getExpectedDamage(String json)
        {
            var dict = new Dictionary<Int32, ExpectedDamageEntry>();
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpectedDamageResponse>(json);

            return data.data.ToDictionary(_ => _.IDNum);
        }

        private TankClass? _marshallTankClass(String name)
        {
            switch(name.ToLower())
            {
                case "light":
                case "lighttank":
                    return TankClass.LightTank;
                case "medium":
                case "mediumtank":
                    return TankClass.MediumTank;
                case "heavy":
                case "heavytank":
                    return TankClass.HeavyTank;
                case "at-spg":
                case "td":
                    return TankClass.TankDestroyer;
                case "spg":
                    return TankClass.Artillery;
                default:
                    return null;
            }
        }

        private Nation? _marshallNation(String nationName)
        {
            switch(nationName.ToLower())
            {
                case "china":
                    return Nation.China;
                case "germany":
                    return Nation.Germany;
                case "ussr":
                    return Nation.USSR;
                case "usa":
                    return Nation.USA;
                case "france":
                    return Nation.France;
                case "uk":
                    return Nation.GreatBritain;
                case "japan":
                case "jp":
                    return Nation.Japan;
            }

            return null;
        }

        private int _getTankHealth(Int32 wg_id)
        {
            var json = _fetchJSONText(String.Format(TANK_DETAILS_ENDPOINT, wg_id));
            var raw = _parseJson(json);
            var value = raw["data"][wg_id.ToString()]["max_health"].ToString();
            return Int32.Parse(value);
        }

        private dynamic _parseJson(String json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
        }

        private String _fetchJSONText(String endpoint)
        {
            var req = WebRequest.Create(endpoint);
            req.Method = "GET";
            using (var response = req.GetResponse().GetResponseStream())
            {
                using(var sr = new System.IO.StreamReader(response))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
