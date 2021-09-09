using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json;
using Posy.V2.Infra.CrossCutting.Identity.Extensions;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Posy.V2.Infra.CrossCutting.Identity.Configuration
{
    // Validação do Secutiry Stamp para usuário conectado nos clients registrados.
    public static class ApplicationCookieIdentityValidator
    {
        private static async Task<bool> VerifySecurityStampAsync(ApplicationUserManager manager, ApplicationUser user, CookieValidateIdentityContext context)
        {
            var userId = Guid.Parse(context.Identity.GetUserId());
            string stamp = context.Identity.FindFirstValue("AspNet.Identity.SecurityStamp");
            return (stamp == await manager.GetSecurityStampAsync(userId));
        }

        private static Task<bool> VerifyClientIdAsync(ApplicationUserManager manager, ApplicationUser user, CookieValidateIdentityContext context)
        {
            string clientId = context.Identity.FindFirstValue("AspNet.Identity.ClientId");
            if (!string.IsNullOrEmpty(clientId) && user.UsuarioClientes.Any(c => c.UsuarioClienteId.ToString() == clientId))
            {
                user.CurrentClientId = clientId;

                user.Nome = context.Identity.FindFirstValue("posy:nome"); // POSEYDON
                user.Sobrenome = context.Identity.FindFirstValue("posy:sobrenome"); // POSEYDON

                var userPreferences = context.Identity.GetCulture(); // POSEYDON
                if (userPreferences != null)
                {
                    user.Language = userPreferences.Language; // POSEYDON
                    user.ShortDateFormat = userPreferences.ShortDateFormat; // POSEYDON
                    user.LongDateFormat = userPreferences.LongDateFormat; // POSEYDON
                    user.CurrencySymbol = userPreferences.CurrencySymbol; // POSEYDON
                }
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public static Func<CookieValidateIdentityContext, Task> OnValidateIdentity(TimeSpan validateInterval, Func<ApplicationUserManager, ApplicationUser, Task<ClaimsIdentity>> regenerateIdentity)
        {
            return async context =>
            {
                DateTimeOffset utcNow = context.Options.SystemClock.UtcNow;
                DateTimeOffset? issuedUtc = context.Properties.IssuedUtc;
                bool expired = false;
                if (issuedUtc.HasValue)
                {
                    TimeSpan t = utcNow.Subtract(issuedUtc.Value);
                    expired = (t > validateInterval);
                }
                if (expired)
                {
                    var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                    var userId = context.Identity.GetUserId();
                    if (userManager != null && !string.IsNullOrEmpty(userId))
                    {
                        var user = await userManager.FindByIdAsync(Guid.Parse(userId));
                        bool reject = true;
                        if (user != null
                            && await VerifySecurityStampAsync(userManager, user, context)
                            && await VerifyClientIdAsync(userManager, user, context))
                        {
                            reject = false;
                            if (regenerateIdentity != null)
                            {
                                ClaimsIdentity claimsIdentity = await regenerateIdentity(userManager, user);
                                if (claimsIdentity != null)
                                {
                                    context.OwinContext.Authentication.SignIn(new ClaimsIdentity[]
                                    {
                                        claimsIdentity
                                    });
                                }
                            }
                        }
                        if (reject)
                        {
                            context.RejectIdentity();
                            context.OwinContext.Authentication.SignOut(new string[]
                            {
                                context.Options.AuthenticationType
                            });
                        }
                    }
                }
            };
        }
    }
}
