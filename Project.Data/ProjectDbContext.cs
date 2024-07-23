using Microsoft.EntityFrameworkCore;
using Data.Entities;

namespace Data
{
    public class ProjectDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Disability> Disabilities { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }

        public List<object> DbSets { get;  set; }

        public ProjectDbContext()
        {
            DbSets = new List<object>
            {
                Articles,
                Authors,
                Contents,
                Disabilities,
                Reviews,
                Users
            };
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDB)\\mssqllocaldb;Database=ProjectDataBase;Trusted_Connection=True;Integrated Security=true;MultipleActiveResultSets=true;");
        }

    }
}


