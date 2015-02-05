using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Politicas.UserProfile
{
    public class UserProfileController : Controller
    {
        //
        // GET: /UserProfile/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateUserProfile()
        {
            return View("~/Views/Politicas/UserProfile/CreateUserProfile.cshtml");
        }

        public ActionResult SearchUserProfile()
        {
            return View("~/Views/Politicas/UserProfile/SearchUserProfile.cshtml");
        }

        public ActionResult SearchResultUserProfile()
        {
            return View("~/Views/Politicas/UserProfile/SearchResultUserProfile.cshtml");
        }
        public ActionResult DetailUserProfile()
        {
            return View("~/Views/Politicas/UserProfile/DetailUserProfile.cshtml");
        }
	}
}