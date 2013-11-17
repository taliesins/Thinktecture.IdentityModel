/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Security.Claims;

namespace Thinktecture.IdentityModel.Owin
{
    public class ClaimsTransformationOptions
    {
        public ClaimsAuthenticationManager ClaimsAuthenticationManager { get; set; }
    }
}