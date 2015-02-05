using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAB.Domain.Publication;
using System.Text.RegularExpressions;
using System.Text;
using SAB.Domain.Library;
using SAB.Application.Libray;
using SAB.Base.Library;
using SAB.Application.Acquisition;
using SAB.Shared;
using SAB.Base.Acquisition;
using SAB.Application.Publication;
using SAB.Base.Publication;
using SAB.Domain.User;

namespace SAB.Controllers.Adquisiciones.PurchaseRequest
{
    public class PurchaseRequestController : Controller
    {

        readonly private PurchaseOrderApplication _purchaseOrderApplication = new PurchaseOrderApplication(InstanceFactory.Instance.GetInstance<IPurchaseOrderRepository>());
        readonly private PurchaseRequestApplication _purchaseRequestApplication = new PurchaseRequestApplication(InstanceFactory.Instance.GetInstance<IPurchaseRequestRepository>());
        readonly private PublicationTitleApplication _publicationTitleApplication = new PublicationTitleApplication(InstanceFactory.Instance.GetInstance<IPublicationTitleRepository>());
        readonly private PurchaseOrderDetailApplication _purchaseOrderDetailApplication = new PurchaseOrderDetailApplication(InstanceFactory.Instance.GetInstance<IPurchaseOrderDetailRepository>());
        readonly private PurchaseRequestDetailApplication _purchaseRequestDetailApplication = new PurchaseRequestDetailApplication(InstanceFactory.Instance.GetInstance<IPurchaseRequestDetailRepository>());
        readonly private PublicationTypeApplication _publicationTypeApplication = new PublicationTypeApplication(InstanceFactory.Instance.GetInstance<IPublicationTypeRepository>());
        readonly private SupplierApplication _supplierApplication = new SupplierApplication(InstanceFactory.Instance.GetInstance<ISupplierRepository>());

        // GET: PurchaseRequest
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PurchaseRequestRegister()
        {

            ViewBag.Publications = _publicationTitleApplication.SearchPublicationsByFilters(new PublicationTitle()).Take(5);
            ViewBag.PublicationTypes = _publicationTypeApplication.QueryAll();
            

            return View("~/Views/Adquisiciones/PurchaseRequest/PurchaseRequestRegisterView.cshtml");
        }

        public ActionResult PurchaseRequestSearch()
        {
            return View("~/Views/Adquisiciones/PurchaseRequest/PurchaseRequestSearchView.cshtml");
        }

        public ActionResult NewPublicationRequest()
        {
            return View("~/Views/Adquisiciones/PurchaseRequest/NewPublicationRequest.cshtml");
        }

        public ActionResult PurchaseRequestSearchResult()
        {

            return View("~/Views/Adquisiciones/PurchaseRequest/PurchaseRequestSearchResultView.cshtml", _purchaseRequestApplication.QueryAll());
        }

        public ActionResult PurchaseRequestSearchResult2(FormCollection form=null)
        {
            if (form.Count != 0)
            {
                int codigo;
                if (form["codigo"] == null)
                {
                    codigo = 0;

                }
                else if (form["codigo"].Equals(""))
                {
                    codigo = 0;
                }
                else
                    codigo = Convert.ToInt32(form["codigo"]);

                DateTime fechaDesde = form["fechaDesde"].Equals("") ? new DateTime(1900, 01, 01) : Convert.ToDateTime(form["fechaDesde"]);
                DateTime fechaHasta = form["fechaHasta"].Equals("") ? new DateTime(2100, 01, 01) : Convert.ToDateTime(form["fechaHasta"]);
                

                string state;

                if (form["state"].Equals("Todos"))
                {
                    state = null;
                }
                else state = form["state"];
                
                string solicitante = form["solicitante"].Equals("") ? null : form["solicitante"];


                IEnumerable<SAB.Domain.Acquisition.PurchaseRequest> _list;
                if (form["state"].Equals("Todos") && codigo == 0  && form["fechaDesde"].Equals("") && form["fechaHasta"].Equals("") && form["solicitante"].Equals(""))
                    _list = _purchaseRequestApplication.QueryAll();
                else
                    _list = _purchaseRequestApplication.Search2(codigo, fechaDesde, fechaHasta, state, solicitante);
                ViewData["codigo"] = form["codigo"];
                ViewData["fechaD"] = form["fechaDesde"];
                ViewData["fechaH"] = form["fechaHasta"];
                
                ViewData["solicitante"] = form["solicitante"];

                int pageIndex = Int32.Parse(form["pageIndex"]);
                int _pageSize = 10;
                int _totalRecords = _list.Count();
                int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
                if (pageIndex < 1) pageIndex = 1;
                if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
                //IEnumerable<PurchaseOrder> list = _purchaseOrderList.ToList();
                _list = _list.
                    Skip((pageIndex - 1) * _pageSize).
                    Take(_pageSize);
                ViewBag.pageIndex = pageIndex;
                return View("~/Views/Adquisiciones/PurchaseRequest/PurchaseRequestSearchResultView.cshtml", _list);
            }
            else
            {

                return View("~/Views/Adquisiciones/PurchaseRequest/PurchaseRequestSearchResultView.cshtml", _purchaseRequestApplication.QueryAll().Take(10));

            }
        }
        

