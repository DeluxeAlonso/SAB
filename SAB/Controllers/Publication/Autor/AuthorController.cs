using SAB.Application.Publication;
using SAB.Base.Publication;
using SAB.Domain.Publication;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Publication.Author
{
    [BasicAuthAttribute]
    public class AuthorController : Controller
    {
        /*********************************************************************************************/

        readonly private AuthorApplication _authorApplication = 
            new AuthorApplication(InstanceFactory.Instance.GetInstance<IAuthorRepository>());

        /*********************************************************************************************/

        public ActionResult Index()
        {
            return View();
        }

        /*********************************************************************************************/

        public ActionResult AuthorRegister()
        {

            return View("~/Views/Publication/Author/AuthorRegisterView.cshtml");
        }

        /*********************************************************************************************/

        public ActionResult AuthorSearch()
        {
            ViewData["pais"] = "";
            ViewBag.pageIndex = null;

            return View("~/Views/Publication/Author/AuthorSearchView.cshtml");
        }

        /*********************************************************************************************/

        public ActionResult AuthorSearchResult()
        {
            string codigo, nombre, pais;

            codigo = Convert.ToString(Request["codigo"]);
            nombre = Convert.ToString(Request["nombre"]);
            pais = Convert.ToString(Request["pais"]);

            int id = codigo.Equals("") ? 0 : Convert.ToInt32(codigo);
            string name = nombre.Equals("") ? null : nombre;
            pais = pais.Equals("Todos") ? "" : pais;

            ViewData["codigo"] = codigo;
            ViewData["nombre"] = name;
            ViewData["pais"] = pais;

            IEnumerable<SAB.Domain.Publication.Author> resultado = _authorApplication.Search(id, name, pais);

            int pageIndex = Int32.Parse(Request["pageIndex"]);

            int _pageSize = 10;
            int _totalRecords = resultado.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            resultado = resultado.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);

            ViewBag.pageIndex = pageIndex;

            return View("~/Views/Publication/Author/AuthorSearchResultView.cshtml", resultado);
        }


        /*********************************************************************************************/

        public ActionResult AuthorDetail(int id)
        {
            SAB.Domain.Publication.Author resultado = _authorApplication.QueryById(id);
            return View("~/Views/Publication/Author/AuthorDetailView.cshtml", resultado);
        }

        /*********************************************************************************************/

        public ActionResult AuthorModify(int id)
        {
            SAB.Domain.Publication.Author resultado = _authorApplication.QueryById(id);
            return View("~/Views/Publication/Author/AuthorModifyView.cshtml", resultado);
        }


        /*********************************************************************************************/

        [HttpPost]
        public ActionResult NewAuthor(SAB.Domain.Publication.Author author)
        {
            if (author.Second_last_Name == null) author.Second_last_Name = "";
            _authorApplication.Insert(author);
            TempData["message"] = "Se ha registrado el nuevo Autor";
            return RedirectToAction("AuthorSearch");
        }

        /*********************************************************************************************/

        [HttpPost]
        public ActionResult UpdateAuthor(SAB.Domain.Publication.Author author)
        {
            _authorApplication.Update(author);
            TempData["message"] = "Se ha guardado los cambio en el Autor " + author.Id + " con éxito";
            return RedirectToAction("AuthorSearch");
        }

        /*********************************************************************************************/

        public ActionResult DeleteAuthor(int id)
        {
            TempData["alert"] = "Se ha eliminado el Autor " + id + " con éxito";
            return RedirectToAction("AuthorSearchResult");
        }

        /*********************************************************************************************/
	}
}