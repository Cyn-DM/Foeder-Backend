using FoederBusiness.Dtos;
using FoederBusiness.Helpers;
using FoederDomain.DomainModels;

namespace FoederBusiness.Interfaces;

public interface IRecipeService
{
    Task<List<GetRecipesResponse>> GetRecipes(Guid householdId);
    Task AddRecipe(Recipe recipe);
    Task<Recipe> GetRecipe(Guid recipeId);
    Task UpdateRecipe(Recipe recipe);
    Task DeleteRecipe(Guid recipeId);
}