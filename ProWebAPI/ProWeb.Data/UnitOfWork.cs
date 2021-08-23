using ProWeb.Data.Repos;
using ProWeb.Data.Repos.Implementations;
using ProWeb.Data.Repos.Interfaces;

namespace ProWeb.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext context;
        public IGoalRepository Goals { get; private set; }

        public UnitOfWork(MyDbContext context)
        {
            this.context = context;
            Goals = new GoalRepository(context);
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