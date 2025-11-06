using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTCG_Manager.Models
{
    [Table("CardStock")]
    public class CardStock
    {
        [Key]
        public string cardId { get; set; }
        public string cardName { get; set; }
        public string rarity { get; set; }
        public string type { get; set; }
        public string subType { get; set; }
        public string attribute { get; set; }
        public string color { get; set; }
        public int cost { get; set; }
        public int counter { get; set; }
        public int power { get; set; }
        public string setDesc { get; set; }
        public bool isAlter { get; set; }
        public string description { get; set; }
        public int units { get; set; }
        public string cardImage { get; set; }
    }
}
