using Microsoft.EntityFrameworkCore;
using ProWeb.Data;
using ProWeb.Data.Base;
using ProWeb.Data.Repos.Interfaces;
using ProWeb.Entities;
using System.Linq;

namespace ProWeb.Data.Repos.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MyDbContext context) : base(context)
        {
        }

        public MyDbContext Context { get { return (MyDbContext)context; } }

        public User GetUserByUsername(string username)
        {
            return Context.Users.Where(x => x.Username == username).FirstOrDefault();
        }
    }
}