using ExpressGlobalExceptionHandler;
using ProWeb.Data;
using ProWeb.Entities;
using ProWeb.Service.Interfaces;
using System.Collections.Generic;

namespace ProWeb.Service.Implementations
{
    public class ProjectService : IProjectService
    {
        public IEnumerable<Project> GetMyProjects()
        {
            using (var unitOfWork = new UnitOfWork(new MyDbContext()))
            {
                var user = unitOfWork.Projects.GetAll();
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}