        public ActionResult PurchaseRequestDetail(int id, int tipo)
        {
            if (tipo == 1)
            {
                SAB.Domain.Acquisition.PurchaseRequest p = _purchaseRequestApplication.QueryById(id);
                IEnumerable<SAB.Domain.Acquisition.PurchaseRequestDetail> prd = _purchaseRequestDetailApplication.QueryByRequest(id);

                ViewData["PurchaseRequest"] = p;
                ViewData["PurchaseRequestDetails"] = prd;
                return View("~/Views/Adquisiciones/PurchaseRequest/PurchaseRequestDetailView.cshtml");
            }
            else
            {
                SAB.Domain.Acquisition.PurchaseRequest p = _purchaseRequestApplication.QueryById(id);
                IEnumerable<SAB.Domain.Acquisition.PurchaseRequestDetailN> prd = _purchaseRequestDetailApplication.QueryByRequestN(id);

                ViewData["PurchaseRequest"] = p;
                ViewData["PurchaseRequestDetails"] = prd;
                return View("~/Views/Adquisiciones/PurchaseRequest/DetailNew.cshtml");
            }
        }
        public ActionResult PurchaseRequestModify()
        {
            return View("~/Views/Adquisiciones/PurchaseRequest/PurchaseRequestModifyView.cshtml");
        }

        [HttpPost]
        public ActionResult NewPurchaseRequest()
        {
            TempData["message"] = "Se ha registrado una nueva Solicitud de Compras";
            return RedirectToAction("PurchaseRequestSearch");
        }


        [HttpPost]
        public ActionResult UpdatePurchaseRequest(int id)
        {
            TempData["message"] = "Se ha guardado los cambio en la Solicitud de Compra " + id + " con éxito";
            return RedirectToAction("PurchaseRequestSearchResult");
        }

        [HttpPost]
        public ActionResult DeletePurchaseRequest(int id)
        {

            TempData["alert"] = "Se ha eliminado la Solicitud de Compra " + id + " con éxito";
            _purchaseRequestApplication.Delete(id);
            return Json(new { Url = Url.Action("PurchaseRequestSearchResult", "PurchaseRequest") });
        }
        public ActionResult PublicationSearch(string txtTitulo, string txtAutor, int tipoPublicacion)
        {

            txtTitulo = txtTitulo.Trim();
            txtAutor = txtAutor.Trim();
            PublicationTitle _publication = new PublicationTitle();
            _publication.Title = txtTitulo;
            _publication.nameAuthor = txtAutor;
            _publication.Id_Type = tipoPublicacion;


            return PartialView("~/Views/Adquisiciones/PurchaseOrder/_PartialPublicationTable.cshtml", _publicationTitleApplication.SearchPublicationsByFilters(_publication));


        }
        [HttpPost]
        public ActionResult Save(FormCollection form)
        {

            
            string[] publicationIDs = form["publicationIDs"] == null ? null : form["publicationIDs"].Split(',');
            string[] publicationNames = form["publicationNames"] == null ? null : form["publicationNames"].Split(',');           
            string[] publicationQuantities = form["publicationQuantities"] == null ? null : form["publicationQuantities"].Split(',');
            string descripcion = form["descripcion"]==null? null: form["descripcion"].Split(',')[0];
            
            if (publicationIDs != null)
                ViewData["numPublications"] = publicationIDs.Length;

          
            ViewData["publicationIDs"] = publicationIDs;
            ViewData["publicationNames"] = publicationNames;            
            ViewData["publicationQuantities"] = publicationQuantities;




            if (publicationIDs != null)
            {


                TempData["message"] = "Se ha registrado su solicidud de compra exitosamente";
                //_purchaseRequestApplication
                _purchaseRequestApplication.Insert(publicationIDs, publicationQuantities, descripcion, ((UserAccount)Session["usuario"]).Id);
                return RedirectToAction("PurchaseRequestSearchResult2");
            }
            else
            {
                string alert = "";
                if (publicationIDs == null)
                    alert += "\nNo se ha agregado ninguna publicación a la orden de compra";                
                TempData["alert"] = alert;
            }


            ViewData["Publications"] = _publicationTitleApplication.QueryAll();
            ViewBag.PublicationTypes = _publicationTypeApplication.QueryAll();


            return View("~/Views/Adquisiciones/PurchaseRequest/PurchaseRequestRegisterView.cshtml");


        }
        public ActionResult Approve(int id)
        {


            _purchaseRequestApplication.Approve(id);
            TempData["message"] = "Se ha aprobado la Solicitud de Compra " + id + " con éxito";


            return RedirectToAction("PurchaseRequestSearchResult");


        }

        public ActionResult Reject(int id)
        {



            _purchaseRequestApplication.Reject(id);
            TempData["alert"] = "Se ha rechazado la Solicitud de Compra " + id + " con éxito";

            return RedirectToAction("PurchaseRequestSearchResult");


        }

        [HttpPost]
        public ActionResult SaveNew(FormCollection form)
        {


            string[] ISBNs = form["ISBNs"] == null ? null : form["ISBNs"].Split(',');
            string[] publicationes = form["publicaciones"] == null ? null : form["publicaciones"].Split(',');
            string[] proveedores = form["proveedores"] == null ? null : form["proveedores"].Split(',');
            string[] precios = form["precios"] == null ? null : form["precios"].Split(',');
            _purchaseRequestApplication.InsertN(ISBNs, publicationes, null, proveedores, precios, ((UserAccount)Session["usuario"]).Id);

            TempData["message"] = "Se ha registrado su solicidud de compra exitosamente";
                
            return RedirectToAction("PurchaseRequestSearchResult2");
            


        }



    }
}