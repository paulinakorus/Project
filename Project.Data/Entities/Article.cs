namespace Data.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Author> AuthorsList { get; set; } = new List<Author>();
        public List<Disability> DisabilitiesList { get; set; } = new List<Disability>();
        public List<Content> ContentsList { get; set; } = new List<Content>();
        public double AveragePositivityLevel { get; set; }
    }
}


