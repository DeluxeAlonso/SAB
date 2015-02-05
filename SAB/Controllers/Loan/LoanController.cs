using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAB.Application.LoanApp;
using SAB.Base.LoanInterface;
using SAB.Domain.Publication;
using SAB.Domain.Loan;
using SAB.Shared;
using SAB.Domain.Reserves;
using SAB.Base.Sanctions;
using SAB.Domain.User;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Data;

namespace SAB.Controllers.LoanController
{
    [BasicAuthAttribute]
    public class LoanController : Controller
    {
        private LoanApplication loanApplication = new LoanApplication(InstanceFactory.Instance.GetInstance<ILoanRepository>(), InstanceFactory.Instance.GetInstance<ISanctionRepository>());
        private static List<Loan> loansList = new List<Loan>();
        private static List<Loan> loansReportList = new List<Loan>();
        private static Loan currentLoan = new Loan();

        public ActionResult Index()
        {
            var user =  (UserAccount)Session["usuario"];
            return View();
        }

        public ActionResult LoanRegister()
        {
            ViewData["loanTypes"] = loanApplication.QueryAllLoanTypes();
            return View("~/Views/Loan/LoanRegisterView.cshtml");
        }

        public ActionResult LoanSearch(FormCollection form)
        {
            var user =  (UserAccount)Session["usuario"];
            ViewData["loans"] = loanApplication.QueryAllP(user);
            IEnumerable<Loan> _list = (IEnumerable<Loan>)ViewData["loans"];
            int pageIndex;
            int _pageSize = 10;
            int _totalRecords;
            try
            {
                _totalRecords = _list.Count();
            }
            catch (Exception)
            {
                _list = new List<Loan>();
                _totalRecords = 0;
            }

            try
            {
                pageIndex = Convert.ToInt32(form["pageIndex"]);
            }
            catch (Exception)
            {
                pageIndex = 1;
            }


            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            //IEnumerable<PurchaseOrder> list = _purchaseOrderList.ToList();
            ViewData["loans"] = (_list.Skip((pageIndex - 1) * _pageSize).Take(_pageSize)).ToList();
            ViewBag.pageIndex = pageIndex;
            ViewData["all"] = true;
            ViewData["active"] = false;
            ViewData["ended"] = false;
            return View("~/Views/Loan/LoanSearchView.cshtml");
        }




        [HttpPost]
        public ActionResult Search(FormCollection form)
        {
            var user = (UserAccount)Session["usuario"];
            DateTime start = Convert.ToDateTime("1920-01-01"), end = DateTime.Now;
            int user_id = 0, publication_id = 0;
            string status = null;
            bool all = false, active = false, ended = false;
            int pageIndex = Convert.ToInt32(form["pageIndex"]);

            try
            {
                if (Request["from"] != "") start = Convert.ToDateTime(Request["from"]);
                if (Request["to"] != "") end = Convert.ToDateTime(Request["to"]);
            }
            catch (Exception)
            {
                TempData["alert"] = "Ingrese una fecha válida.";
                return RedirectToAction("LoanSearch", "Loan");
            }
            try
            {
                if (Request["user_id"] != "") user_id = Convert.ToInt32(Request["user_id"]);
            }
            catch (Exception)
            {
                user_id = -1;
            }
            try
            {
                if (Request["publication_id"] != "") publication_id = Convert.ToInt32(Request["publication_id"]);
            }
            catch (Exception)
            {
                publication_id = -1;
            }


            
            if (Request["estados"] != "Todos") status = Convert.ToString(Request["estados"]);

            SetupSelectedValues(status, ref all, ref active, ref ended);

            ViewData["all"] = all;
            ViewData["active"] = active;
            ViewData["ended"] = ended;

            ViewData["from"] = Request["from"];
            ViewData["to"] = Request["to"];
            ViewData["user_id"] = Request["user_id"];
            ViewData["publication_id"] = Request["publication_id"];

            ViewData["loans"] = loanApplication.SearchLoan(user_id, publication_id, start, end, status, user);



            //int pageIndex = Int32.Parse(form["pageIndex"]);zz
            IEnumerable<Loan> _list = (IEnumerable<Loan>)ViewData["loans"];
            int _pageSize = 10;
            int _totalRecords;

            try
            {
                _totalRecords = _list.Count();
            }
            catch (Exception)
            {
                _list = new List<Loan>();
                _totalRecords = 0;
            }

            try
            {
                pageIndex = Convert.ToInt32(form["pageIndex"]);
            }
            catch (Exception)
            {
                pageIndex = 1;
            }

            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            //IEnumerable<PurchaseOrder> list = _purchaseOrderList.ToList();
            ViewData["loans"] = (_list.Skip((pageIndex - 1) * _pageSize).Take(_pageSize)).ToList();
            ViewBag.pageIndex = pageIndex;

            return View("~/Views/Loan/LoanSearchView.cshtml");
        }

