using SAB.Application.Publication;
using SAB.Base.Publication;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Publication.PublicationType
{
    [BasicAuthAttribute]
    public class PublicationTypeController : Controller
    {
        /***************************************************************************************/

        private readonly PublicationTypeApplication _publicationTypeApplication = 
            new PublicationTypeApplication(InstanceFactory.Instance.GetInstance<IPublicationTypeRepository>());

        /***************************************************************************************/

        public ActionResult Index()
        {
            return View();
        }

        /***************************************************************************************/

        public ActionResult Create()
        {
            return View("~/Views/Publication/PublicationType/PublicationTypeRegisterView.cshtml");
        }

        /***************************************************************************************/

        [HttpPost]
        public ActionResult Save(SAB.Domain.Publication.PublicationType publicationType)
        {
            if (ModelState.IsValid)
            {
                _publicationTypeApplication.Insert(publicationType);
                TempData["message"] = "Se ha registrado el nuevo Tipo de Publicación";
                return RedirectToAction("Search");
            }

            return View("~/Views/Publication/PublicationType/PublicationTypeRegisterView.cshtml", publicationType);

        }

        /***************************************************************************************/
        public ActionResult Search()
        {
            //IEnumerable<SAB.Domain.Publication.PublicationType> lista = _publicationTypeApplication.QueryAll();
            return View("~/Views/Publication/PublicationType/PublicationTypeSearchView.cshtml");
        }

        /***************************************************************************************/

        public ActionResult SearchResult()
        {
            string codigo, nombre;

            codigo = Convert.ToString(Request["codigo"]);
            nombre = Convert.ToString(Request["nombre"]);

            int id = codigo.Equals("") ? 0 : Convert.ToInt32(codigo);
            string name = nombre.Equals("") ? null : nombre;

            ViewData["codigo"] = codigo;
            ViewData["nombre"] = nombre;

            IEnumerable<SAB.Domain.Publication.PublicationType> lista = _publicationTypeApplication.Search(id, name);

            int pageIndex = Int32.Parse(Request["pageIndex"]);

            int _pageSize = 10;
            int _totalRecords = lista.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            lista = lista.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);

            ViewBag.pageIndex = pageIndex;

            return View("~/Views/Publication/PublicationType/PublicationTypeSearchResultView.cshtml", lista);
        }

        /***************************************************************************************/

        public ActionResult Detail(int id)
        {
            SAB.Domain.Publication.PublicationType pt = _publicationTypeApplication.QueryById(id);
            if (pt == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("Search");
            } 
            return View("~/Views/Publication/PublicationType/Detail.cshtml",pt);
        }

        /***************************************************************************************/

        public ActionResult Modify(int id)
        {
            SAB.Domain.Publication.PublicationType pt = _publicationTypeApplication.QueryById(id);
            if (pt == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("Search");
            } 
            return View("~/Views/Publication/PublicationType/Modify.cshtml",pt);
        }

        /***************************************************************************************/

        [HttpPost]
        public ActionResult Update(SAB.Domain.Publication.PublicationType publicationType)
        {
            _publicationTypeApplication.Update(publicationType);
            TempData["message"] = "Se han guardado los cambios en el Tipo de Publicación " + publicationType.Id + " con éxito";
            return RedirectToAction("Search");
            
            //return View("~/Views/Publication/PublicationType/PublicationTypeModifyView.cshtml", publicationType);
        }

        /***************************************************************************************/

        public ActionResult Delete()
        {
            int pt_id = Convert.ToInt32(Request["element_id"]);
            SAB.Domain.Publication.PublicationType pt = new Domain.Publication.PublicationType();
            pt.Id = pt_id;
            _publicationTypeApplication.Delete(pt);

            TempData["message"] = "Se ha eliminado el tipo de publicación " + pt_id + " con éxito";

            return RedirectToAction("Search");
        }

        /***************************************************************************************/
	}
}