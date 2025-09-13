namespace Domain.Entities
{
    public  class CardMember
    {
        public Guid CardId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUser Member { get; private set; } = null!;
        public Card Card { get; private set; } = null!;
    }
}
