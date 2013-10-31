using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenFormatTest.Security
{
    public class TestProvider : OAuthBearerAuthenticationProvider
    {
        public override System.Threading.Tasks.Task RequestToken(OAuthRequestTokenContext context)
        {
            return base.RequestToken(context);
        }
    }
}