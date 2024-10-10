using FoederDomain.DomainModels;
using FoederDomain.Interfaces;

namespace FoederTest.Mocks;

public class RecipeRepositoryMock : IRecipeRepository
{
    private readonly List<Recipe> _recipes;

    public RecipeRepositoryMock()
    {
        _recipes = new List<Recipe>
        {
            new Recipe
            {
                Id = Guid.NewGuid(),
                Title = "Spaghetti Bolognese",
                Description = "A classic Italian pasta dish with a rich, savory meat sauce.",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = Guid.NewGuid(), Name = "Spaghetti", Amount = "200g" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Ground Beef", Amount = "300g" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Tomato Sauce", Amount = "200ml" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Onion", Amount = "1, diced" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Garlic", Amount = "2 cloves, minced" }
                },
                Steps = new List<string>
                {
                    "Cook spaghetti according to package instructions.",
                    "Heat oil in a pan, sauté onions and garlic until fragrant.",
                    "Add ground beef and cook until browned.",
                    "Stir in tomato sauce and simmer for 15 minutes.",
                    "Serve sauce over spaghetti."
                },
                Household = new Household
                {
                    Id = Guid.NewGuid(),
                    Name = "Smith Family",
                    Users = new List<User>
                    {
                        new User { FirstName = "John", LastName = "Smith", Email = "john.smith@example.com" },
                        new User { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
                    }
                }
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Title = "Chicken Caesar Salad",
                Description = "Crisp romaine lettuce topped with grilled chicken, Caesar dressing, and croutons.",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = Guid.NewGuid(), Name = "Romaine Lettuce", Amount = "1 head" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Chicken Breast", Amount = "200g, grilled" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Caesar Dressing", Amount = "50ml" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Croutons", Amount = "50g" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Parmesan Cheese", Amount = "30g, grated" }
                },
                Steps = new List<string>
                {
                    "Chop romaine lettuce and place in a bowl.",
                    "Slice grilled chicken and arrange on top of the lettuce.",
                    "Drizzle with Caesar dressing.",
                    "Sprinkle with croutons and parmesan cheese.",
                    "Toss and serve."
                },
                Household = new Household
                {
                    Id = Guid.NewGuid(),
                    Name = "Johnson Household",
                    Users = new List<User>
                    {
                        new User { FirstName = "Paul", LastName = "Johnson", Email = "paul.johnson@example.com" },
                        new User { FirstName = "Emma", LastName = "Johnson", Email = "emma.johnson@example.com" }
                    }
                }
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Title = "Vegetable Stir Fry",
                Description = "A quick and easy stir-fried vegetable dish with a savory sauce.",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = Guid.NewGuid(), Name = "Broccoli", Amount = "100g" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Carrot", Amount = "1, sliced" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Bell Pepper", Amount = "1, sliced" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Soy Sauce", Amount = "2 tbsp" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Sesame Oil", Amount = "1 tbsp" }
                },
                Steps = new List<string>
                {
                    "Heat sesame oil in a pan over medium heat.",
                    "Add vegetables and stir fry for 5-7 minutes.",
                    "Add soy sauce and stir until well-coated.",
                    "Cook for another 2 minutes and serve."
                },
                Household = new Household
                {
                    Id = Guid.NewGuid(),
                    Name = "Williams Residence",
                    Users = new List<User>
                    {
                        new User { FirstName = "Mark", LastName = "Williams", Email = "mark.williams@example.com" },
                        new User { FirstName = "Sophia", LastName = "Williams", Email = "sophia.williams@example.com" }
                    }
                }
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Title = "Beef Tacos",
                Description = "Seasoned ground beef served in soft tortillas with various toppings.",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = Guid.NewGuid(), Name = "Ground Beef", Amount = "300g" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Taco Seasoning", Amount = "1 packet" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Tortillas", Amount = "4" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Cheddar Cheese", Amount = "50g, shredded" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Lettuce", Amount = "1 cup, chopped" }
                },
                Steps = new List<string>
                {
                    "Cook ground beef in a pan over medium heat until browned.",
                    "Add taco seasoning and stir well.",
                    "Warm tortillas and fill with beef mixture.",
                    "Top with cheese, lettuce, and any other desired toppings.",
                    "Serve immediately."
                },
                Household = new Household
                {
                    Id = Guid.NewGuid(),
                    Name = "Brown Home",
                    Users = new List<User>
                    {
                        new User { FirstName = "James", LastName = "Brown", Email = "james.brown@example.com" },
                        new User { FirstName = "Anna", LastName = "Brown", Email = "anna.brown@example.com" }
                    }
                }
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Title = "Pancakes",
                Description = "Fluffy pancakes perfect for breakfast or brunch.",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = Guid.NewGuid(), Name = "Flour", Amount = "1 cup" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Milk", Amount = "1 cup" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Egg", Amount = "1" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Sugar", Amount = "1 tbsp" },
                    new Ingredient { Id = Guid.NewGuid(), Name = "Baking Powder", Amount = "2 tsp" }
                },
                Steps = new List<string>
                {
                    "In a bowl, whisk together flour, sugar, and baking powder.",
                    "Add milk and egg, whisk until smooth.",
                    "Heat a non-stick pan and pour in pancake batter.",
                    "Cook until bubbles form, then flip and cook until golden.",
                    "Serve with syrup or toppings of choice."
                },
                Household = new Household
                {
                    Id = Guid.NewGuid(),
                    Name = "Garcia Family",
                    Users = new List<User>
                    {
                        new User { FirstName = "Carlos", LastName = "Garcia", Email = "carlos.garcia@example.com" },
                        new User { FirstName = "Maria", LastName = "Garcia", Email = "maria.garcia@example.com" }
                    }
                }
            },

        };

    }

    public List<Recipe> GetRecipes()
    {
        return _recipes;
    }
}