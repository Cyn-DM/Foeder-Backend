using FoederBusiness.Dtos;

namespace FoederBusiness.Interfaces;

public interface IRecipeService
{
    Task<List<GetRecipesResponse>> GetRecipes();
}