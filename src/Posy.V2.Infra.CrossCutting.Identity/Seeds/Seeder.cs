using Microsoft.AspNet.Identity.EntityFramework;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using Posy.V2.Infra.CrossCutting.Identity.Context;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Posy.V2.V2.Infra.CrossCutting.Identity.Seeds
{
    public class Seeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(
                new UserStore<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context));

            //var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole, Guid, ApplicationUserRole>(context));

            //if (!roleManager.Roles.Any())
            //{
            //    await roleManager.CreateAsync(new ApplicationRole { Id = Guid.NewGuid(), Name = "Administrador" /*ApplicationRole.AdminRoleName*/ });
            //    await roleManager.CreateAsync(new ApplicationRole { Id = Guid.NewGuid(), Name = "Colaborador" /*ApplicationRole.AffiliateRoleName*/ });
            //}

            if (!userManager.Users.Any(u => u.UserName == "admin"))
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0123456789",
                    PhoneNumberConfirmed = true
                };

                await userManager.CreateAsync(user, "123456");
                //await userManager.AddToRoleAsync(user.Id, "Administrador" /*ApplicationRole.AdminRoleName*/);
            }
        }
    }
}
