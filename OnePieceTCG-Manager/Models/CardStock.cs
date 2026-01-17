using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnePieceTCG_Manager.Models
{
    [Table("CardStock")]
    public class CardStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string cardId { get; set; }

        [Required]
        [StringLength(100)]
        public string cardName { get; set; }

        [StringLength(50)]
        public string rarity { get; set; }

        [StringLength(50)]
        public string type { get; set; }

        [StringLength(50)]
        public string subType { get; set; }

        [StringLength(50)]
        public string attribute { get; set; }

        [StringLength(50)]
        public string color { get; set; }

        public int cost { get; set; }
        public int counter { get; set; }
        
        [StringLength(2)]
        public string life { get; set; }
        public int power { get; set; }

        [StringLength(50)]
        public string setDesc { get; set; }

        [Required]
        public bool isAlter { get; set; }

        public string description { get; set; }

        [Required]
        public int units { get; set; }

        [Required]
        public int usedCards { get; set; }   // Cartas en uso por decks (controlado por triggers)

        [Required]
        [StringLength(255)]
        public string cardImage { get; set; }

        [Required]
        public DateTime insertedCardDate { get; set; }    // Fecha de alta

        [Required]
        public DateTime lastUpdatedCardDate { get; set; } // Última modificación
    }
}
