using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjectWithEF.Domain.Abstract;
using NinjectWithEF.Domain.Models;

namespace NinjectWithEF.Domain.Concrete
{
    public class SiteRepository : ISiteRepository
    {
        // IUnitOfWork instance IS required for all db actions.
        private readonly IUnitOfWork _uow;

        // create a uow instance that will be used in all methods 
        public SiteRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }


        #region implementations of ISiteRepository
        public int TotalPosts()
        {
            return _uow.PostRepository.GetAll().Count();
        }

        public IEnumerable<Models.Post> AllPosts()
        {
            return _uow.PostRepository.GetAll().ToList();
        }

        #endregion


        public void AddPost(Post post)
        {
            _uow.PostRepository.Add(post);

            _uow.Commit();
        }
    }
}
