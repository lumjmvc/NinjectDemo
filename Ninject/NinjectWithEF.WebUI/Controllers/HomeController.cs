using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NinjectWithEF.Domain.Abstract;
using NinjectWithEF.Domain.Concrete;
using NinjectWithEF.Domain.Models;
using NinjectWithEF.WebUI.Common.Helpers;
using NinjectWithEF.WebUI.Filters;

namespace NinjectWithEF.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISiteRepository _repository;

        public HomeController(ISiteRepository repository)
        {
            _repository = repository;
        }


        [Log(level:LogSeverityLevel.Warning)]
        public ActionResult Index()
        {
            var posts = _repository.AllPosts();

            if (!posts.Any())
            {
                ViewBag.Message = "No Posts Found";
            }

            return View(posts);
        }

        public ActionResult Articles()
        {
            var posts = _repository.AllPosts();

            if (!posts.Any())
            {
                ViewBag.Message = "No Posts Found";
            }

            return View(posts);
        }


        [TamperProofQuerystring(idName: "id",expiryName:"expiry")]
        public ActionResult Article(int id, string expiry)
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult CreatePost(Post post)
        {
            if (ModelState.IsValid)
            {
                   _repository.AddPost(post);

                return RedirectToAction("Index");
                
            }

            return View("CreatePost");
        }
    }
}