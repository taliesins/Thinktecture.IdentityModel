using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Extensions;

namespace Thinktecture.IdentityModel.Hawk.Owin.Extensions
{
    public static class HawkAuthenticationExtension
    {
        public static IAppBuilder UseHawkAuthentication(this IAppBuilder app, HawkAuthenticationOptions options)
        {
            app.Use(typeof(HawkAuthenticationMiddleware), app, options);
            app.UseStageMarker(PipelineStage.Authenticate);
            return app;
        }
    }
}
