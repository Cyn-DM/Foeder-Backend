using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederDAL.Repository;
using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Moq;

namespace FoederTest;

[TestFixture]
public class RecipeServiceTests
{
    private (RecipeService service, Household validHousehold, Household invalidHousehold) GetSetup()
    {
        var mockHouseholdRepo = new Mock<IHouseholdRepository>();
        var mockRecipeRepo = new Mock<IRecipeRepository>();
        var recipeService = new RecipeService(mockRecipeRepo.Object, mockHouseholdRepo.Object);
        
        var validHousehold = new Household()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
        };

        var invalidHousehold = new Household()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
        };
        
        mockHouseholdRepo
            .Setup(x => x.GetHouseholdById(It.Is<Guid>( x => x == validHousehold.Id)))
            .ReturnsAsync(validHousehold);
        mockHouseholdRepo
            .Setup(x => x.GetHouseholdById(It.Is<Guid>(x => x == invalidHousehold.Id)))
            .ReturnsAsync((Household?)null);
        
        return (recipeService, validHousehold, invalidHousehold);
    }

    [Test]
    public void AddRecipeTestSuccessful()
    {
        var setup = GetSetup();
        
        var validRecipe = new Recipe()
        {
            Household = setup.validHousehold,
            Description = "Classic",
            HouseholdId = setup.validHousehold.Id,
            Ingredients = new List<Ingredient>()
            {
                new Ingredient()
                {
                    Amount = "10 spoonfuls",
                    Name = "Milk",
                }
            },
            Title = "Glass of Milk",
            Steps = new List<string>()
            {
                "Add spoons of milk to glass."
            },
        };
        
        Assert.DoesNotThrowAsync(() => setup.service.AddRecipe(validRecipe));
    }

    [Test]
    public void AddRecipeTestFailInvalidHousehold()
    {
        var setup = GetSetup();
        
        var validRecipeInvalidHousehold = new Recipe()
        {
            Household = setup.invalidHousehold,
            Description = "Classic",
            HouseholdId = setup.invalidHousehold.Id,
            Ingredients = new List<Ingredient>()
            {
                new Ingredient()
                {
                    Amount = "10 spoonfuls",
                    Name = "Milk",
                }
            },
            Title = "Glass of Milk",
            Steps = new List<string>()
            {
                "Add spoons of milk to glass."
            },
        };
        
        Assert.ThrowsAsync<HouseholdNotFoundException>(() => setup.service.AddRecipe(validRecipeInvalidHousehold));
    }

    [Test]
    public void AddRecipeTestFailInvalidRecipeNoTitle()
    {
        var setup = GetSetup();
        
        var invalidRecipe = new Recipe()
        {
            Household = setup.validHousehold,
            Description = "Classic",
            HouseholdId = setup.validHousehold.Id,
            Ingredients = new List<Ingredient>()
            {
                new Ingredient()
                {
                    Amount = "10 spoonfuls",
                    Name = "Milk",
                }
            },
            Steps = new List<string>()
            {
                "Add spoons of milk to glass."
            },
        };
        
        Assert.ThrowsAsync<InvalidObjectException>(() => setup.service.AddRecipe(invalidRecipe));
    }
    
    
    [Test]
    public void AddRecipeTestFailInvalidRecipeEmptyTitle()
    {
        var setup = GetSetup();
        
        var invalidRecipe = new Recipe()
        {
            Household = setup.validHousehold,
            Description = "Classic",
            HouseholdId = setup.validHousehold.Id,
            Ingredients = new List<Ingredient>()
            {
                new Ingredient()
                {
                    Amount = "10 spoonfuls",
                    Name = "Milk",
                }
            },
            Title = "",
            Steps = new List<string>()
            {
                "Add spoons of milk to glass."
            },
        };
        
        Assert.ThrowsAsync<InvalidObjectException>(() => setup.service.AddRecipe(invalidRecipe));
    }
    
    [Test]
    public void AddRecipeTestFailInvalidRecipeTooLongTitle()
    {
        var setup = GetSetup();
        
        var invalidRecipe = new Recipe()
        {
            Household = setup.validHousehold,
            Description = "Classic",
            HouseholdId = setup.validHousehold.Id,
            Ingredients = new List<Ingredient>()
            {
                new Ingredient()
                {
                    Amount = "10 spoonfuls",
                    Name = "Milk",
                }
            },
            Title =
                "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest",
            Steps = new List<string>()
            {
                "Add spoons of milk to glass."
            },
        };
        
        Assert.ThrowsAsync<InvalidObjectException>(() => setup.service.AddRecipe(invalidRecipe));
    }
     
    [Test]
    public void AddRecipeTestFailInvalidRecipeNoIngredients()
    {
        var setup = GetSetup();
        
        var invalidRecipe = new Recipe()
        {
            Household = setup.validHousehold,
            Description = "Classic",
            HouseholdId = setup.validHousehold.Id,
            Ingredients = new List<Ingredient>(),
            Title = "Test",
            Steps = new List<string>()
            {
                "Add spoons of milk to glass."
            },
        };
        
        Assert.ThrowsAsync<InvalidObjectException>(() => setup.service.AddRecipe(invalidRecipe));
    }
}