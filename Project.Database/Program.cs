using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Database 
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await CreateData();
            var articles = await ReadArticles();
            var authors = await ReadAuthors();
            var contents = await ReadContents();
            var review = await ReadReview();
            var users = await ReadUsers();
        }

        static async Task<List<Article>> ReadArticles()
        {
            using (var context = new ProjectDbContext())
            {
                var articles = await context.Articles
                    .Where(article => !string.IsNullOrWhiteSpace(article.Title))
                    .ToListAsync();
                return articles;
            }
        }

        static async Task<List<Author>> ReadAuthors()
        {
            using (var context = new ProjectDbContext())
            {
                var authors = await context.Authors
                    .Where(author => !string.IsNullOrWhiteSpace(author.FirstName))
                    .ToListAsync();
                return authors;
            }
        }

        static async Task<List<Disability>> ReadDisability()
        {
            using (var context = new ProjectDbContext())
            {
                var disabilities = await context.Disabilities
                    .Where(disability => !string.IsNullOrWhiteSpace(disability.Name))
                    .ToListAsync();
                return disabilities;
            }
        }

        static async Task<List<Review>> ReadReview()
        {
            using (var context = new ProjectDbContext())
            {
                var reviews = await context.Reviews
                    .Where(review => !string.IsNullOrWhiteSpace(review.Comment))
                    .ToListAsync();
                return reviews;
            }
        }

        static async Task<List<Content>> ReadContents()
        {
            using (var context = new ProjectDbContext())
            {
                var contents = await context.Contents
                    .Where(content => !string.IsNullOrWhiteSpace(content.Name))
                    .ToListAsync();
                return contents;
            }
        }

        static async Task<List<User>> ReadUsers()
        {
            using (var context = new ProjectDbContext())
            {
                var users = await context.Users
                    .Where(user => !string.IsNullOrWhiteSpace(user.FirstName))
                    .ToListAsync();
                return users;
            }
        }

        static async Task CreateData()
        {
            using (var context = new ProjectDbContext())
            {
                context.Database.EnsureDeleted();
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
                        PositivityLevel = random.Next(100, 500) / 100.0
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
                        ArticleId = Guid.NewGuid(),
                        AveragePositivityLevel = random.Next(100, 500) / 100.0
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
                        AveragePositivityLevel = random.Next(100, 500) / 100.0,
                        ArticleId = authors[random.Next(authors.Count)].Id,
                        DisabilityId = disabilities[random.Next(disabilities.Count)].Id,
                        ContentId = contents[random.Next(contents.Count)].Id
                    };

                    await context.Articles.AddAsync(article);
                }
                await context.SaveChangesAsync();

                // authors again
                var articles = context.Articles.ToList();

                foreach (var author in context.Authors) 
                {
                    int max = articles.Count;
                    int number = random.Next(max);
                    Guid id = articles[number].Id;
                    author.ArticleId = id;
                }

                await context.SaveChangesAsync();
            }
        }
    }
}



