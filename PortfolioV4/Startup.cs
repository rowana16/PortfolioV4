using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PortfolioV4.Startup))]
namespace PortfolioV4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
