using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmbeddedAuthorizationServer.Provider
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // accept any client
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // validate user credentials
            if (context.UserName != context.Password)
            {
                context.Rejected();
                return;
            }

            // create identity
            var id = new ClaimsIdentity("Embedded");
            id.AddClaim(new Claim("sub", context.UserName));

            context.Validated(id);
        }
    }
}