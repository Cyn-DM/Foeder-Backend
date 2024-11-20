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
    public async Task AddHousehold(Household household)
    {
        _context.Households.Add(household);
        
        await _context.SaveChangesAsync();
    }
}