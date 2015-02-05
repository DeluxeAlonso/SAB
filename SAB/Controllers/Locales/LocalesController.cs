using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAB.Shared;
using SAB.Base.Library;
using SAB.Domain.Library;
using SAB.Application.Libray;
using System.Text;
using System.Text.RegularExpressions;

namespace SAB.Controllers.Locales
{
    public class LocalesController : Controller
    {
        readonly private LocalApplication _localApplication = new LocalApplication(InstanceFactory.Instance.GetInstance<ILocalRepository>());

        // GET: Locales
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LocalesRegister()
        {
            return View("~/Views/Locales/LocalesRegisterView.cshtml");
        }

  
        
        public ActionResult LocalesDetail(int id)
        {
            Local local = _localApplication.QueryById(id);
            if (local == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("LocalesSearch");
            }     
            return View("~/Views/Locales/LocalesDetailView.cshtml", local);
        }

        public ActionResult LocalesSearch()
        {
            IEnumerable<Local> resultado = _localApplication.QueryAll();
            return View("~/Views/Locales/LocalesListView.cshtml", resultado);
        }

        public ActionResult LocalesList(Local parametrosBusqueda)
        {
            if (parametrosBusqueda.Id == 0)
            {
                ViewData["id"] = null;
            }
            else ViewData["id"] = parametrosBusqueda.Id;
            ViewData["nombre"] = parametrosBusqueda.Name;
            ViewData["ciudad"] = parametrosBusqueda.City;
            ViewData["distrito"] = parametrosBusqueda.Distric;
            if (parametrosBusqueda.Name != null) parametrosBusqueda.Name = parametrosBusqueda.Name.Trim();
            if (parametrosBusqueda.City != null) parametrosBusqueda.City = parametrosBusqueda.City.Trim();
            if (parametrosBusqueda.Distric != null) parametrosBusqueda.Distric = parametrosBusqueda.Distric.Trim();
            IEnumerable<Local> resultado = _localApplication.Search(parametrosBusqueda);
            if (resultado == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("LocalesSearch");
            }
            return View("~/Views/Locales/LocalesListView.cshtml",resultado);
        }
        public ActionResult LocalesModify(int id)
        {
            Local local = _localApplication.QueryById(id);
            if (local == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("LocalesSearch");
            }
          
            return View("~/Views/Locales/LocalesModifyView.cshtml",local);
        }

        [HttpPost]
        public ActionResult Save(Local local)
        {
            if (ModelState.IsValid)
            {
                _localApplication.Insert(local);
                TempData["message"] = "Se ha registrado una nueva biblioteca.";
                return RedirectToAction("LocalesSearch");
            }
            return View("~/Views/Locales/LocalesRegisterView.cshtml",local);
        }


        [HttpPost]
        public ActionResult Update(Local local)
        {
            if (ModelState.IsValid)
            {
                _localApplication.Update(local);
                TempData["message"] = "Se han guardado los cambios en la biblioteca " + local.Id + " con éxito.";
                return RedirectToAction("LocalesSearch");
            }
            return View("~/Views/Locales/LocalesModifyView.cshtml", local);
        }


        public ActionResult Delete()
        {
            int local_id = Convert.ToInt32(Request["element_id"]);
            try
            {
                _localApplication.Delete(local_id);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                int numeroError = e.Number;
                if (numeroError == 547) // Error porque otros registros dependen de éste.
                {
                    TempData["alert"] = "No se pudo eliminar la biblioteca. Otros registros dependen de ésta.";
                }
                else TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("LocalesSearch");
            }
            TempData["message"] = "Se ha eliminado la biblioteca " + local_id + " con éxito.";
            return RedirectToAction("LocalesSearch");
        }
    }
}