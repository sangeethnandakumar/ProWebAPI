using ProWeb.Commons;
using ProWeb.Data;
using ProWeb.Entities;
using ProWeb.Service.Interfaces;
using System;
using System.Linq;

namespace ProWeb.Service.Implementations
{
    public class UserService : IUserService
    {
        public Result<User> GetUserById(int id)
        {
            using (var unitOfWork = new UnitOfWork(new MyDbContext()))
            {
                var user = unitOfWork.Users.Find(x => x.Id == id).FirstOrDefault();
                if (user != null)
                {
                    return new Success<User>(user);
                }
                else
                {
                    return new Failure<User>($"Unable to find a user with id {id}");
                }
            }
        }

        public Result<User> GetUserByUsername(string username)
        {
            using (var unitOfWork = new UnitOfWork(new MyDbContext()))
            {
                var user = unitOfWork.Users.Find(x => x.Username == username).FirstOrDefault();
                if (user != null)
                {
                    return new Success<User>(user);
                }
                else
                {
                    return new Failure<User>($"Unable to find a user with username {username}");
                }
            }
        }
    }
}