namespace Data.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
        public Author ArticleAuthor { get; set; } = new Author();
        public Guid DisabilityId { get; set; }
        public Disability ArticleDisability { get; set; } = new Disability();
        public Guid ContentId { get; set; }
        public Content ArticleContent { get; set; } = new Content();
        public double AveragePositivityLevel { get; set; }
    }
}


