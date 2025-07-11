﻿using System.ComponentModel.DataAnnotations;
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
    public async Task<ActionResult<List<GetRecipesResponse>>> GetRecipes(Guid householdId)
    {
        return await _recipeService.GetRecipes(householdId);
    }

    [HttpPost("AddRecipe")]
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

    [HttpGet("GetRecipe")]
    [Authorize]
    public async Task<IActionResult> GetRecipe(Guid recipeId)
    {
        try
        {
            return Ok(await _recipeService.GetRecipe(recipeId));
        }
        catch (RecipeNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("UpdateRecipe")]
    [Authorize]
    public async Task<IActionResult> UpdateRecipe(Recipe recipe)
    {
        try
        {
            await _recipeService.UpdateRecipe(recipe);
            return Ok();
        }
        catch (InvalidObjectException ex)
        {
            return BadRequest(ex.ValidationResults);
        }
        catch (RecipeNotFoundException ex)
        {
            return NotFound(ex.Message);
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

    [HttpDelete("DeleteRecipe")]
    [Authorize]
    public async Task<IActionResult> DeleteRecipe(Guid recipeId)
    {
        try
        {
            await _recipeService.DeleteRecipe(recipeId);
            return Ok();
        }
        catch (RecipeNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

}