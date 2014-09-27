using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using NinjectWithEF.Domain.Abstract;

namespace NinjectWithEF.WebUI.Filters
{
    public class LogFilter : IActionFilter
    {
        // You can only use [Inject] on a property injection if you have registered the dependency/interface in the RegisterServices()
        [Inject]
        public ISiteRepository _repository { get; set; }

        // Since LogSeverityLevel is not an interface, you can not use [Inject] as a property injection
        protected readonly LogSeverityLevel Level;

        public LogFilter(LogSeverityLevel level )
        {
            this.Level = level;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // get the result of the action which is a viewresult type
            ViewResult result = filterContext.Result as ViewResult;

            // check that its not null
            if (result != null)
            {
                result.ViewBag.PostsFromFilter = _repository.AllPosts();

                // Add the log level value which is coming from LogAttribute 
                result.ViewBag.LogLevel = Level;
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}