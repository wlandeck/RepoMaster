using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkRepository.Interfaces
{
    public interface IDataContext : IDisposable, IObjectContextAdapter
    {
        IEnumerable<T> SqlCommand<T>(string cmd) where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
    }
}
