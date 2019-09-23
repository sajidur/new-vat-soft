using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Owin;

[assembly: OwinStartupAttribute(typeof(RiceMill_MVC.Startup))]
namespace RiceMill_MVC
{
    public partial class Startup
    {
        public void Configuration(AppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
