using SAB.Application.Acquisition;
using SAB.Base.Acquisition;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Adquisiciones.Supplier
{
    public class SupplierController : Controller
    {
        readonly private SupplierApplication _supplierApplication = new SupplierApplication(InstanceFactory.Instance.GetInstance<ISupplierRepository>());

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SupplierRegister()
        {

            return View("~/Views/Adquisiciones/Supplier/SupplierRegisterView.cshtml");
        }

        public ActionResult SupplierSearch()

        {
            ViewData["allSupplier"] = _supplierApplication.QueryAll();
            return View("~/Views/Adquisiciones/Supplier/SupplierSearchResultView.cshtml");
        }


        public ActionResult ModifySupplier(int id)


        {
            ViewData["supplier"] = _supplierApplication.QueryById(id);
            return View("~/Views/Adquisiciones/Supplier/SupplierModifyView.cshtml");
        }



        public ActionResult SearchSupplierResult(string searchName,string searchCode,string from,string to, string searchContacto,string searchRUC)
        {
            ViewData["allSupplier"] = _supplierApplication.Search(searchName, searchCode, from, to, searchContacto, searchRUC);
            return View("~/Views/Adquisiciones/Supplier/SupplierSearchResultView.cshtml");
        }


        public ActionResult SearchSupplierResultDetail(int id)


        {
            ViewData["supplier"] = _supplierApplication.QueryById(id);
            return View("~/Views/Adquisiciones/Supplier/SupplierDetailView.cshtml");
        }


        [HttpPost]
        public ActionResult Save(SAB.Domain.Acquisition.Supplier supplier)
            
        {
            long number;
            if (Int64.TryParse(supplier.Ruc, out number) && supplier.Ruc.Length == 11)
            {
                _supplierApplication.Insert(supplier);
                TempData["message"] = "Se ha registrado un Nuevo Proveedor";
                ViewData["allSupplier"] = _supplierApplication.QueryAll();
                return View("~/Views/Adquisiciones/Supplier/SupplierSearchResultView.cshtml");
            }
            else {

                ViewData["proveedor"] = supplier;
                TempData["alert"] = "Este RUC es invalido";
                return View("~/Views/Adquisiciones/Supplier/SupplierRegisterView.cshtml");
            
            }
        }

        [HttpPost]
        public ActionResult Update(int id,string nombre, string contacto,string direccion,string telefono,string correo)
        {
            _supplierApplication.Actualiza(id, nombre, contacto, direccion, telefono, correo);
            TempData["message"] = "Se ha guardado los cambios del Proveedor " + id + " con éxito";

            ViewData["allSupplier"] = _supplierApplication.QueryAll();
            return View("~/Views/Adquisiciones/Supplier/SupplierSearchResultView.cshtml");
        }


        public ActionResult Delete(int id)
        {
            TempData["alert"] = "Se ha eliminado el proveedor " + id + " con éxito";
            _supplierApplication.Delete(id);


            return Json(new { Url = Url.Action("SupplierSearch", "Supplier") });
        }



    }
}