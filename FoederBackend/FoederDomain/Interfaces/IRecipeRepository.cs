using FoederDomain.DomainModels;

namespace FoederDomain.Interfaces;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetRecipes(Guid householdId);
    Task AddRecipe(Recipe recipe);
    Task<Recipe?> GetRecipe(Guid recipeId);
    Task UpdateRecipe(Recipe recipe);
    Task DeleteRecipe(Guid recipeId);
}