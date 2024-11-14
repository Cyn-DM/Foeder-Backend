using FoederDomain.DomainModels;

namespace FoederDomain.Interfaces;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetRecipes();
}