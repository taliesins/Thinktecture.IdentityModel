/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Tokens;
using System.IO;
using System.Xml;
using Thinktecture.IdentityModel.Http;

namespace Thinktecture.IdentityModel.Tokens.Http
{
    class HttpSaml2SecurityTokenHandler : Saml2SecurityTokenHandler
    {
        public HttpSaml2SecurityTokenHandler()
            : base()
        { }

        public HttpSaml2SecurityTokenHandler(SamlSecurityTokenRequirement requirement)
            : base(requirement)
        { }

        public override SecurityToken ReadToken(string tokenString)
        {
            // unbase64 header if necessary
            if (HeaderEncoding.IsBase64Encoded(tokenString))
            {
                tokenString = HeaderEncoding.DecodeBase64(tokenString);
            }

            return ReadToken(new XmlTextReader(new StringReader(tokenString)));
        }
    }
}
