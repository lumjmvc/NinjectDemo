using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using NinjectWithEF.Domain.Abstract;
using NinjectWithEF.Domain.Models;

namespace NinjectWithEF.WebUI.Pages
{
    // using WebViewPage<IEnumerable<Post>> will allow the action method to return a collection of Posts so you can reference 
    // them in your view
    public class ArticlesBasePage : WebViewPage<IEnumerable<Post>>
    {
        // [Inject] performs property injection so you can use the MessageService instance to access the 
        // implementations of IMessageService like 
        //             @MessageService.Message in the view page.
        //
        // The view page must inherit ArticlesBasePage using 
        //             @inherits NinjectWithEF.WebUI.Pages.ArticlesBasePage
        [Inject]
        public IMessageService MessageService { get; set; }

        public override void Execute()
        {
        }
    }
}