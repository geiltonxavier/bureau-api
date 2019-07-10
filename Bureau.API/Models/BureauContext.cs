using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bureau.API.Models
{
    public class BureauContext : DbContext
    {
        public BureauContext(DbContextOptions<BureauContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User { UserId = 1, UserName = "admin", Password = "admin" });

            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 1, Name = "Categoria A" });
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 2, Name = "Categoria B" });
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 3, Name = "Categoria C" });

            modelBuilder.Entity<Product>().HasData(new Product { ProductId = 1, Name = "Nome Produto 1", Description = "Descrição Produto 1",  CategoryId = 1, Price = 10 });
            modelBuilder.Entity<Product>().HasData(new Product { ProductId = 2, Name = "Nome Produto 2", Description = "Descrição Produto 2", CategoryId = 2, Price = 20 });
            modelBuilder.Entity<Product>().HasData(new Product { ProductId = 3, Name = "Nome Produto 3", Description = "Descrição Produto 3", CategoryId = 3, Price = 30 });
        }

    }

}
