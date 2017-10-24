using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jdean_budgeter.Startup))]
namespace jdean_budgeter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
