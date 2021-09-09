using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Posy.V2.Infra.CrossCutting.Identity.Configuration
{
    // Configuração do UserManager Customizado
    public class ApplicationUserManager : UserManager<ApplicationUser, Guid>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, Guid> store)
            : base(store)
        {
            //var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            //var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("YourAppName");
            //manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));

            // Configurando validator para nome de usuario
            UserValidator = new UserValidator<ApplicationUser, Guid>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Logica de validação e complexidade de senha
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            // Configuração de Lockout
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Registrando os providers para Two Factor.
            RegisterTwoFactorProvider("Código via SMS", new PhoneNumberTokenProvider<ApplicationUser, Guid>
            {
                MessageFormat = "Seu código de segurança é: {0}"
            });

            RegisterTwoFactorProvider("Código via E-mail", new EmailTokenProvider<ApplicationUser, Guid>
            {
                Subject = "Código de Segurança",
                BodyFormat = "Seu código de segurança é: {0}"
            });

            // Definindo a classe de serviço de e-mail
            EmailService = new EmailService();

            // Definindo a classe de serviço de SMS
            SmsService = new SmsService();

            var provider = new DpapiDataProtectionProvider("POSY");
            var dataProtector = provider.Create("ASP.NET Identity");

            UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, Guid>(dataProtector);

            //var dataProtectionProvider = options.DataProtectionProvider;

            //if (dataProtectionProvider != null)
            //{
            //    manager.UserTokenProvider =
            //        new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}
        }

        //public async Task<IdentityResult> SignInClientAsync(ApplicationUser user, string clientKey)
        public async Task<IdentityResult> SignInClientAsync(ApplicationUser user, string clientKey, OthersClaims othersClaims)
        {
            if (string.IsNullOrEmpty(clientKey))
            {
                throw new ArgumentNullException("clientKey");
            }

            var client = user.UsuarioClientes.SingleOrDefault(c => c.ClientKey == clientKey);
            if (client == null)
            {
                client = new UsuarioCliente() { ClientKey = clientKey };
                user.UsuarioClientes.Add(client);
            }

            var result = await UpdateAsync(user);
            user.CurrentClientId = client.UsuarioClienteId.ToString();

            user.Nome = othersClaims.Nome; // POSEYDON
            user.Sobrenome = othersClaims.Sobrenome; // POSEYDON
            user.Language = othersClaims.Language; // POSEYDON
            user.ShortDateFormat = othersClaims.ShortDateFormat; // POSEYDON
            user.LongDateFormat = othersClaims.LongDateFormat; // POSEYDON
            user.CurrencySymbol = othersClaims.CurrencySymbol; // POSEYDON

            return result;
        }

        // Metodo para login async que remove os dados Client conectado
        public async Task<IdentityResult> SignOutClientAsync(ApplicationUser user, string clientKey)
        {
            if (string.IsNullOrEmpty(clientKey))
            {
                throw new ArgumentNullException("clientKey");
            }

            var client = user.UsuarioClientes.SingleOrDefault(c => c.ClientKey == clientKey);
            if (client != null)
            {
                user.UsuarioClientes.Remove(client);
            }

            user.CurrentClientId = null;
            return await UpdateAsync(user);
        }




        // CONFIGURACAO PARA WEBFORMS
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, Microsoft.Owin.IOwinContext context)
        {
            #region UTILIZADO NA APLICACAO WEBFORMS

            var manager = new ApplicationUserManager(
                new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context.Get<Posy.V2.Infra.CrossCutting.Identity.Context.ApplicationDbContext>()));

            //var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("YourAppName");
            //manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));

            // Configurando validator para nome de usuario
            manager.UserValidator = new UserValidator<ApplicationUser, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Logica de validação e complexidade de senha
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            // Configuração de Lockout
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Registrando os providers para Two Factor.
            manager.RegisterTwoFactorProvider("Código via SMS", new PhoneNumberTokenProvider<ApplicationUser, Guid>
            {
                MessageFormat = "Seu código de segurança é: {0}"
            });

            manager.RegisterTwoFactorProvider("Código via E-mail", new EmailTokenProvider<ApplicationUser, Guid>
            {
                Subject = "Código de Segurança",
                BodyFormat = "Seu código de segurança é: {0}"
            });

            // Definindo a classe de serviço de e-mail
            manager.EmailService = new EmailService();

            // Definindo a classe de serviço de SMS
            manager.SmsService = new SmsService();

            var provider = new DpapiDataProtectionProvider("POSY");
            var dataProtector = provider.Create("ASP.NET Identity");

            manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, Guid>(dataProtector);

            //var dataProtectionProvider = options.DataProtectionProvider;

            //if (dataProtectionProvider != null)
            //{
            //    manager.UserTokenProvider =
            //        new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}

            return manager;

            #endregion
        }

        // CONFIGURACAO PARA WEBFORMS
        public IdentityResult SignInClient(ApplicationUser user, string clientKey)
        {
            if (string.IsNullOrEmpty(clientKey))
            {
                throw new ArgumentNullException("clientKey");
            }

            var client = user.UsuarioClientes.SingleOrDefault(c => c.ClientKey == clientKey);
            if (client == null)
            {
                client = new UsuarioCliente() { ClientKey = clientKey };
                user.UsuarioClientes.Add(client);
            }

            IdentityResult result = this.Update(user);

            user.CurrentClientId = client.UsuarioClienteId.ToString();
            return result;
        }

        // CONFIGURACAO PARA WEBFORMS
        public IdentityResult SignOutClient(ApplicationUser user, string clientKey)
        {
            if (string.IsNullOrEmpty(clientKey))
            {
                throw new ArgumentNullException("clientKey");
            }

            var client = user.UsuarioClientes.SingleOrDefault(c => c.ClientKey == clientKey);
            if (client != null)
            {
                user.UsuarioClientes.Remove(client);
            }

            user.CurrentClientId = null;
            return this.Update(user);
        }
    }
}