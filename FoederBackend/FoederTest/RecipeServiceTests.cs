using FoederBusiness.Dtos;
using FoederBusiness.Interfaces;
using FoederDomain.Interfaces;
using FoederTest.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace FoederTest
{
    public class RecipeServiceTests
    {
        private readonly IRecipeService _recipeService;
        public RecipeServiceTests()
        {
            var services = new ServiceCollection();

            services.AddTransient<IRecipeService, RecipeServiceMock>();
            services.AddTransient<IRecipeRepository, RecipeRepositoryMock>();

            var serviceProvider = services.BuildServiceProvider();

            _recipeService = serviceProvider.GetService<IRecipeService>()!;

        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task AssertGetRecipeResponses()
        {
            List<GetRecipesResponse> responses = await _recipeService.GetRecipes();

            Assert.IsNotEmpty(responses);
            
        }
    }
}