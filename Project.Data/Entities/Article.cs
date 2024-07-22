namespace Data.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ArticleId { get; set; }
        public Guid DisabilityId { get; set; }
        public Guid ContentId { get; set; }
        public double AveragePositivityLevel { get; set; }
    }
}


