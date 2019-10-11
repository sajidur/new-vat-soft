using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Owin;

[assembly: OwinStartupAttribute(typeof(REX_MVC.Startup))]
namespace REX_MVC
{
    public partial class Startup
    {
        public void Configuration(AppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
