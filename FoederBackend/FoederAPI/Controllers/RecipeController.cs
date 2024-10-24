using FoederBusiness.Dtos;
using FoederBusiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoederAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    public RecipeController(IRecipeService recipeService)
    {
        this._recipeService = recipeService;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<List<GetRecipesResponse>> GetRecipes()
    {
        return _recipeService.GetRecipes();
    }
}