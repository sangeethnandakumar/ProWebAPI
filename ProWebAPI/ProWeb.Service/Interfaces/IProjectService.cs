using ExpressGlobalExceptionHandler;
using ProWeb.Entities;
using System.Collections.Generic;

namespace ProWeb.Service.Interfaces
{
    public interface IProjectService
    {
        public IEnumerable<Project> GetMyProjects();
    }
}