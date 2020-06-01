using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cdmx.Scg.Sancionados.Web.Startup))]
namespace Cdmx.Scg.Sancionados.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
