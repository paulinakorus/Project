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

            await DeleteContent(content);
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
                    var user = userList[random.Next(userList.Count)];
                    var review = new Review()
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        ReviewUser = user,
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
                    var author = authors[random.Next(authors.Count)];
                    var disability = disabilities[random.Next(disabilities.Count)];
                    var content = contents[random.Next(contents.Count)];
                    var article = new Article()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Title " + i.ToString(),
                        Description = "Description " + i.ToString(),
                        AveragePositivityLevel = random.Next(100, 500) / 100.0,
                        AuthorId = author.Id,
                        ArticleAuthor = author,
                        DisabilityId = disability.Id,
                        ArticleDisability = disability,
                        ContentId = content.Id,
                        ArticleContent = content
                    };

                    await context.Articles.AddAsync(article);
                }
                await context.SaveChangesAsync();
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
                    .FirstOrDefaultAsync(article => article.Id.Equals(oldArticle.Id));

                if (articleToChange != null)
                {
                    articleToChange.Title = newArticle.Title;
                    articleToChange.Description = newArticle.Description;
                    articleToChange.AuthorId = newArticle.AuthorId;
                    articleToChange.ArticleAuthor = newArticle.ArticleAuthor;
                    articleToChange.ContentId = newArticle.ContentId;
                    articleToChange.ArticleContent = newArticle.ArticleContent;
                    articleToChange.DisabilityId = newArticle.DisabilityId;
                    articleToChange.ArticleDisability = newArticle.ArticleDisability;
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
                    .FirstOrDefaultAsync(author => author.Id.Equals(oldAuthor.Id));

                if (authorToChange != null)
                {
                    authorToChange.FirstName = newAuthor.FirstName;
                    authorToChange.LastName = newAuthor.LastName;
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
                    .FirstOrDefaultAsync(content => content.Id.Equals(oldContent.Id));

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
                    .FirstOrDefaultAsync(disability => disability.Id.Equals(oldDisability.Id));

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
                    .FirstOrDefaultAsync(review => review.Id.Equals(oldReview.Id));

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
                    .FirstOrDefaultAsync(user => user.Id.Equals(oldUser.Id));

                if (userToChange != null)
                {
                    userToChange.FirstName = newUser.FirstName;
                    userToChange.LastName = newUser.LastName;
                    userToChange.ReviewsNumber = newUser.ReviewsNumber;
                }

                await context.SaveChangesAsync();
            }
        }

        // DELETE

        private static async Task DeleteArticle(Article articleToDelete)
        {
            using (var context = new ProjectDbContext())
            {
                var article = await context.Articles
                    .FirstOrDefaultAsync(article => article.Id.Equals(articleToDelete.Id));
            
                if (article != null)
                {
                    context.Articles.Remove(article);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task DeleteAuthor(Author authorToDelete)
        {
            using (var context = new ProjectDbContext())
            {
                var author = await context.Authors
                    .FirstOrDefaultAsync(auth => auth.Id.Equals(authorToDelete.Id));

                if (author != null)
                {
                    context.Authors.Remove(author);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task DeleteContent(Content contentToDelete)
        {
            using (var context = new ProjectDbContext())
            {
                var content = await context.Contents
                    .FirstOrDefaultAsync(cont => cont.Id.Equals(contentToDelete.Id));

                if (content != null)
                {
                    context.Contents.Remove(content);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task DeleteDisability(Disability disabilityToDelete)
        {
            using (var context = new ProjectDbContext())
            {
                var disability = await context.Disabilities
                    .FirstOrDefaultAsync(dis => dis.Id.Equals(disabilityToDelete.Id));

                if (disability != null)
                {
                    context.Disabilities.Remove(disability);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task DeleteReview(Review reviewToDelete)
        {
            using (var context = new ProjectDbContext())
            {
                var review = await context.Reviews
                    .FirstOrDefaultAsync(rev => rev.Id.Equals(reviewToDelete.Id));

                if (review != null)
                {
                    context.Reviews.Remove(review);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task DeleteUser(Review userToDelete)
        {
            using (var context = new ProjectDbContext())
            {
                var user = await context.Users
                    .FirstOrDefaultAsync(us => us.Id.Equals(userToDelete.Id));

                if (user != null)
                {
                    context.Users.Remove(user);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}



