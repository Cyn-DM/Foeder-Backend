using FoederBusiness.Dtos;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederBusiness.Tools;
using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;

namespace FoederBusiness.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IHouseholdRepository _householdRepository;

    public RecipeService(IRecipeRepository recipeRepository, IHouseholdRepository householdRepository)
    {
        this._recipeRepository = recipeRepository;
        _householdRepository = householdRepository;
    }

    public async Task<List<GetRecipesResponse>> GetRecipes(Guid householdId)
    {
        var household = await _householdRepository.GetHouseholdById(householdId);

        if (household == null)
        {
            throw new HouseholdNotFoundException();
        }
        
        List<Recipe> domainRecipes = await _recipeRepository.GetRecipes(householdId);
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

    public async Task AddRecipe(Recipe recipe)
    {
        ValidationUtils.ValidateObject(recipe);

        if (await _householdRepository.GetHouseholdById(recipe.HouseholdId) == null)
        {
            throw new HouseholdNotFoundException();
        }
        
        await _recipeRepository.AddRecipe(recipe);
    }

    public async Task<Recipe> GetRecipe(Guid recipeId)
    {
        var recipe = await _recipeRepository.GetRecipe(recipeId);

        if (recipe == null)
        {
            throw new RecipeNotFoundException();
        }

        return recipe;
    }

    public async Task UpdateRecipe(Recipe recipe)
    {
        ValidationUtils.ValidateObject(recipe);
        
        if (await _householdRepository.GetHouseholdById(recipe.HouseholdId) == null)
        {
            throw new HouseholdNotFoundException();
        }
        
        await _recipeRepository.UpdateRecipe(recipe);
    }

    public async Task DeleteRecipe(Guid recipeId)
    {
        await _recipeRepository.DeleteRecipe(recipeId);
    }
}