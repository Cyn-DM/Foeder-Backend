using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Household?> GetHouseholdByUserId(Guid userId)
    {
        var household = await _context.Households
            .Include(h => h.Users)
            .FirstOrDefaultAsync(h => h.Users.Any(u => u.Id == userId));

        return household;
    }

    public async Task<Household?> GetHouseholdById(Guid householdId)
    {
        var household = await _context.Households.FindAsync(householdId);

        return household;
    }
}