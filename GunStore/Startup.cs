using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GunStore.Startup))]
namespace GunStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
