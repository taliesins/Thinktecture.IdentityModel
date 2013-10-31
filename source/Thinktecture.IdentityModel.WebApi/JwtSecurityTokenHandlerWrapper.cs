/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see LICENSE
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace Thinktecture.IdentityModel.Tokens
{
    class IdentityModelJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        TokenValidationParameters validationParams;

        public IdentityModelJwtSecurityTokenHandler(TokenValidationParameters validationParams, Dictionary<string, string> inboundClaimTypeMap = null)
        {
            this.validationParams = validationParams;

            if (inboundClaimTypeMap != null)
            {
                InboundClaimTypeMap = inboundClaimTypeMap;
            }
        }

        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            var jwt = token as JwtSecurityToken;
            var list = new List<ClaimsIdentity>(this.ValidateToken(jwt, validationParams).Identities);
            return list.AsReadOnly();
        }
    }
}
