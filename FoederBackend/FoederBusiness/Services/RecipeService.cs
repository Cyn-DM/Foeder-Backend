using FoederBusiness.Interfaces;
using FoederBusiness.Models;

namespace FoederBusiness;

public class RecipeService : IRecipeService
{
    public RecipeService(){}

    private List<Recipe> _recipes = new List<Recipe>()
    {
        new Recipe()
        {
            Id = 1,
            Name = "Chicken Risotto",
            Description = "Creamy dish with chicken.",
            Ingredients = new List<Ingredient>()
            {
                new Ingredient()
                {
                    Amount = "1kg",
                    Name = "Chicken"
                },

                new Ingredient()
                {
                    Amount = "500gr",
                    Name = "Risotto"
                }
            },
            Steps = new List<string>()
            {
                "Add chicken",
                "Add Risotto"
            }
        },

        new Recipe()
        {
            Id = 2,
            Name = "Spaghetti Bolognese",
            Description = "Classic Italian pasta dish with a rich meat sauce.",
            Ingredients = new List<Ingredient>()
            {
                new Ingredient()
                {
                    Amount = "200g",
                    Name = "Spaghetti"
                },
                new Ingredient()
                {
                    Amount = "150g",
                    Name = "Ground Beef"
                },
                new Ingredient()
                {
                    Amount = "1",
                    Name = "Onion, diced"
                },
                new Ingredient()
                {
                    Amount = "2 cloves",
                    Name = "Garlic, minced"
                },
                new Ingredient()
                {
                    Amount = "400g",
                    Name = "Canned Tomatoes"
                },
                new Ingredient()
                {
                    Amount = "2 tbsp",
                    Name = "Olive Oil"
                },
                new Ingredient()
                {
                    Amount = "To taste",
                    Name = "Salt and Pepper"
                }
            },
            Steps = new List<string>()
            {
                "Cook spaghetti according to package instructions.",
                "Heat olive oil in a pan over medium heat.",
                "Sauté onions and garlic until translucent.",
                "Add ground beef and cook until browned.",
                "Stir in canned tomatoes and simmer for 15 minutes.",
                "Season with salt and pepper.",
                "Serve sauce over spaghetti."
            }
        },
        new Recipe()
        {
            Id = 3,
            Name = "Classic Pancakes",
            Description = "Fluffy pancakes perfect for a hearty breakfast.",
            Ingredients = new List<Ingredient>()
            {
                new Ingredient()
                {
                    Amount = "1 1/2 cups",
                    Name = "All-purpose Flour"
                },
                new Ingredient()
                {
                    Amount = "3 1/2 tsp",
                    Name = "Baking Powder"
                },
                new Ingredient()
                {
                    Amount = "1 tsp",
                    Name = "Salt"
                },
                new Ingredient()
                {
                    Amount = "1 tbsp",
                    Name = "White Sugar"
                },
                new Ingredient()
                {
                    Amount = "1 1/4 cups",
                    Name = "Milk"
                },
                new Ingredient()
                {
                    Amount = "1",
                    Name = "Egg"
                },
                new Ingredient()
                {
                    Amount = "3 tbsp",
                    Name = "Melted Butter"
                }
            },
            Steps = new List<string>()
            {
                "In a large bowl, sift together the flour, baking powder, salt, and sugar.",
                "Make a well in the center and pour in the milk, egg, and melted butter; mix until smooth.",
                "Heat a lightly oiled griddle or frying pan over medium-high heat.",
                "Pour or scoop the batter onto the griddle, using approximately 1/4 cup for each pancake.",
                "Brown on both sides and serve hot."
            }
        }
    };

    public List<Recipe> GetRecipes()
    {
        return _recipes;
    }
}