using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NinjectWithEF.Domain.Abstract;

namespace NinjectWithEF.WebUI.Filters
{
    // To use/apply this filter, you use Ninject to register this filter for the action method or methods 
    // in a controller or globally.
    // you don't have to use this filter by adding [Products] to the action or controller as Ninject has registered that for you.
    public class ProductsFilter : IActionFilter
    {
        private readonly ISiteRepository _repository;

        public ProductsFilter(ISiteRepository repository)
        {
            _repository = repository;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // get the result of the action which is a viewresult type
            ViewResult result = filterContext.Result as ViewResult;
            
            // check that its not null
            if (result != null)
            {
                // Add the posts collection to the viewbag.posts
                result.ViewBag.Posts = _repository.AllPosts();
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}