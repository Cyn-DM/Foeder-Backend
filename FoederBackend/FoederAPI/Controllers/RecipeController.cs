using FoederBusiness.Dtos;
using FoederBusiness.Interfaces;
using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
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
    public async Task<ActionResult<List<GetRecipesResponse>>> GetRecipes()
    {
        return await _recipeService.GetRecipes();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddRecipe(Recipe recipe)
    {
        try
        {
            await _recipeService.AddRecipe(recipe);
            return Ok();
        }
        catch (InvalidObjectException ex)
        {
            return BadRequest(ex.ValidationResults);
        }
        catch (HouseholdNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}