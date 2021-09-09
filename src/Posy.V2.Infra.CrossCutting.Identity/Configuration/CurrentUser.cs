using Microsoft.AspNet.Identity;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using System;
using System.Security.Claims;
using System.Threading;

namespace Posy.V2.Infra.CrossCutting.Identity.Configuration
{
    public class CurrentUser : ICurrentUser
    {
        public GlobalUser GetCurrentUser()
        {
            var principal = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            return new GlobalUser {
                UsuarioId = GetCurrentUserId(),
                Nome = $"{principal.FindFirstValue("posy:nome")} {principal.FindFirstValue("posy:sobrenome")}"
            };
        }

        public Guid GetCurrentUserId()
        {
            var principal = Thread.CurrentPrincipal.Identity.GetUserId();
            return Guid.Parse(principal); // Guid.Parse(HttpContext.Current.User.Identity.GetUserId())
        }
    }
}
