using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTCG_Manager.Models
{
    public class CardApi
    {
        public double inventory_price { get; set; }
        public double market_price { get; set; }
        public string card_name { get; set; }
        public string set_name { get; set; }
        public string card_text { get; set; }
        public string set_id { get; set; }
        public string rarity { get; set; }
        public string card_set_id { get; set; }
        public string card_color { get; set; }
        public string card_type { get; set; }
        public string life { get; set; }
        public string card_cost { get; set; }
        public string card_power { get; set; }
        public string sub_types { get; set; }
        public int? counter_amount { get; set; }
        public string attribute { get; set; }
        public string card_image_id { get; set; }
        public string card_image { get; set; }
    }
}
