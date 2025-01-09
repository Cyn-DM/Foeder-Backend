using FoederBusiness.Services;
using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Moq;

namespace FoederTest;

[TestFixture]
public class HouseholdServiceTest
{
    private (Household validHousehold,
        string bearerToken, HouseholdService service, Household invalidHousehold, string invalidBearerToken, string
        notFoundUserToken, string householdUserToken, Guid userId, Guid noHouseholdUserId) GetSetup()
    {
        var mockHouseholdRepo = new Mock<IHouseholdRepository>();
        var mockAuthRepo = new Mock<IAuthRepository>();
        var service = new HouseholdService(mockHouseholdRepo.Object, mockAuthRepo.Object);
        
        var userId = Guid.NewGuid();
        var noHouseholdUserId = Guid.NewGuid();
        var email = "valid_test@email.com";
        var validBearerToken =
            "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QiLCJlbWFpbCI6InZhbGlkX3Rlc3RAZW1haWwuY29tIiwiSWQiOiI1NzA2MzIxYy02ODFhLTQ0MWMtMWUwZi0wOGRkMGEwY2I5ZjAiLCJuYmYiOjE3MzM5MjM4MzMsImV4cCI6MTczMzkyNTYzMywiaWF0IjoxNzMzOTIzODMzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDU4IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTE3MyJ9.Zi8IeyeYmSiCr44vtSGddSUIAkgeffeG7hzHBpHhSJ0";
        var invalidBearerToken =
            "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QiLCJJZCI6IjU3MDYzMjFjLTY4MWEtNDQxYy0xZTBmLTA4ZGQwYTBjYjlmMCIsIm5iZiI6MTczMzkyMzgzMywiZXhwIjoxNzMzOTI1NjMzLCJpYXQiOjE3MzM5MjM4MzMsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwNTgiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MTczIn0.8JYGxFy0RRQEyvaL4_WXYiHQV9SCTFx9FRehxMFb1nE";
        var notFoundUserToken =
            "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QiLCJlbWFpbCI6ImludmFsaWRfdGVzdEBlbWFpbC5jb20iLCJJZCI6IjU3MDYzMjFjLTY4MWEtNDQxYy0xZTBmLTA4ZGQwYTBjYjlmMCIsIm5iZiI6MTczMzkyMzgzMywiZXhwIjoxNzMzOTI1NjMzLCJpYXQiOjE3MzM5MjM4MzMsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwNTgiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MTczIn0.Hg8dnJ0lulIK-IHKfIza05cNRDjpK_RSslOO1rXzvYI";
        var householdUserToken =
            "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QiLCJlbWFpbCI6ImhvdXNlaG9sZF90ZXN0QGVtYWlsLmNvbSIsIklkIjoiNTcwNjMyMWMtNjgxYS00NDFjLTFlMGYtMDhkZDBhMGNiOWYwIiwibmJmIjoxNzMzOTIzODMzLCJleHAiOjE3MzM5MjU2MzMsImlhdCI6MTczMzkyMzgzMywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA1OCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUxNzMifQ.TV2zuHJpFQEV3xnSc0niXQWyiO_MOCC0G92sPPPxoso";
        var validUser = new User { Email = email, FirstName = "John", LastName = "Doe" , Id = userId };
        var validHousehold = new Household()
        {
            Name = "TestHousehold",
            Recipes = new List<Recipe>(),
        };
        var invalidHousehold = new Household()
        {
            Name = "",
        };
        
        var householdUser = new User { Email = "household_test@email.com", FirstName = "John", LastName = "Doe" , Id = userId, Household = validHousehold };
        var noHouseholdUser = new User
        {
            Email = "household_test@email.com", FirstName = "John", LastName = "Doe", Id = noHouseholdUserId,
        };

        mockAuthRepo.Setup(x => x.FindUserByEmail(It.Is<string>(x => x == "valid_test@email.com")))
            .ReturnsAsync(validUser);

        mockAuthRepo.Setup(x => x.FindUserByEmail(It.Is<string>(x => x == "household_test@email.com")))
            .ReturnsAsync(householdUser);

        mockAuthRepo.Setup(x => x.FindUserById(It.Is<Guid>(x => x == userId))).ReturnsAsync(validUser);
        mockAuthRepo.Setup(x => x.FindUserById(It.Is<Guid>(x => x == noHouseholdUserId))).ReturnsAsync(noHouseholdUser);
        
        Household? noHousehold = null;
        
        mockHouseholdRepo.Setup(x => x.GetHouseholdByUserId(It.Is<Guid>(x => x == userId))).ReturnsAsync(validHousehold);
        mockHouseholdRepo.Setup(x => x.GetHouseholdByUserId(It.Is<Guid>(x => x == noHouseholdUserId))).ReturnsAsync(noHousehold);
        
        return (validHousehold, validBearerToken, service, invalidHousehold, invalidBearerToken, notFoundUserToken, householdUserToken, userId, noHouseholdUserId);
    }

    [Test]
    public void AddHouseholdSuccess()
    {
        var setup = GetSetup();
        
        Assert.DoesNotThrowAsync(() => setup.service.AddHousehold(setup.validHousehold, setup.bearerToken));
    }

    [Test]
    public void AddHouseholdFailInvalidHousehold()
    {
        var setup = GetSetup();
        
        Assert.ThrowsAsync<InvalidObjectException>(() => setup.service.AddHousehold(setup.invalidHousehold, setup.bearerToken));
    }

    [Test]
    public void AddHouseholdFailInvalidBearer()
    {
        var setup = GetSetup();
        
        Assert.ThrowsAsync<InvalidBearerTokenException>(() => setup.service.AddHousehold(setup.validHousehold, setup.invalidBearerToken));
    }

    [Test]
    public void AddHouseholdFailUserNotFound()
    {
        var setup = GetSetup();

        Assert.ThrowsAsync<UserNotFoundException>(() =>
            setup.service.AddHousehold(setup.validHousehold, setup.notFoundUserToken));
    }

    [Test]
    public void AddHouseholdFailUserAlreadyHasHousehold()
    {
        var setup = GetSetup();
        
        Assert.ThrowsAsync<UserAlreadyHasHouseholdException>(() => setup.service.AddHousehold(setup.validHousehold, setup.householdUserToken ));
    }

    [Test]
    public void LeaveHouseholdSuccess()
    {
        var setup = GetSetup();
        
        Assert.DoesNotThrowAsync(() => setup.service.LeaveHousehold(setup.userId));
    }

    [Test]
    public void LeaveHouseholdFailUserNotFound()
    {
        var setup = GetSetup();
        
        Assert.ThrowsAsync<UserNotFoundException>(() => setup.service.LeaveHousehold(Guid.NewGuid()));
    }

    [Test]
    public void LeaveHouseholdNotFound()
    {
        var setup = GetSetup();
        
        Assert.ThrowsAsync<HouseholdNotFoundException>(() => setup.service.LeaveHousehold(setup.noHouseholdUserId));
    }
}