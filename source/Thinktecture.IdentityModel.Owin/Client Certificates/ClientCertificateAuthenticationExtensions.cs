/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Selectors;
using Thinktecture.IdentityModel.Owin;

namespace Owin
{
    public static class ClientCertificateAuthenticationExtensions
    {
        public static IAppBuilder UseClientCertificateAuthentication(this IAppBuilder app, X509CertificateValidator validator = null)
        {
            var options = new ClientCertificateAuthenticationOptions
            {
                Validator = validator ?? X509CertificateValidator.ChainTrust
            };

            app.UseClientCertificateAuthentication(options);
            return app;
        }

        public static IAppBuilder UseClientCertificateAuthentication(this IAppBuilder app, ClientCertificateAuthenticationOptions options)
        {
            app.Use<ClientCertificateAuthenticationMiddleware>(options);
            return app;
        }
    }
}