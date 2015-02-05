using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.User.LibraryUser
{
    public class LibraryUserController : Controller
    {
        //
        // GET: /LibraryUser/
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult Create()
        {
            return View("~/Views/User/LibraryUser/Create.cshtml");
        }

        public ActionResult Search()
        {
            return View("~/Views/User/LibraryUser/Search.cshtml");
        }

        public ActionResult SearchResult()
        {
            return View("~/Views/User/LibraryUser/SearchResult.cshtml");
        }
        public ActionResult Detail()
        {
            return View("~/Views/User/LibraryUser/Detail.cshtml");
        }

        public ActionResult ProfileL()
        {
            return View("~/Views/User/LibraryUser/Profile.cshtml");
        }

        public ActionResult RegistroS(int id)
        {

            TempData["message"] = "Se ha registrado exitosamente";

            return RedirectToAction("Index", "Home");
        }

	}
}