using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TMWebRole.Startup))]
namespace TMWebRole
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
