using ProWeb.Data;
using ProWeb.Data.Base;
using ProWeb.Data.Repos.Interfaces;
using ProWeb.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ProWeb.Data.Repos.Implementations
{
    public class GoalRepository : Repository<Goal>, IGoalRepository
    {
        public GoalRepository(MyDbContext context) : base(context)
        {
        }

        public MyDbContext Context { get { return (MyDbContext)context; } }

    }
}