        public ActionResult LoanUserSearch(FormCollection form)
        {
            var user =  (UserAccount)Session["usuario"];
            ViewData["loans"] = loanApplication.QueryAllMyLoan(user);
            IEnumerable<Loan> _list = (IEnumerable<Loan>)ViewData["loans"];
            int pageIndex;
            int _pageSize = 10;
            int _totalRecords;
            try
            {
                _totalRecords = _list.Count();
            }
            catch (Exception)
            {
                _list = new List<Loan>();
                _totalRecords = 0;
            }

            try
            {
                pageIndex = Convert.ToInt32(form["pageIndex"]);
            }
            catch (Exception)
            {
                pageIndex = 1;
            }
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            //IEnumerable<PurchaseOrder> list = _purchaseOrderList.ToList();
            ViewData["loans"] = (_list.Skip((pageIndex - 1) * _pageSize).Take(_pageSize)).ToList();
            ViewData["all"] = true;
            ViewData["active"] = false;
            ViewData["ended"] = false;
            return View("~/Views/Loan/LoanUserSearchView.cshtml");
        }

        [HttpPost]
        public ActionResult UserSearch(FormCollection form)
        {
            var user =  (UserAccount)Session["usuario"];
            DateTime start = Convert.ToDateTime("1920-01-01"), end = DateTime.Now;
            int user_id = 0, publication_id = 0;
            string status = null;
            bool all = false, active = false, ended = false;
            int pageIndex;

            try
            {
                if (Request["from"] != "") start = Convert.ToDateTime(Request["from"]);
                if (Request["to"] != "") end = Convert.ToDateTime(Request["to"]);
            }
            catch (Exception)
            {
                TempData["alert"] = "Ingrese una fecha válida.";
                return RedirectToAction("LoanSearch", "Loan");
            }

            try
            {
                if (Request["user_id"] != "") user_id = Convert.ToInt32(Request["user_id"]);
            }
            catch (Exception)
            {
                user_id = -1;
            }
            try
            {
                if (Request["publication_id"] != "") publication_id = Convert.ToInt32(Request["publication_id"]);
            }
            catch (Exception)
            {
                publication_id = -1;
            }
            if (Request["estados"] != "Todos") status = Convert.ToString(Request["estados"]);

            SetupSelectedValues(status, ref all, ref active, ref ended);

            ViewData["all"] = all;
            ViewData["active"] = active;
            ViewData["ended"] = ended;

            ViewData["from"] = Request["from"];
            ViewData["to"] = Request["to"];
            ViewData["user_id"] = Request["user_id"];
            ViewData["publication_id"] = Request["publication_id"];

            UserAccount current = new UserAccount();
            current.IdPerfil = 2;

            ViewData["loans"] = loanApplication.SearchLoan(user.Id, publication_id, start, end, status, current);
            IEnumerable<Loan> _list = (IEnumerable<Loan>)ViewData["loans"];
            int _pageSize = 10;
            int _totalRecords;

            try
            {
                _totalRecords = _list.Count();
            }
            catch (Exception)
            {
                _list = new List<Loan>();
                _totalRecords = 0;
            }

            try
            {
                pageIndex = Convert.ToInt32(form["pageIndex"]);
            }
            catch (Exception)
            {
                pageIndex = 1;
            }
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            //IEnumerable<PurchaseOrder> list = _purchaseOrderList.ToList();
            ViewData["loans"] = (_list.Skip((pageIndex - 1) * _pageSize).Take(_pageSize)).ToList();
            ViewBag.pageIndex = pageIndex;

            return View("~/Views/Loan/LoanUserSearchView.cshtml");
        }


        private void   SetupObtainedValues(ref DateTime start, ref DateTime end, ref int user_id,ref int publication_id, ref string status)
        {

            if (Request["from"] != "") start = Convert.ToDateTime(Request["from"]);
            if (Request["to"] != "") end = Convert.ToDateTime(Request["to"]);
            if (Request["user_id"] != "") user_id = Convert.ToInt32(Request["user_id"]);
            if (Request["publication_id"] != "") publication_id = Convert.ToInt32(Request["publication_id"]);
            if (Request["estados"] != "Todos") status = Convert.ToString(Request["estados"]);
        }


