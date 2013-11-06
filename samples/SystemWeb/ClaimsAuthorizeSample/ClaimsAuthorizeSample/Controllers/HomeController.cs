using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.SystemWeb;

namespace ClaimsAuthorizeSample.Controllers
{
    public class HomeController : Controller
    {
        [ClaimsAuthorize("View", "Home")]
        public ActionResult Index()
        {
            return View();
        }

        [ClaimsAuthorize("View", "About")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [ClaimsAuthorize("View", "Contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}