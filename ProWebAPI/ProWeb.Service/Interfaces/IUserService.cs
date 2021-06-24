using ProWeb.Commons;
using ProWeb.Entities;

namespace ProWeb.Service.Interfaces
{
    public interface IUserService
    {
        public Result<User> GetUserByUsername(string username);

        public Result<User> GetUserById(int id);
    }
}