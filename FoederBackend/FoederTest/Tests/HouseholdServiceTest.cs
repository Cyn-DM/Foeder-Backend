using FoederBusiness.Services;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Moq;

namespace FoederTest;

[TestFixture]
public class HouseholdServiceTest
{
    private (Mock<IHouseholdRepository> mockHouseholdRepo, Mock<IAuthRepository> mockAuthRepo, Household validHousehold, string bearerToken, HouseholdService service)GetSetup()
    {
        var mockHouseholdRepo = new Mock<IHouseholdRepository>();
        var mockAuthRepo = new Mock<IAuthRepository>();
        var service = new HouseholdService(mockHouseholdRepo.Object, mockAuthRepo.Object);
        
        var validHouseholdId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var email = "valid_test@email.com";
        var bearerToken =
            "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QiLCJlbWFpbCI6InZhbGlkX3Rlc3RAZW1haWwuY29tIiwiSWQiOiI1NzA2MzIxYy02ODFhLTQ0MWMtMWUwZi0wOGRkMGEwY2I5ZjAiLCJuYmYiOjE3MzM5MjM4MzMsImV4cCI6MTczMzkyNTYzMywiaWF0IjoxNzMzOTIzODMzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDU4IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTE3MyJ9.Zi8IeyeYmSiCr44vtSGddSUIAkgeffeG7hzHBpHhSJ0";
        var validUser = new User { Email = email, FirstName = "John", LastName = "Doe" , Id = userId };
        var validHousehold = new Household()
        {
            Id = validHouseholdId,
            Name = "TestHousehold",
            Recipes = new List<Recipe>(),
            Users = new List<User>()
            {
                validUser
            }
        };

        mockAuthRepo.Setup(x => x.FindUserByEmail(It.Is<string>(x => x == "valid_test@email.com")))
            .ReturnsAsync(validUser);
        
        return (mockHouseholdRepo, mockAuthRepo, validHousehold, bearerToken, service);
    }

    [Test]
    public void AddHouseholdSuccess()
    {
        var setup = GetSetup();
        
        Assert.DoesNotThrowAsync(() => setup.service.AddHousehold(setup.validHousehold, setup.bearerToken));
    }
}