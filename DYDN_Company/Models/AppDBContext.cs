using Microsoft.EntityFrameworkCore;
using ReactAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DYDN_Company.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Set Idenity for primary key
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Logined>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            // Set Unique Constraint
            modelBuilder.Entity<Product>().HasIndex(product => product.Name).IsUnique();
            modelBuilder.Entity<Account>().HasIndex(product => product.Email).IsUnique();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Logined> Logineds { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
