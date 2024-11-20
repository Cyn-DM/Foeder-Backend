using FoederDomain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoederDAL
{
    public class MssqlDbContext: DbContext
    {
        public MssqlDbContext(DbContextOptions<MssqlDbContext> options) : base(options)
        {
        }

        public DbSet<Household> Households { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Household) 
                .WithMany(h => h.Users) 
                .HasForeignKey(u => u.HouseholdId); 
            
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.Household)
                .WithMany(r => r.Recipes)
                .HasForeignKey(r => r.HouseholdId)
                .IsRequired();  
        }
    }
    
   
}