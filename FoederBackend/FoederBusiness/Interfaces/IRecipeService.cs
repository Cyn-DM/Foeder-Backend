using FoederDomain.DomainModels;

namespace FoederBusiness.Interfaces;

public interface IRecipeService
{
    List<GetRecipesResponse> GetRecipes();
}