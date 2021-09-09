using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class ApplicationUser : IdentityUser<Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public DateTime Dir { get; set; }
        public DateTime? Der { get; set; }

        // https://www.jerriepelser.com/blog/allowing-user-to-set-culture-settings-aspnet5-part2/
        public string Culture { get; set; }
        public string UICulture { get; set; }

        public string CurrencySymbol { get; set; }
        [Required]
        public string Language { get; set; }
        public string LongDateFormat { get; set; }
        public string ShortDateFormat { get; set; }

        public ApplicationUser()
        {
            Id = Guid.NewGuid();
            Dir = DateTime.UtcNow;
            Language = "pt-BR";
            UsuarioClientes = new Collection<UsuarioCliente>();
        }

        public virtual ICollection<UsuarioCliente> UsuarioClientes { get; set; }

        [NotMapped]
        public string CurrentClientId { get; set; }
        [NotMapped]
        public string Nome { get; set; } // POSEYDON: Para claims
        [NotMapped]
        public string Sobrenome { get; set; } // POSEYDON: Para claims

        // Esta configuração foi adicionada aqui para uso com webapi
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager, ClaimsIdentity ext = null)
        {
            // Observe que o authenticationType precisa ser o mesmo que foi definido em CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(CurrentClientId))
            {
                claims.Add(new Claim("AspNet.Identity.ClientId", CurrentClientId));
            }

            // Adicione novos Claims aqui //
            claims.Add(new Claim("posy:nome", Nome ?? "")); 
            claims.Add(new Claim("posy:sobrenome", Sobrenome ?? ""));

            // Culture Claims
            // https://www.jerriepelser.com/blog/allowing-user-to-set-culture-settings-aspnet5-part2/
            var userPreferences = new UserCulturePreferences
            {
                Language = Language,
                ShortDateFormat = ShortDateFormat,
                LongDateFormat = LongDateFormat,
                CurrencySymbol = CurrencySymbol
            };
            claims.Add(new Claim("posy:localizationapp:cultureprefs", JsonConvert.SerializeObject(userPreferences)));

            // Adicionando Claims externos capturados no login
            if (ext != null)
            {
                await SetExternalPropertiesAsync(userIdentity, ext);
            }

            // Gerenciamento de Claims para informaçoes do usuario
            //claims.Add(new Claim("AdmRoles", "True"));

            userIdentity.AddClaims(claims);

            return userIdentity;
        }

        private async Task SetExternalPropertiesAsync(ClaimsIdentity identity, ClaimsIdentity ext)
        {
            if (ext != null)
            {
                var ignoreClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims";
                // Adicionando Claims Externos no Identity
                foreach (var c in ext.Claims)
                {
                    if (!c.Type.StartsWith(ignoreClaim))
                        if (!identity.HasClaim(c.Type, c.Value))
                            identity.AddClaim(c);
                }
            }
        }


        // CONFIGURACAO PARA WEBFORMS
        public ClaimsIdentity GenerateUserIdentity(UserManager<ApplicationUser, Guid> manager, ClaimsIdentity ext = null)
        {
            // Observe que o authenticationType precisa ser o mesmo que foi definido em CookieAuthenticationOptions.AuthenticationType
            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);

            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(CurrentClientId))
            {
                claims.Add(new Claim("AspNet.Identity.ClientId", CurrentClientId));
            }

            //  Adicione novos Claims aqui //

            // Adicionando Claims externos capturados no login
            if (ext != null)
            {
                SetExternalProperties(userIdentity, ext);
            }

            // Gerenciamento de Claims para informaçoes do usuario
            //claims.Add(new Claim("AdmRoles", "True"));

            userIdentity.AddClaims(claims);

            return userIdentity;
        }

        // CONFIGURACAO PARA WEBFORMS
        private void SetExternalProperties(ClaimsIdentity identity, ClaimsIdentity ext)
        {
            if (ext != null)
            {
                var ignoreClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims";
                // Adicionando Claims Externos no Identity
                foreach (var c in ext.Claims)
                {
                    if (!c.Type.StartsWith(ignoreClaim))
                        if (!identity.HasClaim(c.Type, c.Value))
                            identity.AddClaim(c);
                }
            }
        }
    }
}