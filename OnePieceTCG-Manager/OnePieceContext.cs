using OnePieceTCG_Manager.Models;
using System.Data.Entity;

namespace OnePieceTCG_Manager.Data
{
    public class OnePieceContext : DbContext
    {
        public OnePieceContext() : base("name=SQLServerConnection") { }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<CardStock> CardStock { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<DeckCard> DeckCards { get; set; }

    }
}
