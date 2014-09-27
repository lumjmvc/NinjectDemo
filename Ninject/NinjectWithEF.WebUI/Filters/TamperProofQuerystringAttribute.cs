using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NinjectWithEF.WebUI.Common.Helpers;

namespace NinjectWithEF.WebUI.Filters
{
    public class TamperProofQuerystringAttribute : ActionFilterAttribute
    {
        public string IdName { get; set; }
        
        public string ExpiryName { get; set; }
        
        public int LinkExpiriesInSeconds { get; set; }

        public string ControllerName = "Home";

        public string ActionName = "Index";

        public TamperProofQuerystringAttribute(string idName, string expiryName, int linkExpiriesInSeconds =  20)
        {
            this.IdName = idName;

            this.ExpiryName = expiryName;

            this.LinkExpiriesInSeconds = linkExpiriesInSeconds;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string id;

            // either read the id value from the RouteData if RouteData.Values[Id] is NOT NULL 
            // otherwise, read the id from the HttpContext.Request.QueryString
            // the id can be either in the form 
            // id in routedata   - Home/Article/7?h=E9-D2-F1-19-ED-1A-D4-C7-D6-02-A7-AC-06-30-78-E1-D2-F9-48-66
            // id in querystring - Home/Article/?id=7&h=E9-D2-F1-19-ED-1A-D4-C7-D6-02-A7-AC-06-30-78-E1-D2-F9-48-66
            if (filterContext.RequestContext.RouteData.Values[IdName] != null)
            {
                id = filterContext.RequestContext.RouteData.Values[IdName].ToString();     
            }
            else
            {
                id = filterContext.HttpContext.Request.QueryString[IdName];
            }

            string expiry = filterContext.HttpContext.Request.QueryString[ExpiryName].ToString();

            DateTime submittedExpiry = Convert.ToDateTime(expiry, new CultureInfo("fr-FR", true));

            TimeSpan ts = DateTime.Now - submittedExpiry;

            int differenceInSeconds = ts.Seconds;

            string submittedHash = filterContext.HttpContext.Request.QueryString["h"];

            if (String.IsNullOrEmpty(submittedHash))
            {
                filterContext.Controller.TempData.Add("Message", "Querystring hash missing!!");

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = ControllerName, action = ActionName, area = ""}));
                
            }
            else if (HashingHelper.ComputeHash(id + expiry) != submittedHash)
            {
                filterContext.Controller.ViewBag.Message = "Invalid querystring hash  !!";
            }
            else if (differenceInSeconds > LinkExpiriesInSeconds)
            {
                filterContext.Controller.ViewBag.Message = "Link has expiried";
            }
            else
            {
                filterContext.Controller.ViewBag.Message = "Valid querystring hash!!";
            }

            filterContext.Controller.ViewBag.Sec = differenceInSeconds;

            base.OnActionExecuting(filterContext);
        }
    }
}