﻿using System;

namespace Posy.V2.Domain.Audit
{
    public class UsuarioAudit : Auditable
    {
        public virtual int UsuarioId { get; set; }

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

        public virtual string CurrencySymbol { get; set; }

        public virtual string Language { get; set; }

        public virtual string LongDateFormat { get; set; }

        public virtual string ShortDateFormat { get; set; }
    }
}
