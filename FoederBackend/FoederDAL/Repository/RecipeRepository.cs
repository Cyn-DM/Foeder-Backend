using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoederDAL.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly MssqlDbContext? _context;

        public RecipeRepository(MssqlDbContext context)
        {
                this._context = context;
        }

        public List<Recipe> GetRecipes()
        {
            return _context!.Recipes.ToList();
        }
    }
}
