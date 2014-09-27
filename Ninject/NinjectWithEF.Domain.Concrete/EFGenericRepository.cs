using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NinjectWithEF.Domain.Abstract;

namespace NinjectWithEF.Domain.Concrete
{
    /// <summary>
    ///  This class implements the Generic Repository Interface ( IGenericRepository.cs )
    ///  which just delegates all calls to the associated Entity Framework DbSet
    /// </summary>
    public class EFGenericRepository<T> : IGenericRepository<T> where T : class 
    {
        private DbContext DbContext { get; set; }

        private DbSet<T> DbSet { get; set; }

        public EFGenericRepository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            else
            {
                DbContext = dbContext;

                DbSet = DbContext.Set<T>();
            }
        }

        #region IGenericRepository<T> implementation
        
        public virtual IQueryable<T> AsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DbSet;
        }

        public virtual IEnumerable<T> GetAllWithNavigationalProperties(string includeNavigationalProperties = "")
        {
            // create IQueryable object for query
            IQueryable<T> query = DbSet;

            // include navigational properties in query
            foreach (var includeProperty in includeNavigationalProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }


        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        // For performing raw sql 
        public IEnumerable<T> GetDataWithRawSql(string query, params object[] parameters)
        {
            return DbSet.SqlQuery(query, parameters).ToList();
        }

        // The Get method uses lambda expressions to allow the calling code to specify a filter condition and 
        // a column to order the results by, and a string parameter lets the caller provide a comma-delimited list 
        // of navigation properties for eager loading:
        // The code Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy also means the caller will provide a lambda expression.
        // For example, if the repository is instantiated for the Student entity type, the code in the calling method might 
        // specify q => q.OrderBy(s => s.LastName) for the orderBy parameter.
        // When you call the Get method, you could do filtering and sorting on the IEnumerable collection returned 
        // by the method instead of providing parameters for these functions. But the sorting and filtering work would 
        // then be done in memory on the web server. By using these parameters, you ensure that the work is done by the 
        // database rather than the web server.
        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
                                           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                           int pageNum = 0,
                                           int pageSize = 0,
                                           string includeNavigationalProperties = "")
        {
            // create IQueryable object for query
            IQueryable<T> query = DbSet;

            // applies the filter expression if there is one to the query
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // include navigational properties in query
            foreach (var includeProperty in includeNavigationalProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // order the query by orderBy expression if there is one
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // skip the number of records if pageNum > 0
            if (pageNum > 0)
            {
                query = query.Skip(pageNum * pageSize);
            }

            // take only pageSize if pageSize > 0
            if (pageSize > 0)
            {
                return query.Take(pageSize).ToList();
            }
            else
            {
                return query.ToList();
            }

        }


        public virtual T GetById(object id)
        {
            return DbSet.Find(id);
        }



        public virtual T Single(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Single(predicate);
        }


        public virtual T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }


        public virtual T First(Expression<Func<T, bool>> predicate)
        {
            return DbSet.First(predicate);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }



        public virtual void Add(T newEntity)
        {
            DbSet.Add(newEntity);
        }

        
        /// <summary>
        /// first overloaded delete() takes id 
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(object id)
        {
            T entityToDelete = DbSet.Find(id);

            Delete(entityToDelete);
        }

        
        /// <summary>
        /// second overloaded delete() takes entity type 
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(T entityToDelete)
        {
            if (DbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }

            DbSet.Remove(entityToDelete);
        }


        /// <summary>
        /// Update ALL properties/columns in the given entity
        /// </summary>
        /// <param name="entityToUpdate"></param>
        //public virtual void Update(T entityToUpdate)
        //{
        //    DbSet.Attach(entityToUpdate);

        //    DbContext.Entry(entityToUpdate).State = EntityState.Modified;
        //}
        public virtual void Update(T entityToUpdate)
        {
            if (entityToUpdate == null)
            {
                throw new ArgumentException("Cannot update a null entity.");
            }

            var entry = DbContext.Entry<T>(entityToUpdate);

            // Retreive the Id through reflection
            var pkey = DbSet.Create().GetType().GetProperty("Id").GetValue(entityToUpdate);

            if (entry.State == EntityState.Detached)
            {
                var set = DbContext.Set<T>();
                T attachedEntity = set.Find(pkey);  // access the key
                if (attachedEntity != null)
                {
                    var attachedEntry = DbContext.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entityToUpdate);
                }
                else
                {
                    entry.State = EntityState.Modified; // attach the entity
                }
            }
        }



        /// <summary>
        /// Updates only the specificed properties/columns in string[] propertyNames for the given entity
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <param name="propertyNames"></param>
        public virtual void AttachAndUpdate(T entityToUpdate, string[] propertyNames)
        {
            DbSet.Attach(entityToUpdate);

            foreach (var propertyName in propertyNames)
            {
                DbContext.Entry(entityToUpdate).Property(propertyName).IsModified = true;
            }
        }

        #endregion
    }
}
