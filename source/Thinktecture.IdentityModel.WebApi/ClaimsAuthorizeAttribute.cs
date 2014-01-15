using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Thinktecture.IdentityModel.WebApi
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        private string _action;
        private string[] _resources;

        public ClaimsAuthorizeAttribute()
        { }

        public ClaimsAuthorizeAttribute(string action, params string[] resources)
        {
            _action = action; 
            _resources = resources;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!string.IsNullOrWhiteSpace(_action))
            {
                var cp = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;
                if (cp == null) cp = new ClaimsPrincipal(actionContext.ControllerContext.RequestContext.Principal);
                
                return ClaimsAuthorization.CheckAccess(cp, _action, _resources);
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

            var cp = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;
            if (cp == null) cp = new ClaimsPrincipal(actionContext.ControllerContext.RequestContext.Principal);
            
            return ClaimsAuthorization.CheckAccess(
                cp, 
                action,
                resource);
        }
    }
}
