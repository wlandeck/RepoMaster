using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IAuditDateInfo
    {
        DateTime CreatedOn { get; set; }
        DateTime ModifiedOn { get; set; } 
    }
}
