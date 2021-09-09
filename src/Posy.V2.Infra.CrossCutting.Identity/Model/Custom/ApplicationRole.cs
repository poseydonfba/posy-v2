using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class ApplicationRole : IdentityRole<Guid, ApplicationUserRole>
    {
        public ApplicationRole() { }
        public ApplicationRole(string name) { Name = name; }
    }
}
