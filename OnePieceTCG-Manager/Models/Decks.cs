using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnePieceTCG_Manager.Models
{
    [Table("Decks")]
    public class Deck
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string deckName { get; set; }

        [StringLength(255)]
        public string deckDescription { get; set; }

        public Guid? leaderCardId { get; set; }

        [Required]
        [StringLength(5)]
        public string codUsu { get; set; }

        [Required]
        public bool isActive { get; set; }

        [Required]
        public DateTime createdDate { get; set; }

        [Required]
        public DateTime lastUpdatedDate { get; set; }

        /* =====================
           Navegación EF
           ===================== */

        [ForeignKey(nameof(leaderCardId))]
        public CardStock LeaderCard { get; set; }

        [ForeignKey(nameof(codUsu))]
        public Usuarios Usuario { get; set; }

        public ICollection<DeckCard> DeckCards { get; set; }
    }
}
