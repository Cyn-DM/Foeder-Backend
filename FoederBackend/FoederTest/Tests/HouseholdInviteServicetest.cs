using FoederBusiness.Services;
using FoederDAL.Repository;
using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Moq;

namespace FoederTest;

[TestFixture]
public class HouseholdInviteServicetest
{
    private (Mock<IHouseholdInvitesRepository> mockHouseholdInvitesRepo, Mock<IAuthRepository> mockAuthRepo,
        Mock<IHouseholdRepository> mockHouseholdRepo, Guid userId, HouseholdInvitesService service, string email, Guid
        householdId) GetSetup()
    {
        var mockHouseholdInvitesRepo = new Mock<IHouseholdInvitesRepository>();
        var mockAuthRepo = new Mock<IAuthRepository>();
        var mockHouseholdRepo = new Mock<IHouseholdRepository>();
        var userId = Guid.NewGuid();
        var householdId = Guid.NewGuid();
        var email = "test@example.com";
        var validUser = new User { Email = email, FirstName = "John", LastName = "Doe" , Id = userId };
        var householdUser = new User { Email = email, FirstName = "John", LastName = "Doe", Id = userId , Household = new Household { Id = householdId } };
        var service = new HouseholdInvitesService(mockHouseholdInvitesRepo.Object, mockAuthRepo.Object, mockHouseholdRepo.Object);
        
        mockAuthRepo.Setup(m => m.FindUserById(It.Is<Guid>(id => id == userId)))
            .ReturnsAsync(validUser);

        mockHouseholdInvitesRepo.Setup(m => m.GetHouseholdInvites(It.Is<Guid>(id => id == userId)))
            .ReturnsAsync(new List<HouseholdInvite>()
            {
                new HouseholdInvite()
                {
                    Id = Guid.NewGuid(),
                    Household = new Household()
                }
            });

        mockAuthRepo.Setup(m => m.FindUserByEmail(It.Is<string>(email => email == "test@example.com")))
            .ReturnsAsync(validUser);
        
        mockAuthRepo.Setup(m => m.FindUserByEmail(It.Is<string>(email => email == "userAlreadyHousehold")))
            .ReturnsAsync(householdUser);

        mockHouseholdInvitesRepo.Setup(m =>
            m.InviteToHousehold(It.Is<HouseholdInvite>(m =>
                m.Household == It.IsAny<Household>() && m.InvitedUser == It.IsAny<User>())));

        mockHouseholdRepo.Setup(m => m.GetHouseholdById(It.Is<Guid>(x => x == householdId)))
            .ReturnsAsync(new Household());
        
        return (mockHouseholdInvitesRepo, mockAuthRepo, mockHouseholdRepo, userId, service, email, householdId);


    }

    [Test]
    public async Task AssertGetHouseholdInvitesSuccess()
    {
        var setup = GetSetup();
        
        var result = setup.service.GetHouseholdInvites(setup.userId);
        
        Assert.IsNotNull(result);
        Assert.Greater(result.Result.Count, 0);
    }

    [Test]
    public void AssertGetHouseholdInvitesFailUserNotFound()
    {
        var setup = GetSetup();

        Assert.ThrowsAsync<UserNotFoundException>(() => setup.service.GetHouseholdInvites(Guid.NewGuid()));
    }

    [Test]
    public void InviteToHouseholdSuccess()
    {
        var setup = GetSetup();
        
        Assert.DoesNotThrowAsync(() => setup.service.InviteToHousehold(setup.email, setup.householdId));
    }

    [Test]
    public void InviteToHouseholdFailUserNotFound()
    {
        var setup = GetSetup();

        Assert.ThrowsAsync<UserNotFoundException>(() =>
            setup.service.InviteToHousehold("InvalidEmail", setup.householdId));
    }

    [Test]
    public void InviteToHouseholdFailUserAlreadyHasHousehold()
    {
        var setup = GetSetup();

        Assert.ThrowsAsync<UserAlreadyHasHouseholdException>(() =>
            setup.service.InviteToHousehold("userAlreadyHousehold", setup.householdId));
    }

    [Test]
    public void InviteToHouseholdFailHouseholdNotFound()
    {
        var setup = GetSetup();

        Assert.ThrowsAsync<HouseholdNotFoundException>(() => setup.service.InviteToHousehold(setup.email, Guid.NewGuid()));
    }

}