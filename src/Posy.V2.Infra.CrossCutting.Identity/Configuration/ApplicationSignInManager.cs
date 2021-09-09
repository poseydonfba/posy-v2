using System.Security.Claims;
using System.Threading.Tasks;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;

namespace Posy.V2.Infra.CrossCutting.Identity.Configuration
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, Guid>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        //public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        //{
        //    return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        //}

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        // https://stackoverflow.com/questions/25033418/asp-net-identity-2-1-alpha-signinmanager-getverifieduseridasync-invalid-cast
        // Invalid cast from 'System.String' to 'System.Guid'.
        public override Guid ConvertIdFromString(string id)
        {
            if (string.IsNullOrEmpty(id)) return Guid.Empty;

            return new Guid(id);
        }

        public override string ConvertIdToString(Guid id)
        {
            if (id.Equals(Guid.Empty)) return string.Empty;

            return id.ToString();
        }
    }
}
