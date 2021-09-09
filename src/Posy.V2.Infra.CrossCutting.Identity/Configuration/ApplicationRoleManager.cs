using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Posy.V2.Infra.CrossCutting.Identity.Context;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using System;

namespace Posy.V2.Infra.CrossCutting.Identity.Configuration
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole, Guid>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, Guid> roleStore)
            :base(roleStore)
        {
            
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options,IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<ApplicationRole, Guid, ApplicationUserRole>(context.Get<ApplicationDbContext>()));
            //return new ApplicationRoleManager(new ApplicationRoleStore(context.Get<ApplicationDbContext>()));
        }
    }
}
