using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Models.Api
{
    public class WGTankEncyclopediaResponse
    {
        public String status { get; set; }

        public Int32 count { get; set; }

        public Dictionary<String, WGTank> data { get; set; }
    }

    public class WGTank
    {
        public Int32 tank_id { get; set; }

        public String nation { get; set; }
        public String nation_i18n { get; set; }

        public String name { get; set; }

        public Int32 level { get; set; }

        public String image { get; set; }
        public String image_Small { get; set; }
        public String contour_image { get; set; }

        public Boolean is_premium { get; set; }
        
        public String short_name_i18n { get; set; }
        public String type { get; set; }
    }
}
