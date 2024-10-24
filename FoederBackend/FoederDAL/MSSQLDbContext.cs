using FoederDomain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoederDAL
{
    public class MssqlDbContext(IConfiguration config) : DbContext
    {
        public DbSet<Household> Households { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<User> Users { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(config["DbConnectionString"]);
        }


    }

}