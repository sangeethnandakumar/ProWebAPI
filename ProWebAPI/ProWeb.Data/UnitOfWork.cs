using ProWeb.Data.Repos;
using ProWeb.Data.Repos.Implementations;
using ProWeb.Data.Repos.Interfaces;

namespace ProWeb.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext context;
        public IUserRepository Users { get; private set; }
        public IProjectRepository Projects { get; private set; }

        public UnitOfWork(MyDbContext context)
        {
            this.context = context;
            Users = new UserRepository(context);
            Projects = new ProjectRepository(context);
        }

        public int Complete()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}