using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AToko.Startup))]
namespace AToko
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
