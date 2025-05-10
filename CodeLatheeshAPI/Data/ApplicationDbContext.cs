using CodeLatheeshAPI.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
namespace CodeLatheeshAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Constructor - Passes options to base class-Helpful when we  configure db context from Program.cs file and we will pass connection information through dbcontext options
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
            public DbSet<BlogPost> BlogPosts { get; set; }
            public DbSet<Category> Categories { get; set; }
    }
}
