using FoederDomain.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace FoederDAL;

public class DataSeeder
{
    private readonly MssqlDbContext? _context;

    public DataSeeder(DbContext context)
    {
        if (context is MssqlDbContext dbContext)
        {
            this._context = dbContext;
        }
        
    }

    public void Seed()
    {

        if (!_context!.Households.Any())
        {
            List<User> list = new List<User>();
            _context.Households.AddRange(

                    new Household
                    {
                        Name = "The Johnsons",
                        Users = list
                    },
                    new Household
                    {
                        Name = "Smith Family",
                        Users = list
                    },
                    new Household
                    {
                        Name = "Williams Household",
                        Users = list
                    },
                    new Household
                    {
                        Name = "Brown Family",
                        Users = list
                    },
                    new Household
                    {
                        Name = "Davis Residence",
                        Users = list
                    },
                    new Household
                    {
                        Name = "Miller Home",
                        Users = list
                    },
                    new Household
                    {
                        Name = "Wilson Family",
                        Users = list
                    },
                    new Household
                    {
                        Name = "Moore Household",
                        Users = list
                    },
                    new Household
                    {
                        Name = "Taylor Home",
                        Users = list
                    },
                    new Household
                    {
                        Name = "Anderson Residence",
                        Users = list
                    }


                );

            _context.SaveChanges();
        }

        if (!_context.Users.Any())
        {

            _context.Users.AddRange(new User
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Household = _context.Find<Household>(Guid.Parse("038295A8-2CE9-44A3-F188-08DCE3C81856"))
                },
                new User
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "jane.doe@example.com",
                    Household = _context.Find<Household>(Guid.Parse("A8FA6CAB-114F-4B83-F189-08DCE3C81856"))
                },
                new User
                {
                    FirstName = "Bob",
                    LastName = "Smith",
                    Email = "bob.smith@example.com",
                    Household = _context.Find<Household>(Guid.Parse("A8F4B750-96F4-4BD5-F18A-08DCE3C81856"))
                },
                new User
                {
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@example.com",
                    Household = _context.Find<Household>(Guid.Parse("B993C05D-6499-401F-F18B-08DCE3C81856"))
                },
                new User
                {
                    FirstName = "Charlie",
                    LastName = "Brown",
                    Email = "charlie.brown@example.com",
                    Household = _context.Find<Household>(Guid.Parse("0EEEA997-3541-44FE-F18C-08DCE3C81856"))
                },
                new User
                {
                    FirstName = "Emily",
                    LastName = "Davis",
                    Email = "emily.davis@example.com",
                    Household = _context.Find<Household>(Guid.Parse("A052863E-37C2-402D-F18D-08DCE3C81856"))
                },
                new User
                {
                    FirstName = "Michael",
                    LastName = "Miller",
                    Email = "michael.miller@example.com",
                    Household = _context.Find<Household>(Guid.Parse("34652C8E-7101-4AB3-F18E-08DCE3C81856"))
                },
                new User
                {
                    FirstName = "Sarah",
                    LastName = "Wilson",
                    Email = "sarah.wilson@example.com",
                    Household = _context.Find<Household>(Guid.Parse("F9A2C068-F77C-494F-F18F-08DCE3C81856"))
                },
                new User
                {
                    FirstName = "David",
                    LastName = "Moore",
                    Email = "david.moore@example.com",
                    Household = _context.Find<Household>(Guid.Parse("5947B20F-E2D3-4821-F190-08DCE3C81856"))
                },
                new User
                {
                    FirstName = "Sophia",
                    LastName = "Taylor",
                    Email = "sophia.taylor@example.com",
                    Household = _context.Find<Household>(Guid.Parse("CE54EB65-3FA4-44FA-F191-08DCE3C81856"))
                });

