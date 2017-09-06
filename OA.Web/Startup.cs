using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OA.Web.Startup))]
namespace OA.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
