
﻿using SAB.Application.Acquisition;
using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;
using SAB.Application.Publication;
using SAB.Base.Publication;
using SAB.Shared;
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
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using SAB.Domain.User;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;



namespace SAB.Controllers.Adquisiciones.PurchaseOrder
{
    [BasicAuthAttribute]
    public class PurchaseOrderController : Controller
    {

        // GET: /PurchaseOrder/
        readonly private PurchaseOrderApplication _purchaseOrderApplication = new PurchaseOrderApplication(InstanceFactory.Instance.GetInstance<IPurchaseOrderRepository>());
        readonly private PurchaseRequestApplication _purchaseRequestApplication = new PurchaseRequestApplication(InstanceFactory.Instance.GetInstance<IPurchaseRequestRepository>());
        readonly private PublicationTitleApplication _publicationTitleApplication = new PublicationTitleApplication(InstanceFactory.Instance.GetInstance<IPublicationTitleRepository>());
        readonly private PurchaseOrderDetailApplication _purchaseOrderDetailApplication = new PurchaseOrderDetailApplication(InstanceFactory.Instance.GetInstance<IPurchaseOrderDetailRepository>());
        readonly private PublicationTypeApplication _publicationTypeApplication = new PublicationTypeApplication(InstanceFactory.Instance.GetInstance<IPublicationTypeRepository>());
        readonly private SupplierApplication _supplierApplication = new SupplierApplication(InstanceFactory.Instance.GetInstance<ISupplierRepository>());


        /***********************************TODO********************************/
        /*
        -arreglar merge sp updatebyid
        -modifcar infraestructure de publicationTitle
        -probar funcionalidad de los botones de OC        
        */
        /***********************************FIN***********************************/


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            //ViewBag.PurchaseRequests = _purchaseRequestApplication.Search(0, new DateTime(1900, 01, 01), new DateTime(2100, 01, 01), 0, 0, null).Take(5);

            ViewBag.Publications = _publicationTitleApplication.SearchPublicationsByFilters(new PublicationTitle()).Take(5);
            ViewBag.PublicationTypes = _publicationTypeApplication.QueryAll();
            ViewBag.Providers = _supplierApplication.Search("", "", "", "", "", "").Take(5);

            return View("~/Views/Adquisiciones/PurchaseOrder/Create.cshtml");
        }

        /*public ActionResult Search()
        {
            DateTime fechaDesde = new DateTime(1900, 01, 01) ;
            DateTime fechaHasta = new DateTime(2100, 01, 01) ;

            return View("~/Views/Adquisiciones/PurchaseOrder/Search.cshtml", _purchaseOrderApplication.
                Search(0,fechaDesde,fechaHasta,null,null,1));
        }*/

        public ActionResult SearchResult()
        {
            return View("~/Views/Adquisiciones/PurchaseOrder/SearchResult.cshtml");
        }
        public ActionResult Detail(int id)
        {
            /*Jalas las cosas con ViewData, checka el view de detail*/
            SAB.Domain.Acquisition.PurchaseOrder p = _purchaseOrderApplication.QueryById(id);
            IEnumerable<SAB.Domain.Acquisition.PurchaseRequest> pr = _purchaseRequestApplication.QueryByOrder(id);
            IEnumerable<SAB.Domain.Acquisition.PurchaseOrderDetail> pod = _purchaseOrderDetailApplication.QueryByOrder(id);
            ViewData["PurchaseOrder"] = p;
            ViewData["PurchaseRequests"] = pr;
            ViewData["PurchaseOrderDetails"] = pod;
            /*tienes que pasar el objeto con todo y proveedor
             * en ViewData[PurchaseOrder]
             * ViewData[PurchaseOrderDetails]  la lista de publicaciones y sus cantidades
             * ViewData[PurchaseRequests] la lista de Solicitudes de compra
            */

            return View("~/Views/Adquisiciones/PurchaseOrder/Detail.cshtml");
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TempData["alert"] = "Se ha eliminado el Tipo de Publicación " + id + " con éxito";
            _purchaseOrderApplication.Delete(id);
            return Json(new { Url = Url.Action("Search", "PurchaseOrder") });
        }