            _context.SaveChanges();
        }

        if (!_context.Recipes.Any())
        {
            _context.Recipes.AddRange(
                new Recipe
                {
                    Title = "Spaghetti Bolognese",
                    Description = "A classic Italian pasta dish with a rich meat sauce.",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Spaghetti", Amount = "200g" },
                        new Ingredient { Name = "Ground beef", Amount = "300g" },
                        new Ingredient { Name = "Tomato sauce", Amount = "1 cup" }
                    },
                    Steps = new List<string>
                    {
                        "Boil spaghetti",
                        "Cook ground beef",
                        "Mix with tomato sauce"
                    },
                    Household = _context.Find<Household>(Guid.Parse("038295A8-2CE9-44A3-F188-08DCE3C81856"))!
                },
                new Recipe
                {
                    Title = "Chicken Curry",
                    Description = "A flavorful curry with tender chicken pieces.",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Chicken breast", Amount = "400g" },
                        new Ingredient { Name = "Curry powder", Amount = "2 tbsp" },
                        new Ingredient { Name = "Coconut milk", Amount = "1 cup" }
                    },
                    Steps = new List<string>
                    {
                        "Cook chicken",
                        "Add curry powder",
                        "Pour in coconut milk and simmer"
                    },
                    Household = _context.Find<Household>(Guid.Parse("A8FA6CAB-114F-4B83-F189-08DCE3C81856"))!
                },
                new Recipe
                {
                    Title = "Vegetable Stir-fry",
                    Description = "A quick and healthy stir-fry with mixed vegetables.",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Broccoli", Amount = "150g" },
                        new Ingredient { Name = "Carrots", Amount = "2" },
                        new Ingredient { Name = "Soy sauce", Amount = "2 tbsp" }
                    },
                    Steps = new List<string>
                    {
                        "Chop vegetables",
                        "Stir-fry in a pan",
                        "Add soy sauce"
                    },
                    Household = _context.Find<Household>(Guid.Parse("A8F4B750-96F4-4BD5-F18A-08DCE3C81856"))!
                },
                new Recipe
                {
                    Title = "Beef Stew",
                    Description = "A hearty stew with beef and vegetables.",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Beef chunks", Amount = "500g" },
                        new Ingredient { Name = "Potatoes", Amount = "3" },
                        new Ingredient { Name = "Carrots", Amount = "2" }
                    },
                    Steps = new List<string>
                    {
                        "Brown beef",
                        "Add vegetables",
                        "Simmer in broth"
                    },
                    Household = _context.Find<Household>(Guid.Parse("B993C05D-6499-401F-F18B-08DCE3C81856"))!
                },
                new Recipe
                {
                    Title = "Pancakes",
                    Description = "Fluffy breakfast pancakes served with syrup.",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Flour", Amount = "1 cup" },
                        new Ingredient { Name = "Milk", Amount = "1 cup" },
                        new Ingredient { Name = "Egg", Amount = "1" }
                    },
                    Steps = new List<string>
                    {
                        "Mix ingredients",
                        "Cook on a griddle",
                        "Serve with syrup"
                    },
                    Household = _context.Find<Household>(Guid.Parse("0EEEA997-3541-44FE-F18C-08DCE3C81856"))!
                },
                new Recipe
                {
                    Title = "Tacos",
                    Description = "Soft tacos with seasoned meat and toppings.",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Ground beef", Amount = "300g" },
                        new Ingredient { Name = "Taco shells", Amount = "8" },
                        new Ingredient { Name = "Cheese", Amount = "1 cup" }
                    },
                    Steps = new List<string>
                    {
                        "Cook beef",
                        "Assemble tacos",
                        "Serve with toppings"
                    },
                    Household = _context.Find<Household>(Guid.Parse("A052863E-37C2-402D-F18D-08DCE3C81856"))!
                },
                new Recipe
                {
                    Title = "Caesar Salad",
                    Description = "A fresh salad with lettuce, croutons, and Caesar dressing.",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Lettuce", Amount = "1 head" },
                        new Ingredient { Name = "Croutons", Amount = "1 cup" },
                        new Ingredient { Name = "Caesar dressing", Amount = "1/2 cup" }
                    },
                    Steps = new List<string>
                    {
                        "Chop lettuce",
                        "Add croutons",
                        "Toss with dressing"
                    },
                    Household = _context.Find<Household>(Guid.Parse("34652C8E-7101-4AB3-F18E-08DCE3C81856"))!
                },
                new Recipe
                {
                    Title = "Grilled Cheese",
                    Description = "A simple and delicious grilled cheese sandwich.",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Bread", Amount = "2 slices" },
                        new Ingredient { Name = "Cheese", Amount = "2 slices" },
                        new Ingredient { Name = "Butter", Amount = "1 tbsp" }
                    },
                    Steps = new List<string>
                    {
                        "Butter bread",
                        "Add cheese",
                        "Grill until golden"
                    },
                    Household = _context.Find<Household>(Guid.Parse("F9A2C068-F77C-494F-F18F-08DCE3C81856"))!
                },
                new Recipe
                {
                    Title = "Lasagna",
                    Description = "Layered lasagna with meat sauce and cheese.",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Lasagna noodles", Amount = "12" },
                        new Ingredient { Name = "Ricotta cheese", Amount = "1 cup" },
                        new Ingredient { Name = "Ground beef", Amount = "300g" }
                    },
                    Steps = new List<string>
                    {
                        "Cook noodles",
                        "Layer with sauce and cheese",
                        "Bake until bubbly"
                    },
                    Household = _context.Find<Household>(Guid.Parse("5947B20F-E2D3-4821-F190-08DCE3C81856"))!
                },
                new Recipe
                {
                    Title = "Chili",
                    Description = "A spicy chili with ground beef and beans.",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Ground beef", Amount = "400g" },
                        new Ingredient { Name = "Kidney beans", Amount = "1 can" },
                        new Ingredient { Name = "Chili powder", Amount = "2 tbsp" }
                    },
                    Steps = new List<string>
                    {
                        "Cook beef",
                        "Add beans and spices",
                        "Simmer for 30 minutes"
                    },
                    Household = _context.Find<Household>(Guid.Parse("CE54EB65-3FA4-44FA-F191-08DCE3C81856"))!
                });

            _context.SaveChanges();
        }

    }
}