using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoederDAL.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly DbContext _context;

    public AuthRepository(DbContext context)
    {
        this._context = context;
    }

    public User FindOrCreateUser(User user)
    {
        User? dbUser = _context.Find<User>(user.Email);

        if (dbUser == null)
        {
            _context.Add(user);
            _context.SaveChanges();
            return user;
        }

        return dbUser;
    }
}