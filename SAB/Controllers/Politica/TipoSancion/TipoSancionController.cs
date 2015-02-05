using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Politicas.TipoSancion
{
    public class TipoSancionController : Controller
    {
        //
        // GET: /TipoSancion/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TipoSancionDriver()
        {
            return View("~/Views/Politica/TipoSancion/TipoSancionView.cshtml");
        }

        public ActionResult Create()
        {
            return View("~/Views/Politica/TipoSancion/Create.cshtml");
        }

        public ActionResult Search()
        {
            return View("~/Views/Politica/TipoSancion/SearchResult.cshtml");
        }

        public ActionResult Detail()
        {
            return View("~/Views/Politica/TipoSancion/Detail.cshtml");
        }

        public ActionResult Modify()
        {
            
            return View("~/Views/Politica/TipoSancion/Modify.cshtml");
        }
        
        [HttpPost]
        public ActionResult Save()
        {
            TempData["message"] = "Se ha registrado el nuevo Tipo de Sanción";
            return RedirectToAction("Search");
        }


        [HttpPost]
        public ActionResult Update( int id)
        {
            TempData["message"] = "Se ha guardado los cambio en el Tipo de Sanción " + id + " con éxito";
            return RedirectToAction("Search");
        }

        
        public ActionResult Delete(int id)
        {
            TempData["alert"] = "Se ha eliminado el Tipo de sanción " + id + " con éxito";
            return RedirectToAction("Search");
        }

        
    }
}