using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoederDAL.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly MssqlDbContext _context;

    public AuthRepository(MssqlDbContext context)
    {
        this._context = context;
    }

    public async Task<User> FindOrCreateUser(User user)
    {
        User? dbUser = await _context.Users.Include(u => u.Household)
            .FirstOrDefaultAsync(us =>  us.Email == user.Email);

        if (dbUser == null)
        {
            _context.Add(user);
            _context.SaveChanges();
            return user;
        }

        return dbUser;
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        return  await _context.Users
            .Include(u => u.Household)
            .FirstOrDefaultAsync(us => us.Email == email);
    }

    public async Task<User?> FindUserById(Guid userId)
    {
        return await _context.Users.FindAsync(userId);
    }
    

    public async Task<RefreshToken?> GetStoredRefreshToken(string refreshToken)
    {
        return await _context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == refreshToken);
    }

    public void StoreRefreshToken(RefreshToken refreshToken)
    {
        if (_context.RefreshTokens.Find(refreshToken.Token) == null)
        {
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();
        }
        else
        {
            throw new Exception("Refresh token already exists");
        }
        
    }
}