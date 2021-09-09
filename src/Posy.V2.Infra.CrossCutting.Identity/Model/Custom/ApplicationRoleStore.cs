using Microsoft.AspNet.Identity.EntityFramework;
using Posy.V2.Infra.CrossCutting.Identity.Context;
using System;

namespace Posy.V2.Infra.CrossCutting.Identity.Model.Custom
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole, Guid, ApplicationUserRole>
    {
        public ApplicationRoleStore(ApplicationDbContext context) 
            : base(context) 
        {
        }
    }
}
