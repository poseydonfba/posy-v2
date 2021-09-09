using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using System;
using System.Security.Claims;

namespace Posy.V2.Infra.CrossCutting.Identity.Extensions
{
    // https://www.jerriepelser.com/blog/allowing-user-to-set-culture-settings-aspnet5-part2/
    public static class PrincipalExtensions
    {
        public static UserCulturePreferences GetCulture(this ClaimsIdentity principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var userPreferences = principal.FindFirstValue("posy:localizationapp:cultureprefs");

            if (userPreferences == null)
                return null;

            return JsonConvert.DeserializeObject<UserCulturePreferences>(userPreferences);
        }
    }
}
