using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WordBank.Startup))]
namespace WordBank
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
