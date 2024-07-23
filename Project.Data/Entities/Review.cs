namespace Data.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User ReviewUser { get; set; } = new User();
        public string Title { get; set; }
        public string Comment { get; set; }
        public double PositivityLevel { get; set; }

    }
}


