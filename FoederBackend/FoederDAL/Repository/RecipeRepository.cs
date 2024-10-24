using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoederDAL.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly MssqlDbContext? _context;

        public RecipeRepository(DbContext context)
        {
            if (context is MssqlDbContext dbContext)
            {
                this._context = dbContext;
            }
            
        }

        public List<Recipe> GetRecipes()
        {
            return _context!.Recipes.ToList();
        }
    }
}
