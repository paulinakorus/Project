using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Database 
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await CreateData();
            var context = new ProjectDbContext();
            var articles = await ReadArticles();
            var authors = await ReadAuthors();
            var contents = await ReadContents();
            var review = await ReadReview();
            var users = await ReadUsers();

            var content = new Content()
            {
                Id = context.Contents.First().Id,
                Name = "Głuchota",
                Description = "Brak słuchu"
            };

            await ChangeContent(context.Contents.First(), content)
        }

        // CREATE

        private static async Task CreateData()
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

                var type = context.DbSets[0].GetType();
            }
        }

        // READ
        private static async Task<List<Article>> ReadArticles()
        {
            using (var context = new ProjectDbContext())
            {
                var articles = await context.Articles
                    .Where(article => !string.IsNullOrWhiteSpace(article.Title))
                    .ToListAsync();
                return articles;
            }
        }

        private static async Task<List<Author>> ReadAuthors()
        {
            using (var context = new ProjectDbContext())
            {
                var authors = await context.Authors
                    .Where(author => !string.IsNullOrWhiteSpace(author.FirstName))
                    .ToListAsync();
                return authors;
            }
        }

        private static async Task<List<Disability>> ReadDisability()
        {
            using (var context = new ProjectDbContext())
            {
                var disabilities = await context.Disabilities
                    .Where(disability => !string.IsNullOrWhiteSpace(disability.Name))
                    .ToListAsync();
                return disabilities;
            }
        }

        private static async Task<List<Review>> ReadReview()
        {
            using (var context = new ProjectDbContext())
            {
                var reviews = await context.Reviews
                    .Where(review => !string.IsNullOrWhiteSpace(review.Comment))
                    .ToListAsync();
                return reviews;
            }
        }

        private static async Task<List<Content>> ReadContents()
        {
            using (var context = new ProjectDbContext())
            {
                var contents = await context.Contents
                    .Where(content => !string.IsNullOrWhiteSpace(content.Name))
                    .ToListAsync();
                return contents;
            }
        }

        private static async Task<List<User>> ReadUsers()
        {
            using (var context = new ProjectDbContext())
            {
                var users = await context.Users
                    .Where(user => !string.IsNullOrWhiteSpace(user.FirstName))
                    .ToListAsync();
                return users;
            }
        }

        // UPDATE
        private static async Task ChangeArticle(Article oldArticle, Article newArticle)
        {
            using (var context = new ProjectDbContext())
            {
                var articleToChange = await context.Articles
                    .FirstOrDefaultAsync(article => article.Equals(oldArticle));

                if (articleToChange != null)
                {
                    articleToChange.Title = newArticle.Title;
                    articleToChange.Description = newArticle.Description;
                    articleToChange.ArticleId = newArticle.ArticleId;
                    articleToChange.ContentId = newArticle.ContentId;
                    articleToChange.DisabilityId = newArticle.DisabilityId;
                    articleToChange.AveragePositivityLevel = newArticle.AveragePositivityLevel;
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task ChangeAuthor(Author oldAuthor, Author newAuthor)
        {
            using (var context = new ProjectDbContext())
            {
                var authorToChange = await context.Authors
                    .FirstOrDefaultAsync(author => author.Equals(oldAuthor));

                if (authorToChange != null)
                {
                    authorToChange.FirstName = newAuthor.FirstName;
                    authorToChange.LastName = newAuthor.LastName;
                    authorToChange.ArticleId = newAuthor.ArticleId;
                    authorToChange.AveragePositivityLevel = newAuthor.AveragePositivityLevel;
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task ChangeContent(Content oldContent, Content newContent)
        {
            using (var context = new ProjectDbContext())
            {
                var contentToChange = await context.Contents
                    .FirstOrDefaultAsync(content => content.Equals(oldContent));

                if (contentToChange != null)
                {
                    contentToChange.Name = newContent.Name;
                    contentToChange.Description = newContent.Description;
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task ChangeDisability(Disability oldDisability, Disability newDisability)
        {
            using (var context = new ProjectDbContext())
            {
                var disabilityToChange = await context.Disabilities
                    .FirstOrDefaultAsync(disability => disability.Equals(oldDisability));

                if (disabilityToChange != null)
                {
                    disabilityToChange.Name = newDisability.Name;
                    disabilityToChange.Description = newDisability.Description;
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task ChangeReview(Review oldReview, Review newReview)
        {
            using (var context = new ProjectDbContext())
            {
                var reviewToChange = await context.Reviews
                    .FirstOrDefaultAsync(review => review.Equals(oldReview));

                if (reviewToChange != null)
                {
                    reviewToChange.Title = newReview.Title;
                    reviewToChange.Comment = newReview.Comment;
                    reviewToChange.PositivityLevel = newReview.PositivityLevel;
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task ChangeUser(User oldUser, User newUser)
        {
            using (var context = new ProjectDbContext())
            {
                var userToChange = await context.Users
                    .FirstOrDefaultAsync(user => user.Equals(oldUser));

                if (userToChange != null)
                {
                    userToChange.FirstName = newUser.FirstName;
                    userToChange.LastName = newUser.LastName;
                    userToChange.ReviewsNumber = newUser.ReviewsNumber;
                }

                await context.SaveChangesAsync();
            }
        }
    }
}



