using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjectWithEF.Domain.Models;

namespace NinjectWithEF.Domain.Abstract
{
    /// <summary>
    /// Create a Unit Of Work interface
    //  containing all the generic repositories for each entity being part of the unit of work, along with a single Commit() 
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        IGenericRepository<Post> PostRepository { get; }

    }
}
