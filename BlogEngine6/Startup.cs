using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogEngine6.Startup))]
namespace BlogEngine6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
