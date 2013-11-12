using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Owin
{
    public class ClientCertificateAuthenticationHandler : AuthenticationHandler<ClientCertificateAuthenticationOptions>
    {
        protected override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            var cert = Context.Get<X509Certificate2>("ssl.ClientCertificate");

            if (cert == null)
            {
                return Task.FromResult<AuthenticationTicket>(null);
            }

            try
            {
                Options.Validator.Validate(cert);
            }
            catch
            {
                return Task.FromResult<AuthenticationTicket>(null);
            }

            var claims = GetClaimsFromCertificate(cert, cert.Issuer, Options.CreateExtendedClaimSet);

            var identity = new ClaimsIdentity(Options.AuthenticationType);
            identity.AddClaims(claims);

            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            return Task.FromResult<AuthenticationTicket>(ticket);
        }

        public virtual IEnumerable<Claim> GetClaimsFromCertificate(X509Certificate2 certificate, string issuer, bool includeAllClaims)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim("issuer", issuer));

            var thumbprint = certificate.Thumbprint;
            claims.Add(new Claim(ClaimTypes.Thumbprint, thumbprint, ClaimValueTypes.Base64Binary, issuer));

            string name = certificate.SubjectName.Name;
            if (!string.IsNullOrEmpty(name))
            {
                claims.Add(new Claim(ClaimTypes.X500DistinguishedName, name, ClaimValueTypes.String, issuer));
            }

            if (includeAllClaims)
            {
                name = certificate.SerialNumber;
                if (!string.IsNullOrEmpty(name))
                {
                    claims.Add(new Claim(ClaimTypes.SerialNumber, name, ClaimValueTypes.String, issuer));
                }

                name = certificate.GetNameInfo(X509NameType.DnsName, false);
                if (!string.IsNullOrEmpty(name))
                {
                    claims.Add(new Claim(ClaimTypes.Dns, name, ClaimValueTypes.String, issuer));
                }

                name = certificate.GetNameInfo(X509NameType.SimpleName, false);
                if (!string.IsNullOrEmpty(name))
                {
                    claims.Add(new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, issuer));
                }

                name = certificate.GetNameInfo(X509NameType.EmailName, false);
                if (!string.IsNullOrEmpty(name))
                {
                    claims.Add(new Claim(ClaimTypes.Email, name, ClaimValueTypes.String, issuer));
                }

                name = certificate.GetNameInfo(X509NameType.UpnName, false);
                if (!string.IsNullOrEmpty(name))
                {
                    claims.Add(new Claim(ClaimTypes.Upn, name, ClaimValueTypes.String, issuer));
                }

                name = certificate.GetNameInfo(X509NameType.UrlName, false);
                if (!string.IsNullOrEmpty(name))
                {
                    claims.Add(new Claim(ClaimTypes.Uri, name, ClaimValueTypes.String, issuer));
                }

                RSA key = certificate.PublicKey.Key as RSA;
                if (key != null)
                {
                    claims.Add(new Claim(ClaimTypes.Rsa, key.ToXmlString(false), ClaimValueTypes.RsaKeyValue, issuer));
                }

                DSA dsa = certificate.PublicKey.Key as DSA;
                if (dsa != null)
                {
                    claims.Add(new Claim(ClaimTypes.Dsa, dsa.ToXmlString(false), ClaimValueTypes.DsaKeyValue, issuer));
                }

                var expiration = certificate.GetExpirationDateString();
                if (!string.IsNullOrEmpty(expiration))
                {
                    claims.Add(new Claim(ClaimTypes.Expiration, expiration, ClaimValueTypes.DateTime, issuer));
                }
            }

            return claims;
        }
    }
}