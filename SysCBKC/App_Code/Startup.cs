using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SysCBKC.Startup))]
namespace SysCBKC
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