        private void SetupSelectedValues(string status,ref bool all,ref bool active,ref bool ended)
        {
            switch (status)
            {
                case "Activo":
                    active = true; ended = false; all= false;
                    break;
                case "Finalizado":
                    active = false; ended = true; all = false;
                    break;
                default:
                    active = false; ended = false; all = true;
                    break;
            }
        }

        [HttpPost]
        public ActionResult LoanDetail()
        {
            var user_current =  (UserAccount)Session["usuario"];
            var items = new List<char>();
            string[] publications = Request.Form.GetValues("publications[]");
            string[] types = Request.Form.GetValues("id_tipos[]");
            int requested_loans_quantity = publications.Length, counter = 0;
            int user_id;
            List<Loan> loanList = new List<Loan>();
            List<int> publicationIdList = new List<int>();

            try
            {
               user_id = Convert.ToInt32(Request["user_id"]);
            }
            catch (Exception)
            {
                TempData["alert"] = "Código de Usuario no válido.";
                return RedirectToAction("LoanRegister", "Loan");
            }

            //Validamos usuario existente
            if (!UserExist(user_id))
            {
                TempData["alert"] = "Usuario No Registrado.";

                return RedirectToAction("LoanRegister", "Loan");
            }
            UserAccount user = new UserAccount();
            user.Id = user_id;
            if (loanApplication.isSanctionUser(user))
            {
                TempData["alert"] = "El usuario no puede reservar items por estar sancionado";

                return RedirectToAction("LoanRegister", "Loan");
            }

            TempData["user_id_reg"] = Convert.ToString(user_id);




            //Validamos Publicacion existente
            foreach (string publication in publications)
            {
                counter++;
                Loan l = new Loan();
                l.Loan_User = user;
                int p_id,t_id;
                PublicationItem selected_item = new PublicationItem();

                try
                {
                    p_id = Convert.ToInt32(publication);
                    t_id = Convert.ToInt32(types[counter - 1]);
                }
                catch (Exception)
                {
                    TempData["alert"] = "Código de Publicación no válido.";
                    return RedirectToAction("LoanRegister", "Loan");
                }


                if (!PublicationExist(p_id))
                {
                    TempData["alert"] = "El Código de la publicación ingresada número " + counter+ " no existe.";

                    return RedirectToAction("LoanRegister", "Loan");
                }
                string temp = Convert.ToString(counter);
                ViewData["temp"] = temp;


                if (IsRepeating(ref publicationIdList, p_id))
                {
                    TempData["alert"] = "El Código de la publicación ingresada número " + counter + " se ingreso más de una vez.";

                    return RedirectToAction("LoanRegister", "Loan");
                }


                //Verificamos perfil por tipo de publicacion
                if (loanApplication.ValidatePublicationTypePerUser(user_id, p_id) == 0)
                {
                    TempData["alert"] = "El usuario no tiene permitido pedir prestado este tipo de publicación.";

                    return RedirectToAction("LoanRegister", "Loan");
                }


                IList<PublicationItem> p = (IList<PublicationItem>)loanApplication.GetItemsWithoutReserve(p_id, user_current);
                Reserve r = new Reserve();
                IList<PublicationItem> p_r = (IList<PublicationItem>)loanApplication.GetItemsWithReserve(user_id, p_id, ref r, user_current);
                int reserve_count;
                try
                {
                    reserve_count = p_r.Count;
                }
                catch (Exception)
                {
                    reserve_count = 0;
                }
                if (reserve_count >= 1)
                {
                    selected_item.Id = p_r[0].Id;
                    PublicationTitle pt = new PublicationTitle();
                    pt.Id = p_r[0].Publication.Id;
                    pt.Title = p_r[0].Publication.Title;
                    selected_item.Publication = pt;
                    l.Loan_Reserve = r;
                    int z = l.Loan_Reserve.Id;
                    l.Loan_Item = selected_item;
                }
                else
                {
                    l.Loan_Reserve = null;
                    int loan_count;
                    try
                    {
                        loan_count = p.Count;
                    }
                    catch (Exception)
                    {
                        loan_count = 0;
                    }
                    if (loan_count == 0)
                    {
                        TempData["alert"] = "No hay items disponibles de la publicación ingresada número " + counter + ".";

                        return RedirectToAction("LoanRegister", "Loan");
                    }
                    else
                    {
                        selected_item.Id = p[0].Id;
                        PublicationTitle pt = new PublicationTitle();
                        pt.Id = p[0].Publication.Id;
                        pt.Title = p[0].Publication.Title;
                        selected_item.Publication = pt;
                        l.Loan_Item = selected_item;
                    }
                }
                l.Status = "Activo";
                LoanType lt = new LoanType();
                lt.Id = t_id;
                l.Loan_Type = lt;
                int day_quantity = loanApplication.GetLoanDays(l.Loan_User.Id);
                l.Register_Date = DateTime.Now;
                l.Start_Date = DateTime.Now;
                l.End_Date = l.Start_Date.AddDays(day_quantity);
                loanList.Add(l);
            }
            //Validamos Cantidad Maxima de prestamos activos
            if (!UserHasLoanSlots(user_id, loanList.Count))
            {
                TempData["alert"] = "El número de préstamos sobrepasa la cantidad permitida para el Usuario.";

                return RedirectToAction("LoanRegister", "Loan");
            }

            ViewData["loan_list"] = loanList;
            loansList = (List<Loan>)ViewData["loan_list"];
            return View("~/Views/Loan/LoanDetailView.cshtml");
        }

