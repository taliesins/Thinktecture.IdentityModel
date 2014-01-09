/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see LICENSE
 */

using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Thinktecture.IdentityModel.WebApi
{
    public class ResourceActionAuthorizeAttribute : AuthorizeAttribute
    {
        private string _action;
        private string[] _resources;

        public ResourceActionAuthorizeAttribute()
        { }

        public ResourceActionAuthorizeAttribute(string action, params string[] resources)
        {
            _action = action; 
            _resources = resources;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!string.IsNullOrWhiteSpace(_action))
            {
                var principal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;
                return ClaimsAuthorization.CheckAccess(_action, _resources);
            }
            else
            {
                return CheckAccess(actionContext);
            }
        }

        protected virtual bool CheckAccess(HttpActionContext actionContext)
        {
            var action = actionContext.ActionDescriptor.ActionName;
            var resource = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var principal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;

            return ClaimsAuthorization.CheckAccess(
                principal,
                action,
                resource);
        }
    }
}
