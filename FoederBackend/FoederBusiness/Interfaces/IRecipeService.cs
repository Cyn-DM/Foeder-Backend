using FoederBusiness.Models;

namespace FoederBusiness.Interfaces;

public interface IRecipeService
{
    List<Recipe> GetRecipes();
}