        public Boolean UserExist(int user_id)
        {
            var user =  (UserAccount)Session["usuario"];
            int quantity = loanApplication.ValidateUser(user_id);
            if (quantity > 0)
                return true;
            return false;
        }

        public Boolean PublicationExist(int publication_id)
        {
            int quantity = loanApplication.ValidatePublication(publication_id);
            if (quantity > 0)
                return true;
            return false;
        }

        public Boolean UserHasLoanSlots(int user_id,int requested_loans_quantity)
        {
            int quantity = loanApplication.ValidateCantPerUser(user_id);
            if (requested_loans_quantity <= quantity)
                return true;
            return false;
        }

        public Boolean IsRepeating(ref List<int> l, int p_id)
        {
            for (int i = 0; i < l.Count; i++)
                if (l[i] == p_id)
                    return true;
            l.Add(p_id);
            return false;
        }


        public ActionResult LoanList()
        {
            return View("~/Views/Loan/LoanListView.cshtml");
        }

        public ActionResult LoanShow(int id)
        {
            ViewData["loan"] = loanApplication.QueryById(id);
            currentLoan = (Loan)ViewData["loan"];
            return View("~/Views/Loan/LoanShowView.cshtml");
        }

        public ActionResult LoanUserShow(int id)
        {
            var user_current =  (UserAccount)Session["usuario"];
            ViewData["perfil"] = user_current.IdPerfil;
            ViewData["loan"] = loanApplication.QueryById(id);
            currentLoan = (Loan)ViewData["loan"];
            return View("~/Views/Loan/LoanUserShow.cshtml");
        }

        public ActionResult LoanFinished()
        {
            return View("~/Views/Loan/LoanFinishedView.cshtml");
        }

        [HttpPost]
        public ActionResult Save()
        {


            string b = loansList[0].Loan_Item.Publication.Title;
            int a = loansList.Count;

             foreach (Loan l in loansList){
                 loanApplication.Insert(l);
             }

            TempData["message"] = "Se ha registrado el nuevo Préstamo";
            return RedirectToAction("LoanSearch");
        }


        [HttpPost]
        public ActionResult Update(int id)
        {
            Loan l = loanApplication.QueryById(id);

            //Verificamos si esta a tiempo
            TimeSpan difference = l.End_Date - DateTime.Now;
            int days_diff = (int)difference.TotalDays;
            if (days_diff >= 0)
            {
                //Update simple
                loanApplication.returnLoan(l,0,days_diff,l.Loan_User.Id,l.Id);
                TempData["message"] = "Se ha actualizado el Préstamo " + id + " con éxito";
            }
            else
            {
                //Sancion
                loanApplication.returnLoan(l, 1,days_diff,l.Loan_User.Id,l.Id);
                TempData["message"] = "Se ha actualizado el Préstamo " + id + " con éxito y el usuario fue sancionado.";
            }

            return RedirectToAction("LoanSearch");
        }

        public ActionResult Delete()
        {
            TimeSpan difference = currentLoan.End_Date - DateTime.Now;
            int days_diff = (int)difference.TotalDays;

            if (days_diff >= 0)
            {
                //Update simple
                loanApplication.returnLoan(currentLoan, 2, -1, currentLoan.Loan_User.Id, currentLoan.Id);
            }
            else
            {
                loanApplication.returnLoan(currentLoan, 2, days_diff, currentLoan.Loan_User.Id, currentLoan.Id);
            }
            TempData["message"] = "Se ha dado de baja a la Publicación prestada con éxito y el usuario fue sancionado.";
            return RedirectToAction("LoanSearch");
        }

        public ActionResult LoanReport()
        {
            return View("~/Views/Reports/UserLoansReport.cshtml");
        }


