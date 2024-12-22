using FoederDomain.DomainModels;

namespace FoederDomain.Interfaces;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetRecipes(Guid householdId);
    Task AddRecipe(Recipe recipe);
}