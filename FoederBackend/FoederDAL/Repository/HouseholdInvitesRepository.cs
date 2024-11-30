using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoederDAL.Repository;

public class HouseholdInvitesRepository : IHouseholdInvitesRepository
{
    private readonly MssqlDbContext _context;

    public HouseholdInvitesRepository(MssqlDbContext context)
    {
        this._context = context;
    }

    public async Task<List<HouseholdInvite>> GetHouseholdInvites(Guid userId)
    {
        List<HouseholdInvite> foundInvites = await _context.HouseholdInvites
            .Where(x => x.InvitedUser.Id == userId)
            .ToListAsync();

        return foundInvites;
    }

    public async Task InviteToHousehold(HouseholdInvite householdInvite)
    {
        _context.HouseholdInvites.Add(householdInvite);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateHouseholdInvite(HouseholdInvite householdInvite)
    {
        if (!householdInvite.IsAccepted.HasValue)
        {
            throw new ArgumentNullException("Household invite has no acceptation value.");
        }
        if (householdInvite.IsAccepted.HasValue)
        {
            if (householdInvite.IsAccepted.Value)
            {
                var user = householdInvite.InvitedUser;
                user.Household = householdInvite.Household;
                _context.Users.Update(householdInvite.InvitedUser);
            }  
            _context.HouseholdInvites.Update(householdInvite);
        }
        await _context.SaveChangesAsync();
    }
}