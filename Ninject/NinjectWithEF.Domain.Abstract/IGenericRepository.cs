using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NinjectWithEF.Domain.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> AsQueryable();

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAllWithNavigationalProperties(string includeNavigationalProperties = "");

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        // For performing raw sql 
        IEnumerable<T> GetDataWithRawSql(string query, params object[] parameters);

        // When you call the Get method, you could do filtering and sorting on the IEnumerable collection returned 
        // by the method instead of providing parameters for these functions.
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
                           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                           int pageNum = 0,
                           int pageSize = 0,
                           string includeNavigationalProperties = "");

        T GetById(object id);

        T Single(Expression<Func<T, bool>> predicate);

        T SingleOrDefault(Expression<Func<T, bool>> predicate);

        T First(Expression<Func<T, bool>> predicate);

        T FirstOrDefault(Expression<Func<T, bool>> predicate);


        void Add(T newEntity);

        // first overloaded delete() takes id 
        void Delete(object id);

        // second overloaded delete() takes entity type 
        void Delete(T entityToDelete);

        // Is called to update all properties/column in given entity
        void Update(T entityToUpdate);

        // Is called to update only specific properties/columns in given entity
        void AttachAndUpdate(T entity, string[] propertyNames);
    }
}
