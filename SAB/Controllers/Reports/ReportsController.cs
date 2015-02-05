using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAB.Application.Publication;
using SAB.Base.Publication;
using SAB.Domain.Publication;
using SAB.Application.LoanApp;
using SAB.Base.LoanInterface;
using SAB.Domain.Loan;
using SAB.Shared;
using SAB.Base.Sanctions;
using SAB.Application.User;
using SAB.Base.User;
using SAB.Domain.User;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using Microsoft.Win32;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Data;

using SAB.Application.Acquisition;
using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;
using SAB.Domain.Sanctions;
using SAB.Infraestructure.Sanctions;
using SAB.Application.Sanctions;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;

namespace SAB.Controllers.Reports
{
    public class ReportsController : Controller
    {
        private readonly PublicationTitleApplication _publicationTitleApplication =
            new PublicationTitleApplication(InstanceFactory.Instance.GetInstance<IPublicationTitleRepository>());
        private UserAccountApplication _userAccountApplication =
            new UserAccountApplication(InstanceFactory.Instance.GetInstance<IUserAccountRepository>());
        private LoanApplication loanApplication = new LoanApplication(InstanceFactory.Instance.GetInstance<ILoanRepository>(), InstanceFactory.Instance.GetInstance<ISanctionRepository>());

        readonly private SanctionApplication _sanctionApplication =
            new SanctionApplication(InstanceFactory.Instance.GetInstance<ISanctionRepository>());


        readonly private PurchaseOrderDetailApplication _purchaseOrderDetailApplication = new PurchaseOrderDetailApplication(InstanceFactory.Instance.GetInstance<IPurchaseOrderDetailRepository>());
        private static IEnumerable<PurchaseOrderDetail> PurchaseOrderDetailReportList;
        //
        // GET: /Reports/
        public ActionResult Index()
        {
            return View();
        }

        //Aquí van todos los reportes

        public ActionResult PurchaseOrderReport()
        {
            return View("~/Views/Reports/PurchaseOrderReportView.cshtml");
        }

        public ActionResult PurchaseOrderReportResult()
        {
            return View("~/Views/Reports/PurchaseOrderReportResultView.cshtml");
        }
        public ActionResult NewPublicationReport()
        {
            return View("~/Views/Reports/NewPublicationReportView.cshtml");
        }


        public ActionResult LoansBySpecificMaterialReport()
        {
            ViewData["espublicacion"] = true;
            ViewData["reportsuccess"] = false;
            return View("~/Views/Reports/LoansBySpecificMaterialReportView.cshtml");
        }

        public ActionResult LoansBySpecificMaterialReportResult()
        {
            var user = (UserAccount)Session["usuario"];
            int publication_id = 0;
            bool esPublicacion = true;
            DateTime start = Convert.ToDateTime("1920-01-01"), end = DateTime.Now;
            int user_id = 0;
            string status = null;

            if (Request["publication_id"] != "") publication_id = Convert.ToInt32(Request["publication_id"]);
            if (Request["from"] != "") start = Convert.ToDateTime(Request["from"]);
            if (Request["to"] != "") end = Convert.ToDateTime(Request["to"]);

            esPublicacion = _publicationTitleApplication.PublicationExist(publication_id);

            ViewData["espublicacion"] = esPublicacion;
            DateTime dateFinish = end.AddDays(1);
            List<Loan> loans = (List<Loan>)loanApplication.SearchLoan(user_id, publication_id, start, dateFinish, status, user);

            if (esPublicacion)
            {
                GenerateExcelReportLoansBySpecificMaterial(loans, publication_id, start, end);

                ViewData["reportsuccess"] = true;
            }
            else
            {
                ViewData["reportsuccess"] = false;
            }
            return View("~/Views/Reports/LoansBySpecificMaterialReportView.cshtml");
        }

