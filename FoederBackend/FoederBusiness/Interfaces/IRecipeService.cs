using FoederBusiness.Dtos;

namespace FoederBusiness.Interfaces;

public interface IRecipeService
{
    List<GetRecipesResponse> GetRecipes();
}