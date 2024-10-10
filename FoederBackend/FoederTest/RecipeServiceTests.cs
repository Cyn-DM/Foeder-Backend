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

            test
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AssertGetRecipeResponses()
        {
            List<GetRecipesResponse> responses = _recipeService.GetRecipes();

            Assert.IsNotEmpty(responses);
        }
    }
}