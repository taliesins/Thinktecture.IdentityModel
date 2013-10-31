using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartup(typeof(TokenFormatTest.Startup))]

namespace TokenFormatTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseSaml11BearerAuthentication(
            //    audience:         new Uri("urn:testrp"),
            //    issuerThumbprint: "973E8A633185A3A3E88B00B415CF9CBA608BA5F8",
            //    issuerName:       "sts");

            app.UseSaml2BearerAuthentication(
                audience: new Uri("urn:testrp"),
                issuerThumbprint: "973E8A633185A3A3E88B00B415CF9CBA608BA5F8",
                issuerName: "sts");


            app.UseWebApi(WebApiConfig.Register());
        }
    }
}