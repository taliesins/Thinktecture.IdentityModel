using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(KatanaAuthentication.Startup))]

namespace KatanaAuthentication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // require SSL and client certificates
            app.RequireSsl(requireClientCertificate: true);

            // basic authentication
            app.UseBasicAuthentication("katanademo", ValidateUser);
            
            // client certificates
            app.UseClientCertificateAuthentication(X509RevocationMode.NoCheck);

            // transform claims to application identity
            app.UseClaimsTransformation(TransformClaims);

            app.UseWebApi(WebApiConfig.Register());
        }

        private Task<ClaimsPrincipal> TransformClaims(ClaimsPrincipal incoming)
        {
            if (!incoming.Identity.IsAuthenticated)
            {
                return Task.FromResult<ClaimsPrincipal>(incoming);
            }

            // parse incoming claims - create new principal with app claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "foo"),
                new Claim(ClaimTypes.Role, "bar")
            };

            var nameId = incoming.FindFirst(ClaimTypes.NameIdentifier);
            if (nameId != null)
            {
                claims.Add(nameId);
            }

            var thumbprint = incoming.FindFirst(ClaimTypes.Thumbprint);
            if (thumbprint != null)
            {
                claims.Add(thumbprint);
            }

            var id = new ClaimsIdentity("Application");
            id.AddClaims(claims);

            return Task.FromResult<ClaimsPrincipal>(new ClaimsPrincipal(id));
        }

        private Task<IEnumerable<Claim>> ValidateUser(string id, string secret)
        {
            if (id == secret)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Role, "Foo")
                };

                return Task.FromResult<IEnumerable<Claim>>(claims);
            }

            return Task.FromResult<IEnumerable<Claim>>(null);
        }
    }
}