        [HttpPost]
        public ActionResult LoanReportResult()
        {
            var user_current =  (UserAccount)Session["usuario"];
            int user_id;
            DateTime start = Convert.ToDateTime("1920-01-01"), end = DateTime.Now;
            List<Loan> loanList = new List<Loan>();
            try
            {
                if (Request["from"] != "") start = Convert.ToDateTime(Request["from"]);
                if (Request["to"] != "") end = Convert.ToDateTime(Request["to"]);
                TempData["from"] = Request["from"];
                TempData["to"] = Request["to"];
            }
            catch (Exception)
            {
                TempData["alert"] = "Ingrese una fecha válida.";
                return RedirectToAction("LoanReport", "Loan");
            }


            TempData["user_idR"] = Request["user_id"];

            try
            {
                user_id = Convert.ToInt32(Request["user_id"]);
            }
            catch (Exception)
            {
                TempData["alert"] = "Código de Usuario no válido.";
                return RedirectToAction("LoanReport", "Loan");
            }

            //Validamos usuario existente
            if (!UserExist(user_id))
            {
                TempData["alert"] = "Usuario No Registrado.";

                return RedirectToAction("LoanReport", "Loan");
            }
            string n = user_current.Name;

            ViewData["user_name"] = loanApplication.GetLoanUser(user_id).Name;
            ViewData["loans"] = loanApplication.ReportLoan(user_id, start, end, user_current);
            loansReportList = (List<Loan>)ViewData["loans"];
            ViewData["Desde"] = start.Date.ToString("d");
            ViewData["Hasta"] = end.Date.ToString("d");
            ViewData["Fecha"] = DateTime.Now.Date.ToString("d");
            ViewData["Hora"] = DateTime.Now.ToString("hh:mm");
            return View("~/Views/Reports/UserLoansReportResult.cshtml");
        }

        [HttpPost]
        public ActionResult ExportToExcel()
        {
            string username = loansReportList[0].Loan_User.Name;
            ExcelPackage pck = new ExcelPackage();
            var wsDt = pck.Workbook.Worksheets.Add("Prestamos");

            wsDt.Cells[3, 4, 3, 7].Value = "Préstamos del usuario " + username;
            wsDt.Cells[3, 4, 3, 7].Merge = true;
            wsDt.Cells[3, 3, 3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Color.SetColor(Color.FromArgb(31, 78, 120));
            wsDt.Cells[3, 3, 3, 6].Style.Font.Bold = true;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Size = 18;
            wsDt.Cells[3, 3, 3, 6].Style.Font.Name = "Times New Roman";

            DataTable dt = new DataTable();
            dt.Columns.Add("Código de Préstamo");
            dt.Columns.Add("Publicación");
            dt.Columns.Add("Cód. Item");
            dt.Columns.Add("Tipo de Préstamo");
            dt.Columns.Add("Fecha Inicio");
            dt.Columns.Add("Fecha Fin");
            dt.Columns.Add("Estado");
            foreach (Loan l in loansReportList)
            {
                dt.Rows.Add(l.Id, l.Loan_Item.Publication.Title, l.Loan_Item.Id, l.Loan_Type.Name, l.Register_Date.ToString("d"), l.End_Date.ToString("d"), l.Status);
            }
            wsDt.Cells[6, 3].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Dark1);
            wsDt.Cells.AutoFitColumns();

            InsertarImagen(wsDt, "~/images/reporte.png");
            wsDt.View.ShowGridLines = false;
            wsDt.View.ZoomScale = 85;

            byte[] vl_bytSalida = pck.GetAsByteArray();
            Response.ClearContent();
            ResponseByte(vl_bytSalida, "application/vnd.ms-excel", "Préstamos_" + username + ".xlsx");
            return Content(Response.ContentType);
            /* var loans = new System.Data.DataTable("Préstamos");
            loans.Columns.Add("Cód. Préstamo", typeof(int));
            loans.Columns.Add("Publicación", typeof(string));
            loans.Columns.Add("Cód. Item", typeof(int));
            loans.Columns.Add("Tipo de Préstamo", typeof(string));
            loans.Columns.Add("Fecha Inicio", typeof(string));
            loans.Columns.Add("Fecha Fin", typeof(string));
            loans.Columns.Add("Estado", typeof(string));

            foreach (Loan l in loansReportList)
            {
                loans.Rows.Add(l.Id, l.Loan_Item.Publication.Title, l.Loan_Item.Id, l.Loan_Type.Name, l.Register_Date.ToString("d"), l.End_Date.ToString("d"),l.Status);
            }
            string username = loansReportList[0].Loan_User.Name;
            var grid = new GridView();
            grid.DataSource = loans;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Préstamos_" + username + ".xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);
            return Content(sw.ToString(), "application/ms-excel");*/
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

	}
}
