using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoederDAL.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly MssqlDbContext _context;

        public RecipeRepository(MssqlDbContext context)
        {
                this._context = context;
        }

        public async Task<List<Recipe>> GetRecipes()
        {
            return await _context!.Recipes.ToListAsync();
        }

        public async Task AddRecipe(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
        }
    }
}
