using Microsoft.AspNet.Identity;
using System;

namespace Posy.V2.Infra.CrossCutting.Identity.NHibernate.Configuration
{
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store)
            : base(store)
        {
            UserValidator = new UserValidator<User, int>(this);
            PasswordValidator = new PasswordValidator();
        }
    }
}
