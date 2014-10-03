using BaseEntity;
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
        void ApplyAuditChanges();
        IEnumerable<T> SqlCommand<T>(string cmd) where T : Entity;
        DbEntityEntry<T> Entry<T>(T entity) where T : Entity;
        IDbSet<T> Set<T>() where T : Entity;
        int SaveChanges();
    }
}
