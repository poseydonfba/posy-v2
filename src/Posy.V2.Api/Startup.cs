using Microsoft.Owin;
using Owin;
using Posy.V2.Api;

[assembly: OwinStartup(typeof(Startup))]
namespace Posy.V2.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();

            // VER
            // http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/

        }
    }
}