        public ActionResult Reception(int id)
        {
            SAB.Domain.Acquisition.PurchaseOrder p = _purchaseOrderApplication.QueryById(id);
            
            IEnumerable<SAB.Domain.Acquisition.PurchaseOrderDetail> pod = _purchaseOrderDetailApplication.QueryByOrder(id);
            ViewData["PurchaseOrder"] = p;
            
            ViewData["PurchaseOrderDetails"] = pod;
            return View("~/Views/Adquisiciones/PurchaseOrder/Reception.cshtml");
        }
        [HttpPost]
        public ActionResult Save(FormCollection form)
        {

            //string[] requestIDs = form["requestIDs"] == null ? null : form["requestIDs"].Split(',');
            string[] publicationIDs = form["publicationIDs"] == null ? null : form["publicationIDs"].Split(',');
            string[] publicationNames = form["publicationNames"] == null ? null : form["publicationNames"].Split(',');
            string[] requestNames = form["requestNames"] == null ? null : form["requestNames"].Split(',');
            string[] publicationQuantities = form["publicationQuantities"] == null ? null : form["publicationQuantities"].Split(',');
            string proveedorID = form["providerID"] == null ? "" : form["providerID"];

            //if (requestIDs != null)
            //    ViewData["numRequests"] = requestIDs.Length;
            if (publicationIDs != null)
                ViewData["numPublications"] = publicationIDs.Length;

            //ViewData["requestIDs"] = requestIDs;
            ViewData["publicationIDs"] = publicationIDs;
            ViewData["publicationNames"] = publicationNames;
            ViewData["requestNames"] = requestNames;
            ViewData["publicationQuantities"] = publicationQuantities;




            if (publicationIDs != null && !proveedorID.Equals(String.Empty))
            {


                TempData["message"] = "Se ha registrado su orden de compra exitosamente";
                _purchaseOrderApplication.Insert(proveedorID, publicationIDs, publicationQuantities);
                return RedirectToAction("Search");
            }
            else
            {
                string alert = "";
                if (publicationIDs == null)
                    alert += "\nNo se ha agregado ninguna publicación a la orden de compra";
                if (proveedorID.Equals(String.Empty))
                    alert += "\nNo se ha asignado ningun proveedor a la orden de compra";
                TempData["alert"] = alert;
            }


            ViewData["PurchaseRequests"] = _purchaseRequestApplication.QueryAll();
            ViewData["Publications"] = _publicationTitleApplication.QueryAll();
            ViewBag.PublicationTypes = _publicationTypeApplication.QueryAll();
            ViewBag.Providers = _supplierApplication.QueryAll();

            return View("~/Views/Adquisiciones/PurchaseOrder/Create.cshtml");


        }

        public ActionResult Search(FormCollection form = null)
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
                string proveedor = form["proveedor"].Equals("") ? null : form["proveedor"];
                int pageIndex = Int32.Parse(form["pageIndex"]);

                IEnumerable<SAB.Domain.Acquisition.PurchaseOrder> list;
                //if (form["state"].Equals("Todos") && codigo == 0 && form["proveedor"].Equals("") && form["fechaDesde"].Equals("") && form["fechaHasta"].Equals(""))
                //list = _purchaseOrderApplication.QueryAll();
                //else


                list = _purchaseOrderApplication.Search(codigo, fechaDesde, fechaHasta, state, proveedor, ref pageIndex);

