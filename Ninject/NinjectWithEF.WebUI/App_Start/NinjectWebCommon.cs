using System.Web.Mvc;
using Ninject.Web.Mvc.FilterBindingSyntax;
using NinjectWithEF.Domain.Abstract;
using NinjectWithEF.Domain.Concrete;
using NinjectWithEF.WebUI.Controllers;
using NinjectWithEF.WebUI.Filters;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWithEF.WebUI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWithEF.WebUI.App_Start.NinjectWebCommon), "Stop")]

namespace NinjectWithEF.WebUI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Extensions.Conventions;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // automatically bind interfaces and its implementations
            // this statement requires installing => Install-Package ninject.extensions.conventions
            // next, add the namespace  using Ninject.Extensions.Conventions;
            //kernel.Bind(x => x.FromThisAssembly()
            //                   .SelectAllClasses()
            //                   .BindAllInterfaces());

            AddBindings(kernel);

            AddFilterBindings(kernel);
        }

        private static void AddFilterBindings(IKernel kernel)
        {
            // For this filter binding, the custom ProductsFilter is applied to Index action method of Home controller only
            //kernel.BindFilter<ProductsFilter>(FilterScope.Action, 0)
            //    .When((controllerContext, actionDescriptor) =>
            //        actionDescriptor.ControllerDescriptor.ControllerType == typeof(HomeController) &&
            //        actionDescriptor.ActionName == "Index");

            // This filter is similar to the above but using a different code
            /*kernel.BindFilter<ProductsFilter>(FilterScope.Action, 0)
                    .When((controllerContext, actionDescriptor) =>
                        actionDescriptor.ControllerDescriptor.ControllerName == "Home" &&
                        actionDescriptor.ActionName == "Index");
             */

            // For this filter binding, the custom ProductsFilter is applied to ALL action methods of Home controller
            kernel.BindFilter<ProductsFilter>(FilterScope.Action, 0)
                  .WhenControllerType<HomeController>();

            // The filter binding specifies that whenever an action method that has a Log attribute applied to it with a 
            // value assigned for level parameter, then bind and apply the LogFilter to the action method and 
            // inject any dependencies that LogFilter requires.  
            kernel.BindFilter<LogFilter>(FilterScope.Action, 0)
                    .WhenActionMethodHas<LogAttribute>()
                    .WithConstructorArgumentFromActionAttribute<LogAttribute>("level", attr => attr.Level);
        }

        private static void AddBindings(IKernel kernel)
        {
            // create an instance of EfUnitOfWork class to service requests for the IUnitOfWork interface
            // when any controller/class creates an instance of IUnitOfWork interface
            // the use of InRequestScope for the EFUnitOfWork class which tells Ninject to create a single 
            // instance of this object per HTTP request. In other words, every time a user requests a page, 
            // no mater how many repository classes are involved, Ninject will create a single factory class and, 
            // therefore, a single DbContext instance. This also ensures that the object is destroyed once the request completes. 
            kernel.Bind<IUnitOfWork>().To<EFUnitOfWork>().InRequestScope();

            // register a service layer that uses IUnitOfWork interface 
            // that will be used in any controller
            kernel.Bind<ISiteRepository>().To<SiteRepository>();

            kernel.Bind<IMessageService>().To<MessageService>();

            // register the custom model binder with ninject
            //ModelBinders.Binders.Add(typeof(Post), new PostModelBinder(ninjectKernel));
        }
    }
}
