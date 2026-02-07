using System;
using System.Collections.Generic;
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

    public class DeckEditDto
    {
        public Guid Id { get; set; }
        public string deckName { get; set; }
        public string deckDescription { get; set; }
        public string codUsu { get; set; }
        public bool isActive { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastUpdatedDate { get; set; }

        public Guid? LeaderCardId { get; set; }
        public List<DeckCardDto> DeckCards { get; set; }
    }

    public class DeckCardDto
    {
        public Guid cardStockId { get; set; }
        public int quantity { get; set; }
    }

}
