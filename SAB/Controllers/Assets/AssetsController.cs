using SAB.Application.Assets;
using SAB.Application.Libray;
using SAB.Base.Assets;
using SAB.Base.Library;
using SAB.Domain.Assets;
using SAB.Domain.Library;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Assets
{
    public class AssetsController : Controller
    {
        //
        // 

        private readonly LocalApplication _localApplication =
            new LocalApplication(InstanceFactory.Instance.GetInstance<ILocalRepository>());
        private readonly AssetsApplication _assetsApplication = 
            new AssetsApplication(InstanceFactory.Instance.GetInstance<IAssetsRepository>());

        private readonly TypeAssetApplication _typeAssetsApplication =
            new TypeAssetApplication(InstanceFactory.Instance.GetInstance<ITypeAssetRepository>());

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewData["locales"] = _localApplication.QueryAll();
            ViewData["tipoActivos"] = _typeAssetsApplication.QueryAll();
            return View("~/Views/Assets/Create.cshtml");
        }

        public ActionResult Search(int pageIndex = 0)


        {

            ViewData["id"] = "";
            ViewData["assetType"] = 0;
            ViewData["tipoActivos"] = _typeAssetsApplication.QueryAll();
            IEnumerable<Asset> _list = _assetsApplication.QueryAll();
            foreach (Asset r in _list)
            {

                r.TypeAsset = _typeAssetsApplication.QueryById(r.IdAssetType);
            
            }


            int _pageSize = 10;
            int _totalRecords = _list.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;

            _list = _list.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);
            ViewData["pageIndex"] = pageIndex;
            ViewData["activos"] = _list;
            return View("~/Views/Assets/SearchResult.cshtml");
        }

        public ActionResult Search1(string id, string assetType, string dateFrom, string dateTo, int pageIndex = 0)
        
        {
            

            int codigo = Int32.TryParse(id, out codigo) ? Convert.ToInt32(id) : 0;
            ViewData["id"] = Int32.TryParse(id, out codigo) ? id : "";

            
            ViewData["assetType"] = id.Equals("") ? 0 : Convert.ToInt32(assetType);
          
            int tipoActivo = assetType.Equals("") ? 0 : Convert.ToInt32(assetType);

            DateTime fechaD = dateFrom.Equals("") ? new DateTime(1900, 01, 01) : Convert.ToDateTime(dateFrom);

            DateTime fechaH = dateTo.Equals("") ? new DateTime(2100, 01, 01) : Convert.ToDateTime(dateTo);




            IEnumerable<Asset> _list = _assetsApplication.Search(codigo, fechaD, fechaH, tipoActivo);
            foreach (Asset r in _list)
            {

                r.TypeAsset = _typeAssetsApplication.QueryById(r.IdAssetType);

            }

            int _pageSize = 10;
            int _totalRecords = _list.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;

            _list = _list.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);
            ViewData["pageIndex"] = pageIndex;
            ViewData["activos"] = _list;
            ViewData["tipoActivos"] = _typeAssetsApplication.QueryAll();
            return View("~/Views/Assets/SearchResult.cshtml");
        }

        public ActionResult Modify(int id)
        {
            ViewData["locales"] = _localApplication.QueryAll();
            ViewData["tipoActivos"] = _typeAssetsApplication.QueryAll();
            Asset activo= _assetsApplication.QueryById(id);
            activo.Biblioteca = _localApplication.QueryById(activo.Location);
            activo.TypeAsset=_typeAssetsApplication.QueryById(activo.IdAssetType);
            ViewData["activo"] = activo;
            return View("~/Views/Assets/Modify.cshtml");
        }

        public ActionResult SearchResult()
        {
           
            IEnumerable < Asset > lista = _assetsApplication.QueryAll();
            foreach (Asset r in lista)
            {

                r.TypeAsset = _typeAssetsApplication.QueryById(r.IdAssetType);

            }
            ViewData["activos"] = lista;
            ViewData["tipoActivos"] = _typeAssetsApplication.QueryAll();
            return View("~/Views/Assets/SearchResult.cshtml");
        }
        public ActionResult Detail(int id)
        {
            Asset activo = _assetsApplication.QueryById(id);
            activo.Biblioteca = _localApplication.QueryById(activo.Location);
            activo.TypeAsset = _typeAssetsApplication.QueryById(activo.IdAssetType);
            ViewData["activo"] = activo;
            return View("~/Views/Assets/Detail.cshtml");
        }


        [HttpPost]
        public ActionResult Save(string name, string description, string idAssetType, int location, string quantity="")
        {
            Asset activo = new Asset();
            if (quantity != "") activo.Quantity = Convert.ToInt32(quantity);
            else activo.Quantity = 0;
            activo.Name = name;
            activo.Description = description;
            activo.IdAssetType = Convert.ToInt32(idAssetType);
            activo.Location = location;
            _assetsApplication.Insert(activo);
            TempData["message"] = "Se ha registrado un nuevo Activo";
            return RedirectToAction("SearchResult");
        }

        [HttpPost]
        public ActionResult Update(int id, string status, int location, string description, int idAssetType, int quantity)
        {
            Asset a = new Asset();
            a.Id = id; a.State = status; a.Location = location; a.Description = description; a.IdAssetType = idAssetType; a.Quantity = quantity;
            _assetsApplication.Update(a);
            TempData["message"] = "Se ha guardado los cambios del Activo " + id + " con éxito";
            return RedirectToAction("SearchResult");
        }


        public ActionResult Delete(int id)
        {
            TempData["alert"] = "Se ha eliminado el activo " + id + " con éxito";
            _assetsApplication.Delete(_assetsApplication.QueryById(id));
            return Json(new { Url = Url.Action("SearchResult", "Assets") });
        }
    }
}