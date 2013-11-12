using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Owin
{
    public class BasicAuthenticationOptions : AuthenticationOptions
    {
        public BasicAuthenticationMiddleware.CredentialValidationFunction CredentialValidationFunction { get; private set; }
        public string Realm { get; private set; }

        public BasicAuthenticationOptions(string realm, BasicAuthenticationMiddleware.CredentialValidationFunction validationFunction)
            : base("Basic")
        {
            Realm = realm;
            CredentialValidationFunction = validationFunction;
        }
    }
}