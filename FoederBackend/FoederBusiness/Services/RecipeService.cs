using FoederBusiness.Dtos;
using FoederBusiness.Interfaces;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;

namespace FoederBusiness;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;

    public RecipeService(IRecipeRepository recipeRepository)
    {
        this._recipeRepository = recipeRepository;
    }

    public List<GetRecipesResponse> GetRecipes()
    {
        List<Recipe> domainRecipes = _recipeRepository.GetRecipes();
        List<GetRecipesResponse> recipeDtos = new();

        if (domainRecipes.Count == 0)
        {
            return recipeDtos;
        }

        foreach (var DomainRecipe in domainRecipes)
        {
            GetRecipesResponse response = new GetRecipesResponse()
            {
                Id = DomainRecipe.Id,
                Title = DomainRecipe.Title,
            };

            recipeDtos.Add(response);
        }

        return recipeDtos;
    }
}