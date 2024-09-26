using FoederDomain.DomainModels;

namespace FoederBusiness.Interfaces;

public interface IRecipeService
{
    List<Recipe> GetRecipes();
}