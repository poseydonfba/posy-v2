using FluentNHibernate.Mapping;
using Microsoft.AspNet.Identity;
using System;

namespace Posy.V2.Infra.CrossCutting.Identity.NHibernate.Configuration
{
    public class User : IUser<int>
    {
        public virtual int Id { get; protected set; }
        public virtual DateTime Dir { get; set; }
        public virtual DateTime? Der { get; set; }
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual DateTime? LockoutEndDateUtc { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Culture { get; set; }
        public virtual string UICulture { get; set; }
        public virtual string CurrencySymbol { get; set; }
        public virtual string Language { get; set; }
        public virtual string LongDateFormat { get; set; }
        public virtual string ShortDateFormat { get; set; }
        public class Map : ClassMap<User>
        {
            public Map()
            {
                Table("Usuario");
                Id(x => x.Id).GeneratedBy.Identity();
                Map(x => x.UserName).Not.Nullable();
                Map(x => x.PasswordHash).Not.Nullable();

                Map(x => x.Dir);
                Map(x => x.Der);
                Map(x => x.Email);
                Map(x => x.EmailConfirmed);
                Map(x => x.SecurityStamp);
                Map(x => x.PhoneNumber);
                Map(x => x.PhoneNumberConfirmed);
                Map(x => x.TwoFactorEnabled);
                Map(x => x.LockoutEndDateUtc);
                Map(x => x.LockoutEnabled);
                Map(x => x.AccessFailedCount);
                Map(x => x.Culture);
                Map(x => x.UICulture);
                Map(x => x.CurrencySymbol);
                Map(x => x.Language);
                Map(x => x.LongDateFormat);
                Map(x => x.ShortDateFormat);
            }
        }
    }
}
