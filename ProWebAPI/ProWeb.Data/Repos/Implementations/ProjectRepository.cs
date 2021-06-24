using ProWeb.Data;
using ProWeb.Data.Base;
using ProWeb.Data.Repos.Interfaces;
using ProWeb.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ProWeb.Data.Repos.Implementations
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(MyDbContext context) : base(context)
        {
        }

        public MyDbContext Context { get { return (MyDbContext)context; } }

        public Project GetProjectByName(string projectName)
        {
            return Context.Projects.Where(x => x.Name == projectName).FirstOrDefault();
        }

        public List<Project> GetProjectsByAuthor(int authorId)
        {
            return Context.Projects.Where(x => x.IsActive == true).ToList();
        }
    }
}