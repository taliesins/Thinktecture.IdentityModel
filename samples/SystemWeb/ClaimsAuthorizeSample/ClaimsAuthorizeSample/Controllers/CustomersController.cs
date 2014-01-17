using System.Web.Http;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.WebApi;

namespace ClaimsAuthorizeSample.Controllers
{
    public class CustomersController : ApiController
    {
        [ResourceActionAuthorize("Read", "SomeData")]
        public string Get()
        {
            return "OK";
        }

        public string Get(int id)
        {
            var isAllowed = ClaimsAuthorization.CheckAccess("Get", "CustomerId", id.ToString());

            return "OK " + id.ToString();
        }
    }
}
