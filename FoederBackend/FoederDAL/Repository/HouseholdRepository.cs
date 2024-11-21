using FoederDomain.DomainModels;
using FoederDomain.Interfaces;

namespace FoederDAL.Repository;

public class HouseholdRepository : IHouseholdRepository
{
    private readonly MssqlDbContext _context;

    public HouseholdRepository(MssqlDbContext context)
    {
        this._context = context;
    }

    public async Task AddHousehold(Household household, User user)
    {
        _context.Households.Add(household);
        user.HouseholdId = household.Id;
        user.Household = household;
        _context.Users.Update(user);
        
        await _context.SaveChangesAsync();
    }
}