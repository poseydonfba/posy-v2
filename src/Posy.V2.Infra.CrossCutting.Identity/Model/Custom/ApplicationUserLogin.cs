using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid> { }
}