        public ActionResult GenerateExcelReportLoansBySpecificMaterial(List<Loan> loans, int publication_id, DateTime start, DateTime end)
        {

            ExcelPackage pck = new ExcelPackage();
            var wsDt = pck.Workbook.Worksheets.Add("Préstamos");

            //rango de celdas para el título y su formato
            wsDt.Cells[3, 4, 3, 7].Value = "Reporte de préstamos por material específico";
            wsDt.Cells[3, 4, 3, 7].Merge = true;
            wsDt.Cells[3, 3, 3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Color.SetColor(Color.FromArgb(31, 78, 120));
            wsDt.Cells[3, 3, 3, 6].Style.Font.Bold = true;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Size = 18;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Name = "Times New Roman";

            wsDt.Cells[5, 4, 5, 7].Value = "Código de publicación: "+publication_id;
            wsDt.Cells[6, 4, 6, 7].Value = "Título: "+_publicationTitleApplication.QueryById(publication_id).Title;
            wsDt.Cells[7, 4, 7, 7].Value = "Resultados válidos desde " + start.ToString("dd/MM/yyyy") + " hasta " + end.ToString("dd/MM/yyyy");

            for (int i = 0; i < 3; i++)
            {
                wsDt.Cells[5+i, 4, 5+i, 7].Merge = true;
                wsDt.Cells[5 + i, 3, 5 + i, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsDt.Cells[5 + i, 3, 5 + i, 6].Style.Font.Color.SetColor(Color.FromArgb(31, 78, 120));
                wsDt.Cells[5 + i, 3, 5 + i, 6].Style.Font.Bold = true;
                wsDt.Cells[5 + i, 3, 5 + i, 6].Style.Font.Size = 14;
                wsDt.Cells[5 + i, 3, 5 + i, 6].Style.Font.Name = "Times New Roman";
            }


            //Tabla
            DataTable dt = new DataTable();
            dt.Columns.Add("N°");
            dt.Columns.Add("Código de usuario");
            dt.Columns.Add("Número de documento");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Fecha de préstamo");

            int cont = 1;
            foreach (Loan l in loans)
            {
                dt.Rows.Add(cont,l.Loan_User.Id, _userAccountApplication.QueryById(l.Loan_User.Id).NumeroDocumento,
                            _userAccountApplication.QueryById(l.Loan_User.Id).Name + " " + _userAccountApplication.QueryById(l.Loan_User.Id).Lastname1 + " " + _userAccountApplication.QueryById(l.Loan_User.Id).Lastname2,
                            l.Register_Date.Date.ToString("dd/MM/yyyy"));
                cont++;
            }
            wsDt.Cells[9, 3].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Dark1);
            wsDt.Cells.AutoFitColumns();


            InsertarImagen(wsDt, "~/images/reporte.png");
            wsDt.View.ShowGridLines = false;
            wsDt.View.ZoomScale = 85;

            byte[] vl_bytSalida = pck.GetAsByteArray();
            /*System.Web.HttpResponse Response = HttpContext    .Current.Response;
            if (Response.IsClientConnected)
            {
                ResponseByte(vl_bytSalida, "application/vnd.ms-excel", "ListaVehiculos.xlsx");
            }*/
            Response.ClearContent();
            /*Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename= Reporte de Nuevas Publicaciones.xlsx");
            Response.ContentType = "application/vnd.ms-excel";

            Response.Charset = "";*/
            /*StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);*/
            ResponseByte(vl_bytSalida, "application/vnd.ms-excel", "Préstamos por material específico "+_publicationTitleApplication.QueryById(publication_id).Title+".xlsx");
            return Content(Response.ContentType);

            /*end.AddDays(1);
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            object misValue = System.Reflection.Missing.Value;

            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);

            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "";
            xlWorkSheet.Cells[1, 3] = "REPORTE DE PRÉSTAMOS POR MATERIAL ESPECÍFICO";
            xlWorkSheet.Cells[2, 2] = "Código de publicación: " + publication_id;
            xlWorkSheet.Cells[3, 2] = "Nombre de publicación: " + _publicationTitleApplication.QueryById(publication_id).Title;
            xlWorkSheet.Cells[4, 2] = "Resultados válidos desde " + Convert.ToString(start.Date) + " hasta " + Convert.ToString(end.Date);

            xlWorkSheet.Cells[7, 1] = "N°";
            xlWorkSheet.Cells[7, 2] = "Código de usuario";
            xlWorkSheet.Cells[7, 3] = "DNI";
            xlWorkSheet.Cells[7, 4] = "Nombre";
            xlWorkSheet.Cells[7, 5] = "Fecha de préstamo";

            int count = 1;
            foreach (Loan l in loans)
            {
                xlWorkSheet.Cells[7 + count, 1] = count;
                xlWorkSheet.Cells[7 + count, 2] = l.Loan_User.Id;
                xlWorkSheet.Cells[7 + count, 3] = _userAccountApplication.QueryById(l.Loan_User.Id).DNI;
                xlWorkSheet.Cells[7 + count, 4] = _userAccountApplication.QueryById(l.Loan_User.Id).Name + _userAccountApplication.QueryById(l.Loan_User.Id).Lastname1;
                xlWorkSheet.Cells[7 + count, 5] = Convert.ToString(l.Register_Date.Date);
                count++;
            }

            */
        }



        public ActionResult NewPublicationReportResult()
        {
            DateTime fechaD = Request["fechaD"].Equals("") ? new DateTime(1920, 01, 01) : Convert.ToDateTime(Request["fechaD"]);
            DateTime fechaH = Request["fechaH"].Equals("") ? DateTime.Now : Convert.ToDateTime(Request["fechaH"]);
            int numberC = Convert.ToInt32(Request["numeroC"]);
            PurchaseOrderDetailReportList = _purchaseOrderDetailApplication.QueryByDate(0, fechaD, fechaH);

            DateTime Ahora = DateTime.Now;
            ViewData["fechaD"] = fechaD.Date.ToShortDateString();
            ViewData["fechaH"] = fechaH.Date.ToShortDateString();
            ViewData["fecha"] = Ahora.Date.ToShortDateString();
            ViewData["hora"] = Ahora.ToShortTimeString();
            ViewData["Details"] = PurchaseOrderDetailReportList;


            return View("~/Views/Reports/NewPublicationReportResultView.cshtml");
        }

        public ActionResult ExportNewPublication()
        {
            /*
            var pod = new System.Data.DataTable("Nuevas Publicaciones");
            //pod.Rows.Add("esto es una prueba");
            pod.Columns.Add("Nº", typeof(int));
            pod.Columns.Add("Código de Material", typeof(int));
            pod.Columns.Add("Título", typeof(string));
            pod.Columns.Add("Autor", typeof(string));
            pod.Columns.Add("Cantidad", typeof(int));
            foreach (PurchaseOrderDetail p in PurchaseOrderDetailReportList)
            {
                pod.Rows.Add(p.LineNumber, p.IdPublication, p.PublicationName, p.AuthorName, p.Cantidad);
            }
            var grid = new GridView();
            grid.DataSource = pod;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename= Reporte de Nuevas Publicaciones.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);
            return Content(sw.ToString(), "application/ms-excel");
            */
            ExcelPackage pck = new ExcelPackage();
            var wsDt = pck.Workbook.Worksheets.Add("Nuevas Publicaciones");

            
            wsDt.Cells[3, 4, 3, 7].Value = "Nuevas Publicaciones";
            wsDt.Cells[3, 4, 3, 7].Merge = true;
            wsDt.Cells[3, 3, 3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Color.SetColor(Color.FromArgb(31, 78, 120));
            wsDt.Cells[3, 3, 3, 6].Style.Font.Bold = true;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Size = 18;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Name = "Times New Roman";


            //Tabla
            DataTable dt = new DataTable();
            dt.Columns.Add("Código de Material", typeof(int));
            dt.Columns.Add("Título", typeof(string));
            dt.Columns.Add("Autor", typeof(string));
            dt.Columns.Add("Cantidad", typeof(int));
            int i = 0;
            foreach (PurchaseOrderDetail p in PurchaseOrderDetailReportList)
            {
                dt.Rows.Add(p.IdPublication, p.PublicationName, p.AuthorName, p.Cantidad);
                i++;
            }
            wsDt.Cells[6, 3].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Dark1);
            wsDt.Cells.AutoFitColumns();


            InsertarImagen(wsDt, "~/images/reporte.png");
            wsDt.View.ShowGridLines = false;
            wsDt.View.ZoomScale = 85;

            byte[] vl_bytSalida = pck.GetAsByteArray();
            
            Response.ClearContent();
            
            ResponseByte(vl_bytSalida, "application/vnd.ms-excel", "Nuevas Publicaciones.xlsx");
            return Content(Response.ContentType);



        }

        public ActionResult MostWantedMaterialReport()
        {


            return View("~/Views/Reports/MostWantedMaterialReportView.cshtml");
        }

        public ActionResult GenerateExcelReportMostWantedMaterial(List<PublicationTitle> publicaciones_maspedidas, List<int> numPrestamosPorPub, DateTime start, DateTime end)
        {

            ExcelPackage pck = new ExcelPackage();
            var wsDt = pck.Workbook.Worksheets.Add("Publicaciones");

            //rango de celdas para el título y su formato
            wsDt.Cells[3, 4, 3, 7].Value = "Publicaciones más pedidas";
            wsDt.Cells[3, 4, 3, 7].Merge = true;
            wsDt.Cells[3, 3, 3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Color.SetColor(Color.FromArgb(31, 78, 120));
            wsDt.Cells[3, 3, 3, 6].Style.Font.Bold = true;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Size = 18;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Name = "Times New Roman";

            wsDt.Cells[5, 4, 5, 7].Value = "Resultados válidos desde " + start.ToString("dd/MM/yyyy") + " hasta " + end.ToString("dd/MM/yyyy");
            wsDt.Cells[5 , 4, 5 , 7].Merge = true;
            wsDt.Cells[5, 3, 5 , 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsDt.Cells[5, 3, 5 , 6].Style.Font.Color.SetColor(Color.FromArgb(31, 78, 120));
            wsDt.Cells[5, 3, 5 , 6].Style.Font.Bold = true;
            wsDt.Cells[5, 3, 5 , 6].Style.Font.Size = 14;
            wsDt.Cells[5, 3, 5 , 6].Style.Font.Name = "Times New Roman";

            //Tabla
            DataTable dt = new DataTable();
            dt.Columns.Add("Código de publicación");
            dt.Columns.Add("Nombre de publicación");
            dt.Columns.Add("Editorial");
            dt.Columns.Add("Total de préstamos");
            int i = 0;
            foreach (var p in publicaciones_maspedidas)
            {
                dt.Rows.Add(p.Id,p.Title,p.nameEditorial,numPrestamosPorPub[i]);
                i++;
            }
            wsDt.Cells[7,3].LoadFromDataTable(dt,true,OfficeOpenXml.Table.TableStyles.Dark1);
            wsDt.Cells.AutoFitColumns();

            
                    InsertarImagen(wsDt, "~/images/reporte.png");
                    wsDt.View.ShowGridLines = false;
                    wsDt.View.ZoomScale = 85;
            
                    byte[] vl_bytSalida = pck.GetAsByteArray();
                    /*System.Web.HttpResponse Response = HttpContext    .Current.Response;
                    if (Response.IsClientConnected)
                    {
                        ResponseByte(vl_bytSalida, "application/vnd.ms-excel", "ListaVehiculos.xlsx");
                    }*/
                    Response.ClearContent();
                    /*Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename= Reporte de Nuevas Publicaciones.xlsx");
                    Response.ContentType = "application/vnd.ms-excel";

                    Response.Charset = "";*/
                    /*StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);

                    grid.RenderControl(htw);*/
                    ResponseByte(vl_bytSalida, "application/vnd.ms-excel","Publicaciones más solicitadas.xlsx");
                    return Content(Response.ContentType);


            /*end.AddDays(1);
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            object misValue = System.Reflection.Missing.Value;

            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "";
            xlWorkSheet.Cells[1, 3] = "REPORTE DE PUBLICACIONES MÁS SOLICITADAS";
            xlWorkSheet.Cells[2, 2] = "Resultados válidos desde " + Convert.ToString(start.Date) + " hasta " + Convert.ToString(end.Date);

            xlWorkSheet.Cells[7, 1] = "N°";
            xlWorkSheet.Cells[7, 2] = "Código de publicación";
            xlWorkSheet.Cells[7, 3] = "Nombre de publicación";
            xlWorkSheet.Cells[7, 4] = "Editorial";
            xlWorkSheet.Cells[7, 5] = "Total de préstamos";

            int count = 1;
            foreach (PublicationTitle pub in publicaciones_maspedidas)
            {
                xlWorkSheet.Cells[7 + count, 1] = count;
                xlWorkSheet.Cells[7 + count, 2] = pub.Id;
                xlWorkSheet.Cells[7 + count, 3] = pub.Title;
                xlWorkSheet.Cells[7 + count, 4] = pub.nameEditorial;
                xlWorkSheet.Cells[7 + count, 5] = numPrestamosPorPub[count - 1];
                count++;
            }



            Response.ContentType = "application/ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=ReporteMaterialMasPedido.xls");
            Response.Write("<table>");
            for (int i = 1; i <= publicaciones_maspedidas.Count + 100; i++)
            {
                Response.Write("<tr>");
                for (int j = 1; j < 7; j++)
                {
                    if (i == 1)
                    {
                        Response.Write("<td><h3><a style='color:#0B0B61'>" + xlWorkSheet.Cells[i, j].Value + "</h3></td>");
                    }
                    else
                        Response.Write("<td><a style='color:#0A2A0A'>" + xlWorkSheet.Cells[i, j].Value + "</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</table>");
            Response.Flush();
            Response.End();*/

        }

        public ActionResult MostWantedMaterialReportResult()
        {
            var user = (UserAccount)Session["usuario"];
            int cantidad_publicaciones = 0;
            DateTime start = Convert.ToDateTime("1920-01-01"), end = DateTime.Now;
            int user_id = 0;
            string status = null;

            if (Request["cantidad_pubs"] != "") cantidad_publicaciones = Convert.ToInt32(Request["cantidad_pubs"]);
            if (Request["from"] != "") start = Convert.ToDateTime(Request["from"]);
            if (Request["to"] != "") end = Convert.ToDateTime(Request["to"]);

            IEnumerable<PublicationTitle> publicaciones = _publicationTitleApplication.QueryAll();

            int[] cant_prestamos = new int[publicaciones.Count() + 1];
            int[] publicaciones_ids = new int[publicaciones.Count() + 1];

            int contador = 0;
            DateTime dateFinish = end.AddDays(1);
            foreach (PublicationTitle pub in publicaciones)
            {
                List<Loan> loans = (List<Loan>)loanApplication.SearchLoan(user_id, pub.Id, start, dateFinish, status, user);
                cant_prestamos[contador] = loans.Count;
                publicaciones_ids[contador] = pub.Id;
                contador++;
            }

            //Ahora se ordenan los arreglos de mayor a menor (de más préstamos a menos préstamos)
            int temp;
            for (int i = 0; i < contador - 2; i++)
            {
                for (int j = 0; j < contador - 2; j++)
                {
                    if (cant_prestamos[j + 1] > cant_prestamos[j])
                    {
                        temp = cant_prestamos[j];
                        cant_prestamos[j] = cant_prestamos[j + 1];
                        cant_prestamos[j + 1] = temp;

                        temp = publicaciones_ids[j];
                        publicaciones_ids[j] = publicaciones_ids[j + 1];
                        publicaciones_ids[j + 1] = temp;
                    }
                }
            }

            //Se filtran los "x" primeros resultados de búsqueda
            List<PublicationTitle> publicaciones_maspedidas = new List<PublicationTitle>();
            List<int> numPrestamosPorPub = new List<int>();
            if (cantidad_publicaciones > contador) cantidad_publicaciones = contador;
            for (int i = 0; i < cantidad_publicaciones; i++)
            {
                publicaciones_maspedidas.Add(_publicationTitleApplication.QueryById(publicaciones_ids[i]));
                numPrestamosPorPub.Add(cant_prestamos[i]);
            }

            GenerateExcelReportMostWantedMaterial(publicaciones_maspedidas, numPrestamosPorPub, start, end);
            return View("~/Views/Reports/MostWantedMaterialReportView.cshtml");
        }

        public ActionResult UserLoansReport()
        {
            return View("~/Views/Reports/UserLoansReport.cshtml");
        }

        [HttpPost]
        public ActionResult UserLoansReportResult()
        {
            return View("~/Views/Reports/UserLoansReportResult.cshtml");
        }

        public ActionResult SanctionsReport()
        {
            return View("~/Views/Reports/SanctionsReportView.cshtml");
        }

        //[HttpPost]
        public ActionResult SanctionsReportResult()
        {
            var user = (UserAccount)Session["usuario"];
            DateTime start = Convert.ToDateTime("1920-01-01"), end = DateTime.Now;

            if (Request["from"] != "") start = Convert.ToDateTime(Request["from"]);
            if (Request["to"] != "") end = Convert.ToDateTime(Request["to"]);
            end.AddDays(1);
            List<Sanction> sanctions = (List<Sanction>)_sanctionApplication.GetSanctionsByDates(start, end);
            end.AddDays(-1);
            GenerateExcelSanctionsReport(sanctions, start, end);

            return View("~/Views/Reports/SanctionsReportView.cshtml");
        }

        public ActionResult GenerateExcelSanctionsReport(List<Sanction> sanctions, DateTime start, DateTime end)
        {          
            ExcelPackage pck = new ExcelPackage();
            var wsDt = pck.Workbook.Worksheets.Add("Sancionados");

            //rango de celdas para el título y su formato
            wsDt.Cells[3, 4, 3, 7].Value = "Reporte de sanciones";
            wsDt.Cells[3, 4, 3, 7].Merge = true;
            wsDt.Cells[3, 3, 3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Color.SetColor(Color.FromArgb(31, 78, 120));
            wsDt.Cells[3, 3, 3, 6].Style.Font.Bold = true;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Size = 18;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Name = "Times New Roman";

            wsDt.Cells[5, 4, 5, 7].Value = "Resultados válidos desde " + start.ToString("dd/MM/yyyy") + " hasta " + end.ToString("dd/MM/yyyy");
            wsDt.Cells[5, 4, 5, 7].Merge = true;
            wsDt.Cells[5, 3, 5, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsDt.Cells[5, 3, 5, 6].Style.Font.Color.SetColor(Color.FromArgb(31, 78, 120));
            wsDt.Cells[5, 3, 5, 6].Style.Font.Bold = true;
            wsDt.Cells[5, 3, 5, 6].Style.Font.Size = 14;
            wsDt.Cells[5, 3, 5, 6].Style.Font.Name = "Times New Roman";

            //Tabla
            DataTable dt = new DataTable();
            dt.Columns.Add("Número de documento");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Días de penalidad");
            dt.Columns.Add("Unidad");
            dt.Columns.Add("Tipo de multa");
            dt.Columns.Add("Fecha de multa");
            foreach (var s in sanctions)
            {
                dt.Rows.Add(_userAccountApplication.QueryById(s.Id_User).NumeroDocumento,
                            _userAccountApplication.QueryById(s.Id_User).Name + " " + _userAccountApplication.QueryById(s.Id_User).Lastname1 + " " + _userAccountApplication.QueryById(s.Id_User).Lastname2,
                            _sanctionApplication.GetSanctionType(s.Tipo_Sancion).Cantidad,_sanctionApplication.GetSanctionType(s.Tipo_Sancion).Unidad,
                            _sanctionApplication.GetSanctionType(s.Tipo_Sancion).Nombre,s.Fecha.ToString("dd/MM/yyyy"));
            }
            wsDt.Cells[7, 3].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Dark1);
            wsDt.Cells.AutoFitColumns();


            InsertarImagen(wsDt, "~/images/reporte.png");
            wsDt.View.ShowGridLines = false;
            wsDt.View.ZoomScale = 85;

            byte[] vl_bytSalida = pck.GetAsByteArray();
            /*System.Web.HttpResponse Response = HttpContext    .Current.Response;
            if (Response.IsClientConnected)
            {
                ResponseByte(vl_bytSalida, "application/vnd.ms-excel", "ListaVehiculos.xlsx");
            }*/
            Response.ClearContent();
            /*Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename= Reporte de Nuevas Publicaciones.xlsx");
            Response.ContentType = "application/vnd.ms-excel";

            Response.Charset = "";*/
            /*StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);*/
            ResponseByte(vl_bytSalida, "application/vnd.ms-excel", "Reporte de sanciones.xlsx");
            return Content(Response.ContentType);

            /*end.AddDays(1);
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            object misValue = System.Reflection.Missing.Value;

            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "";
            xlWorkSheet.Cells[1, 3] = "REPORTE DE SANCIONES POR USUARIO";
            xlWorkSheet.Cells[2, 2] = "Usuario afectado: " + usuario_afectado.Name;
            xlWorkSheet.Cells[3, 2] = "Resultados validos desde " + Convert.ToString(start.Date) + " hasta " + Convert.ToString(end.Date);

            xlWorkSheet.Cells[7, 1] = "N°";
            xlWorkSheet.Cells[7, 2] = "Codigo de sancion";
            xlWorkSheet.Cells[7, 3] = "Fecha de sanción";
            xlWorkSheet.Cells[7, 4] = "Penalidad";

            int count = 1;
            foreach(Sanction sanction in sanctions)
            {
                xlWorkSheet.Cells[7 + count, 1] = count;
                xlWorkSheet.Cells[7 + count, 2] = sanction.Id;
                xlWorkSheet.Cells[7 + count, 3] = sanction.Fecha;
                xlWorkSheet.Cells[7 + count, 4] = "";
                count++;
            }
            


            Response.ContentType = "application/ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=ReporteSanciones.xls");
            Response.Write("<table>");
            for (int i = 1; i <=  100; i++)
            {
                Response.Write("<tr>");
                for (int j = 1; j < 7; j++)
                {
                    if (i == 1)
                    {
                        Response.Write("<td><h3><a style='color:#0B0B61'>" + xlWorkSheet.Cells[i, j].Value + "</h3></td>");
                    }
                    else
                        Response.Write("<td><a style='color:#0A2A0A'>" + xlWorkSheet.Cells[i, j].Value + "</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</table>");
            Response.Flush();
            Response.End();*/

        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
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