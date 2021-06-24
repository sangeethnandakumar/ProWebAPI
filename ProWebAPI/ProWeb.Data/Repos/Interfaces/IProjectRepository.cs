using ProWeb.Data;
using ProWeb.Data.Base;
using ProWeb.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ProWeb.Data.Repos.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        public Project GetProjectByName(string projectName);

        public List<Project> GetProjectsByAuthor(int authorId);
    }
}