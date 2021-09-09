using System;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        //public string UserPhoto { get; set; }

        public DateTime Dir { get; set; }
    }
}
