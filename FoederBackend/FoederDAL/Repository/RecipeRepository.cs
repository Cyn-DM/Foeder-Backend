using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoederDAL.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly MssqlDbContext _context;

        public RecipeRepository(DbContext context)
        {
            this._context = context as MssqlDbContext;
        }

        public List<Recipe> GetRecipes()
        {
            return _context.Recipes.ToList();
        }
    }
}
