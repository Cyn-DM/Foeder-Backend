using FoederAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoederAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    

    [HttpGet]
    public ActionResult<List<Recipe>> GetRecipes()
    {
        return _recipes;
    }
}