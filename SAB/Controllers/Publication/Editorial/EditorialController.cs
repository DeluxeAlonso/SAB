using SAB.Application.Publication;
using SAB.Base.Publication;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Publication.Editorial
{
    [BasicAuthAttribute]
    public class EditorialController : Controller
    {
        /***************************************************************************************/

        private readonly EditorialApplication _editorialApplication =
            new EditorialApplication(InstanceFactory.Instance.GetInstance<IEditorialRepository>());

        /***************************************************************************************/

        public ActionResult Index()
        {
            return View();
        }

        /***************************************************************************************/

        public ActionResult EditorialRegister()
        {
            return View("~/Views/Publication/Editorial/EditorialRegisterView.cshtml");
        }

        /***************************************************************************************/

        [HttpPost]
        public ActionResult Save(SAB.Domain.Publication.Editorial editorial)
        {
            _editorialApplication.Insert(editorial);
            TempData["message"] = "Se ha registrado la nueva Editorial";
            return RedirectToAction("EditorialSearch");
        }

        /***************************************************************************************/

        public ActionResult EditorialSearch()
        {
            ViewData["hoy"] = DateTime.Now.ToShortDateString();

            IEnumerable<SAB.Domain.Publication.Editorial> editorialList = _editorialApplication.QueryAll();

            return View("~/Views/Publication/Editorial/EditorialSearchView.cshtml");
        }

        /***************************************************************************************/

        public ActionResult EditorialSearchResult()
        {
            string codigo, nombre;

            codigo = Convert.ToString(Request["codigo"]);
            nombre = Convert.ToString(Request["nombre"]);

            int id = codigo.Equals("") ? 0 : Convert.ToInt32(codigo);
            string name = nombre.Equals("") ? null : nombre;

            DateTime desde, hasta;

            try
            {
                desde = Convert.ToDateTime(Request["desde"]);
            }
            catch (FormatException)
            {
                desde = DateTime.Now;
            }

            try
            {
                hasta = Convert.ToDateTime(Request["hasta"]);
            }
            catch (FormatException)
            {
                hasta = DateTime.Now;
            }

            ViewData["codigo"] = codigo;
            ViewData["nombre"] = nombre;
            ViewData["desde"] = desde.ToShortDateString();
            ViewData["hasta"] = hasta.ToShortDateString();
            ViewData["hoy"] = DateTime.Now.ToShortDateString();

            IEnumerable<SAB.Domain.Publication.Editorial> editorialList = _editorialApplication.Search(id, name, desde, hasta);

            int pageIndex = Int32.Parse(Request["pageIndex"]);

            int _pageSize = 10;
            int _totalRecords = editorialList.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            editorialList = editorialList.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);

            ViewBag.pageIndex = pageIndex;

            return View("~/Views/Publication/Editorial/EditorialSearchResultView.cshtml", editorialList);
        }

        /***************************************************************************************/

        public ActionResult EditorialDetail(int id)
        {
            SAB.Domain.Publication.Editorial editorial = _editorialApplication.QueryById(id);
            if (editorial == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return EditorialSearch();
            }

            return View("~/Views/Publication/Editorial/EditorialDetailView.cshtml", editorial);
        }

        /***************************************************************************************/

        public ActionResult EditorialModify(int id)
        {
            SAB.Domain.Publication.Editorial editorial = _editorialApplication.QueryById(id);
            if (editorial == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return EditorialSearch();
            }
            return View("~/Views/Publication/Editorial/EditorialModifyView.cshtml", editorial);
        }

        /***************************************************************************************/
 
        [HttpPost]
        public ActionResult Update(SAB.Domain.Publication.Editorial Editorial)
        {
            _editorialApplication.Update(Editorial);
            TempData["message"] = "Se ha guardado los cambio en la Editorial " + Editorial.Id + " con éxito";
            return RedirectToAction("EditorialSearch");
        }

        /***************************************************************************************/

        public ActionResult Delete(SAB.Domain.Publication.Editorial Editorial)
        {
            int resultado = _editorialApplication.Delete(Editorial);
            TempData["alert"] = "Se ha eliminado la Editorial " + Editorial.Id + " con éxito";
            return RedirectToAction("EditorialSearch");
        }

        /***************************************************************************************/
	}
}