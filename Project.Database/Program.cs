using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Database 
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await CreateData();
        }

        static async Task CreateData()
        {
            using (var context = new ProjectDbContext())
            {
                context.Database.EnsureCreated();
                Random random = new Random();

                // content
                for (int i = 0; i < 10; i++)
                {
                    var content = new Content()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Name " + i.ToString(),
                        Description = "Description " + i.ToString()
                    };
                    await context.Contents.AddAsync(content);
                }
                await context.SaveChangesAsync();

                // disabilities
                for (int i = 0; i < 10; i++)
                {
                    var disability = new Disability()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Name " + i.ToString(),
                        Description = "Description " + i.ToString()
                    };
                    await context.Disabilities.AddAsync(disability);
                }
                await context.SaveChangesAsync();

                // users
                for (int i = 0; i < 10; i++)
                {
                    var user = new User()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "FirstName " + i.ToString(),
                        LastName = "LastName " + i.ToString(),
                        ReviewsNumber = random.Next(10)
                    };
                    await context.Users.AddAsync(user);
                }
                await context.SaveChangesAsync();

                // reviews
                var userList = context.Users.ToList();
                for (int i = 0; i < 10; i++)
                {
                    var review = new Review()
                    {
                        Id = Guid.NewGuid(),
                        UserId = userList[random.Next(userList.Count)].Id,
                        Title = "Title " + i.ToString(),
                        Comment = "Comment " + i.ToString(),
                        PositivityLevel = random.Next(100, 500) / 100
                    };
                    await context.Reviews.AddAsync(review);
                }
                await context.SaveChangesAsync();

                // authors
                for (int i = 0; i < 10; i++)
                {
                    var author = new Author()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "FirstName " + i.ToString(),
                        LastName = "LastName " + i.ToString(),
                        ArticlesList = new List<Article>() {},
                        AveragePositivityLevel = random.Next(100, 500) / 100
                    };
                    await context.Authors.AddAsync(author);
                }
                await context.SaveChangesAsync();

                // articles
                var authors = context.Authors.ToList();
                var contents = context.Contents.ToList();
                var disabilities = context.Disabilities.ToList();

                for (int i = 0; i < 10; i++)
                {
                    var article = new Article()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Title " + i.ToString(),
                        Description = "Description " + i.ToString(),
                        AveragePositivityLevel = random.Next(100, 500) / 100
                    };

                    var authorsForArticle = new List<Author>();

                    for (int j = 0; j < random.Next(0, 5); j++)
                    {
                        var oneAuthor = authors[random.Next(authors.Count)];
                        authorsForArticle.Add(oneAuthor);
                        oneAuthor.ArticlesList.Add(article);
                    }

                    article.AuthorsList = authorsForArticle;

                    await context.Articles.AddAsync(article);
                }
                
                await context.SaveChangesAsync();
            }
        }
    }
}



