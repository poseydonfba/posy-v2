﻿using Microsoft.Owin.Security;

namespace Posy.V2.Infra.CrossCutting.Identity.Configuration
{
    public class FakeTicket : ISecureDataFormat<AuthenticationTicket>
    {
        public string Protect(AuthenticationTicket data)
        {
            throw new System.NotImplementedException();
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new System.NotImplementedException();
        }
    }
}
