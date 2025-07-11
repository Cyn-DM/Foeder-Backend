﻿using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoederDAL.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly MssqlDbContext _context;

        public RecipeRepository(MssqlDbContext context)
        {
                this._context = context;
        }

        public async Task<List<Recipe>> GetRecipes(Guid householdId)
        {
            var recipes = await _context!.Recipes.Where(recipe => recipe.HouseholdId == householdId).ToListAsync();
            recipes.Reverse();
            return recipes;
        }

        public async Task AddRecipe(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task<Recipe?> GetRecipe(Guid recipeId)
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Household)
                .FirstOrDefaultAsync(r => r.Id == recipeId);
        }

        public async Task UpdateRecipe(Recipe recipe)
        {
            var foundRecipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipe.Id);

            if (foundRecipe == null)
            {
                throw new RecipeNotFoundException();
            }
            
            _context.Recipes.Update(recipe);
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipe(Guid recipeId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id ==  recipeId);
            
            if (recipe == null)
            {
                throw new RecipeNotFoundException();
            }
            
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
        }
    }
}
