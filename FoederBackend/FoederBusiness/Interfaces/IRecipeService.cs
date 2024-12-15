using FoederBusiness.Dtos;
using FoederBusiness.Helpers;
using FoederDomain.DomainModels;

namespace FoederBusiness.Interfaces;

public interface IRecipeService
{
    Task<List<GetRecipesResponse>> GetRecipes();
    Task AddRecipe(Recipe recipe);
}