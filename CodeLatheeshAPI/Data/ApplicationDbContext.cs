﻿using CodeLatheeshAPI.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
namespace CodeLatheeshAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Constructor - Passes options to base class-Helpful when we  configure db context from Program.cs file and we will pass connection information through dbcontext options
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId);
        }
    }
}
