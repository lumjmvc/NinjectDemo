using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NinjectWithEF.WebUI.Startup))]
namespace NinjectWithEF.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
