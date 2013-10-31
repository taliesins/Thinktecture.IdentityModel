using System.Web.Http;

namespace TokenFormatTest.Controllers
{
    [Authorize]
    public class IdentityController : ApiController
    {
        public string Get()
        {
            return User.Identity.Name;
        }
    }
}