namespace Data.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Article> ArticlesList { get; set; } = new List<Article>();
        public double AveragePositivityLevel { get; set; }
    }
}


