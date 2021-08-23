using ProWeb.Data.Repos;
using ProWeb.Data.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProWeb.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IGoalRepository Goals { get; }

        int Complete();
    }
}