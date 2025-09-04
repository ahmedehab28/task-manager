namespace Domain.Entities
{
    public  class CardMember
    {
        public Guid CardId { get; set; }
        public Guid UserId { get; set; }
        public Card Card { get; set; } // nav
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
