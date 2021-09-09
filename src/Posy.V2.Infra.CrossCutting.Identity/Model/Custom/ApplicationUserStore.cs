using Microsoft.AspNet.Identity.EntityFramework;
using Posy.V2.Infra.CrossCutting.Identity.Context;
using System;

namespace Posy.V2.Infra.CrossCutting.Identity.Model.Custom
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
