using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

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

    public class DeckRow : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string DeckName { get; set; }
        public string LeaderName { get; set; }
        public string LeaderImageUrl { get; set; }
        public int TotalCards { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsActive { get; set; }

        private Image _leaderImage;
        public Image LeaderImage
        {
            get => _leaderImage;
            set
            {
                _leaderImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LeaderImage)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class DeckSaveDto
    {
        public Guid Id { get; set; }            // Guid.Empty si nuevo
        public string CodUsu { get; set; }
        public string DeckName { get; set; }
        public Guid LeaderCardId { get; set; }
        public List<DeckCardDto> DeckCards { get; set; } = new List<DeckCardDto>();
    }
}
