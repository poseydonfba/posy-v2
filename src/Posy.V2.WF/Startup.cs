using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Posy.V2.WF.Startup))]
namespace Posy.V2.WF
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
