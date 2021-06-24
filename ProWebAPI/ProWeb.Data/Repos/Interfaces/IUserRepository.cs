using Microsoft.EntityFrameworkCore;
using ProWeb.Data;
using ProWeb.Data.Base;
using ProWeb.Entities;
using System.Linq;

namespace ProWeb.Data.Repos.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public User GetUserByUsername(string username);
    }
}