using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManageRoles.Startup))]
namespace ManageRoles
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