                ViewData["codigo"] = form["codigo"];
                ViewData["fechaD"] = form["fechaDesde"];
                ViewData["fechaH"] = form["fechaHasta"];
                ViewData["provider"] = form["proveedor"];
                ViewBag.pageIndex = pageIndex;
                return View("~/Views/Adquisiciones/PurchaseOrder/Search.cshtml", list);
            }
            else
            {
                int pageIndex = 1;
                DateTime fechaDesde = new DateTime(1900, 01, 01);
                DateTime fechaHasta = new DateTime(2100, 01, 01);

                return View("~/Views/Adquisiciones/PurchaseOrder/Search.cshtml", _purchaseOrderApplication.
                    Search(0, fechaDesde, fechaHasta, null, null, ref pageIndex));
            }

        }


        public ActionResult Approve(int id)
        {


            _purchaseOrderApplication.Approve(id);
            TempData["message"] = "Se ha aprobado la Orden de Compra " + id + " con éxito";


            return RedirectToAction("Search");


        }

        public ActionResult Reject(int id)
        {



            _purchaseOrderApplication.Reject(id);
            TempData["alert"] = "Se ha rechazado la Orden de Compra " + id + " con éxito";

            return RedirectToAction("Search");


        }

        public ActionResult RequestSearch(int pageIndex = 1)
        {

            int id = Request["codigo"] == null || Request["codigo"] == "" ? 0 : Convert.ToInt32(Request["codigo"]);
            DateTime fechaD = Request["fechaD"] == null || Request["fechaD"] == "" ? new DateTime(1900, 01, 01) : Convert.ToDateTime(Request["fechaD"]);
            DateTime fechaH = Request["fechaH"] == null || Request["fechaD"] == "" ? new DateTime(2100, 01, 01) : Convert.ToDateTime(Request["fechaH"]);
            
            IEnumerable<SAB.Domain.Acquisition.PurchaseRequest> _list = _purchaseRequestApplication.Search(id, fechaD, fechaH, 0, 0, null);
            int _pageSize = 5;
            int _totalRecords = _list.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            //IEnumerable<PurchaseOrder> list = _purchaseOrderList.ToList();
            _list = _list.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);

            ViewBag.pageIndex = pageIndex;
            return PartialView("~/Views/Adquisiciones/PurchaseOrder/_PartialRequestTable.cshtml", _list);
        }

        public ActionResult PublicationSearch(string txtTitulo, string txtAutor, int tipoPublicacion, int idProvider, int pageIndex = 1)
        {

            txtTitulo = txtTitulo.Trim();
            txtAutor = txtAutor.Trim();
            PublicationTitle _publication = new PublicationTitle();
            _publication.Title = txtTitulo;
            _publication.nameAuthor = txtAutor;
            _publication.Id_Type = tipoPublicacion;
            _publication.providerId = idProvider;


            IEnumerable<PublicationTitle> _list = _publicationTitleApplication.SearchPublicationsByFilters(_publication);
            int _pageSize = 5;
            int _totalRecords = _list.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            //IEnumerable<PurchaseOrder> list = _purchaseOrderList.ToList();
            _list = _list.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);

            ViewBag.pageIndex = pageIndex;
            return PartialView("~/Views/Adquisiciones/PurchaseOrder/_PartialPublicationTable.cshtml", _list);


        }

        public ActionResult ProviderSearch(string ruc, string providerName, int pageIndex = 1)
        {
            IEnumerable<SAB.Domain.Acquisition.Supplier> _list = _supplierApplication.Search(providerName, "", "", "", "", ruc);
            int _pageSize = 5;
            int _totalRecords = _list.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            //IEnumerable<PurchaseOrder> list = _purchaseOrderList.ToList();
            _list = _list.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);
            ViewBag.pageIndex = pageIndex;
            return PartialView("~/Views/Adquisiciones/PurchaseOrder/_PartialProviderTable.cshtml", _list);
        }

        public ActionResult ExportToExcel(int id)
        {
            ExcelPackage pck = new ExcelPackage();
            var wsDt = pck.Workbook.Worksheets.Add("Orden De Compra");

            //rango de celdas para el título y su formato
            wsDt.Cells[3, 4, 3, 7].Value = "Orden de Compra-" + id;
            wsDt.Cells[3, 4, 3, 7].Merge = true;
            wsDt.Cells[3, 3, 3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Color.SetColor(Color.FromArgb(31, 78, 120));
            wsDt.Cells[3, 3, 3, 6].Style.Font.Bold = true;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Size = 18;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Name = "Times New Roman";


            //Tabla

            DataTable dt = new DataTable();

            dt.Columns.Add("Nombre de publicación");
            dt.Columns.Add("Cantidad");
            var _oc = _purchaseOrderApplication.QueryById(id);
            var _dOC = _purchaseOrderDetailApplication.QueryByOrder(id);
            var _proveedor = _oc.PurchaseOrder_Provider.Name;

            foreach (var orden in _dOC)
            {
                dt.Rows.Add(orden.PublicationName, orden.Cantidad);

            }
            wsDt.Cells[6, 3].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Dark1);
            wsDt.Cells.AutoFitColumns();


            InsertarImagen(wsDt, "~/images/reporte.png");
            wsDt.View.ShowGridLines = false;
            wsDt.View.ZoomScale = 85;

            byte[] vl_bytSalida = pck.GetAsByteArray();

            Response.ClearContent();

            ResponseByte(vl_bytSalida, "application/vnd.ms-excel", "OrdenDeCompra-Nro-" + id + ".xlsx");
            return Content(Response.ContentType);
        }


        public ActionResult ReceptionSave(FormCollection form)
        {

            string[] purchaseOrderDetailIDs = form["purchaseOrderDetailIDs"] == null ? null : form["purchaseOrderDetailIDs"].Split(',');
            string[] publicationIDs = form["publicationIDs"] == null ? null : form["publicationIDs"].Split(',');
            string[] publicationNames = form["publicationNames"] == null ? null : form["publicationNames"].Split(',');
            string[] publicationQuantitiesReceived = form["publicationQuantitiesReceived"] == null ? null : form["publicationQuantitiesReceived"].Split(',');
            string[] publicationQuantitiesPlus = form["publicationQuantitiesPlus"] == null ? null : form["publicationQuantitiesPlus"].Split(',');
            string proveedorID = form["providerID"] == null ? "" : form["providerID"];

            if (publicationIDs != null)
                ViewData["numPublications"] = publicationIDs.Length;

            ViewData["publicationIDs"] = publicationIDs;
            ViewData["publicationNames"] = publicationNames;
            //ViewData["publicationQuantities"] = publicationQuantities;




                TempData["message"] = "Se ha registrado su recepcion exitosamente";
                _purchaseOrderApplication.ReceptionUpdate(purchaseOrderDetailIDs, publicationIDs,publicationQuantitiesReceived, publicationQuantitiesPlus);
            


            ViewData["PurchaseRequests"] = _purchaseRequestApplication.QueryAll();
            ViewData["Publications"] = _publicationTitleApplication.QueryAll();
            ViewBag.PublicationTypes = _publicationTypeApplication.QueryAll();
            ViewBag.Providers = _supplierApplication.QueryAll();

            return RedirectToAction("Search");


        }

        void ResponseByte(byte[] vl_bytSalida, string ContentType, string fileName)
        {
            //Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            Response.ContentType = ContentType;
            Response.BinaryWrite(vl_bytSalida);
            Response.Flush();
            Response.Close();
        }

        void setBackground(ExcelRange excel, Color color, ExcelFillStyle style = ExcelFillStyle.Solid)
        {
            excel.Style.Fill.PatternType = style;
            excel.Style.Fill.BackgroundColor.SetColor(color);
            excel.Style.Font.Bold = true;
        }

        void InsertarImagen(ExcelWorksheet wsDt, string Path)
        {
            var picture = wsDt.Drawings.AddPicture("a", System.Drawing.Image.FromFile(Request.MapPath(Path)));
            picture.SetPosition(0, 0, 0, 0);
            picture.SetSize(80);

        }


    }


}


