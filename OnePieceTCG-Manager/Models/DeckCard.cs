using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnePieceTCG_Manager.Models
{
    [Table("DeckCards")]
    public class DeckCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid deckId { get; set; }

        [Required]
        public Guid cardStockId { get; set; }

        [Required]
        public int quantity { get; set; }

        /* =====================
           Navegación EF
           ===================== */

        [ForeignKey(nameof(deckId))]
        public Deck Deck { get; set; }

        [ForeignKey(nameof(cardStockId))]
        public CardStock CardStock { get; set; }
    }
}
