using FoederDomain.DomainModels;

namespace FoederDomain.Interfaces;

public interface IRecipeRepository
{
    List<Recipe> GetRecipes();
}