using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryAPI.DAL
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}
        public DbSet<Book> Books { get;set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                        .Property(b=>b.Name)
                        .IsRequired()
                        .HasMaxLength(50);

            modelBuilder.Entity<Book>()
                        .Property(b=>b.Price)
                        .IsRequired();

            modelBuilder.Entity<Book>()
                        .Property(b => b.CreatedDate)
                        .HasDefaultValueSql("getdate()");
        }
    }
}
