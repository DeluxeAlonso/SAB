using SAB.Application.User;
using SAB.Base.User;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Personal
{
    public class PersonalController : Controller
    {
        readonly private UserAccountApplication _userAccountApplication =
            new UserAccountApplication(InstanceFactory.Instance.GetInstance<IUserAccountRepository>());
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PersonalRegister()
        {

            return View("~/Views/Personal/PersonalRegisterView.cshtml");
        }

        public ActionResult PersonalSearch()
        {
            return View("~/Views/Personal/PersonalSearchView.cshtml");
        }


        public ActionResult ModifyPersonal()
        {
            return View("~/Views/Personal/PersonalModifyView.cshtml");
        }



        public ActionResult SearchPersonalResult()
        {
            return View("~/Views/Personal/PersonalSearchResultView.cshtml");
        }


        public ActionResult SearchPersonalResultDetail()
        {
            return View("~/Views/Personal/PersonalDetailView.cshtml");
        }


        [HttpPost]
        public ActionResult Save(SAB.Domain.User.UserAccount u)
        {
            TempData["message"] = "Se ha registrado un nuevo Empleado";
            return RedirectToAction("SearchPersonalResult");
        }

        [HttpPost]
        public ActionResult Update(int id)
        {
            TempData["message"] = "Se ha guardado los cambios del Empleado " + id + " con éxito";
            return RedirectToAction("SearchPersonalResult");
        }


        public ActionResult Delete(int id)
        {
            TempData["alert"] = "Se ha eliminado el Empleado " + id + " con éxito";
            return RedirectToAction("SearchPersonalResult");
        }


    }
}