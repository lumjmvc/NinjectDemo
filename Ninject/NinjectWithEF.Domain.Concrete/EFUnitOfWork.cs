using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjectWithEF.Domain.Abstract;
using NinjectWithEF.Domain.Models;

namespace NinjectWithEF.Domain.Concrete
{
    // This class  implements the Unit Of Work Interface ( IUnitOfWork.cs )
    // The unit of work class serves one purpose and that is to creating a single database context class shared by multiple repositories
    // That way, when a unit of work is complete you can call the SaveChanges method on that instance of the context and be 
    // assured that all related changes will be coordinated. All that the class needs is a Save method and a property for each repository. 
    // Each repository property returns a repository instance that has been instantiated using the same database context instance as 
    // the other repository instances.
    public class EFUnitOfWork : IUnitOfWork
    {
        private EFDbContext dbContext;


        // NOTE:: 
        // If you have a class that implements any of these repositories below, then you should reference that class here AND NOT use the
        // the IGenericRepository<entity> below for that entity
        // e.g If a class PostRepository implements EFGenericRepository<Post> like
        //      public class PostRepository : EFGenericRepository<Post> 
        // Then you add the following below
        //      private PostRepository _postRepository;
        // and remove 
        //      private IGenericRepository<Post> _postRepository;
        // 
        private IGenericRepository<Post> _postRepository;

        public EFUnitOfWork()
        {
            this.dbContext = new EFDbContext();
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        // Each repository property checks whether the repository already exists. 
        // If not, it instantiates the repository, passing in the context instance. 
        // As a result, all repositories share the same context instance.
        // The get accessor for postRepository 
        public IGenericRepository<Post> PostRepository
        {
            get
            {
                if (this._postRepository == null)
                {
                    this._postRepository = new EFGenericRepository<Post>(dbContext);
                }

                return _postRepository;
            }
        }

        // Like any class that instantiates a database context in a class variable, 
        // the UnitOfWork class implements IDisposable and disposes the context.
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
