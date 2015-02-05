using SAB.Application.Assets;
using SAB.Base.Assets;
using SAB.Domain.Assets;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Assets.AssetsType
{
    public class AssetsTypeController : Controller
    {
        private readonly TypeAssetApplication _typeAssetsApplication =
           new TypeAssetApplication(InstanceFactory.Instance.GetInstance<ITypeAssetRepository>());
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View("~/Views/Assets/AssetsType/Create.cshtml");
        }

        public ActionResult Search(string searchCode, string searchName, int pageIndex = 0)
        {
            int codigo;

            codigo= Int32.TryParse(searchCode, out codigo)? Convert.ToInt32(searchCode):0;
            ViewData["searchCode"] = Int32.TryParse(searchCode, out codigo) ? searchCode : "";
            
            if (searchName != "") ViewData["searchName"] = searchName;
            else ViewData["searchName"] = "";
            IEnumerable<TypeAsset> _list = _typeAssetsApplication.Search(codigo, searchName);
            int _pageSize = 10;
            int _totalRecords = _list.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;

            _list = _list.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);
            ViewData["pageIndex"] = pageIndex;

            ViewData["tipos"] = _list;
           
            
            return View("~/Views/Assets/AssetsType/SearchResult.cshtml");
        }

        public ActionResult SearchResult(int pageIndex=0)
        {
            ViewData["searchCode"] = "";
            ViewData["searchName"] = "";
            IEnumerable<TypeAsset> _list = _typeAssetsApplication.QueryAll();
            int _pageSize = 10;
            int _totalRecords = _list.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;

            _list = _list.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);
            ViewData["pageIndex"] = pageIndex;

            ViewData["tipos"] = _list;
            return View("~/Views/Assets/AssetsType/SearchResult.cshtml");
        }
        public ActionResult Detail(int id)

        {
             ViewData["tipo"] = _typeAssetsApplication.QueryById(id);
;            return View("~/Views/Assets/AssetsType/Detail.cshtml");
        }

        public ActionResult Modify(int id)
        {
            ViewData["tipo"] = _typeAssetsApplication.QueryById(id);
            return View("~/Views/Assets/AssetsType/Modify.cshtml");
        }

        public ActionResult Save(SAB.Domain.Assets.TypeAsset type)
        {
            _typeAssetsApplication.Insert(type);
            TempData["message"] = "Se ha registrado un nuevo Tipo de Activo";
            return RedirectToAction("SearchResult");
        }

        [HttpPost]
        public ActionResult Update(int id, string name, string description)
        {
            TypeAsset t = _typeAssetsApplication.QueryById(id);
            t.Description = description;
            t.Name = name;
            _typeAssetsApplication.Update(t);
            TempData["message"] = "Se ha guardado los cambios del Tipo de Activo " + t.Id + " con éxito";
            return RedirectToAction("SearchResult");
        }


        public ActionResult Delete(int id)
        {
            TempData["alert"] = "Se ha eliminado el activo " + id + " con éxito";
            _typeAssetsApplication.Delete(_typeAssetsApplication.QueryById(id));
            return Json(new { Url = Url.Action("SearchResult", "AssetsType") });
        }
    }
}
