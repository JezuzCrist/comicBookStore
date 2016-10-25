using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ComicBookStore.Startup))]
namespace ComicBookStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
