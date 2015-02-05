using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SAB.Application.Reserves;

using SAB.Shared;
using SAB.Base.Reserves;
using SAB.Domain.Publication;
using SAB.Domain.Reserves;
using SAB.Base.Sanctions;
using SAB.Application.LoanApp;
using SAB.Base.LoanInterface;
using SAB.Domain.User;
using SAB.Domain.Assets;
using System.Globalization;

namespace SAB.Controllers.Reserves
{
    [BasicAuthAttribute]
    public class ReservesController : Controller
    {
        private LoanApplication loanApplication = new LoanApplication(InstanceFactory.Instance.GetInstance<ILoanRepository>(), InstanceFactory.Instance.GetInstance<ISanctionRepository>());
        private ReserveAplication reserveAplication = new ReserveAplication(InstanceFactory.Instance.GetInstance<IReserveRepository>(), InstanceFactory.Instance.GetInstance<ISanctionRepository>());
        //
        // GET: /Reserve/
        public ActionResult Publications()
        {
            var user = (UserAccount)Session["usuario"];

            ViewData["reserves"] = reserveAplication.PublicationQueryAll(user.Id);
            
            return View();
        }

        [HttpPost]
        public ActionResult SearchPublications()
        {
            DateTime start, end;
            UserAccount user = (UserAccount)Session["usuario"];
            try
            {
                start = DateTime.ParseExact(Request["from"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                end = DateTime.ParseExact(Request["to"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                start = Convert.ToDateTime("1901-01-01");
                end = Convert.ToDateTime("2099-01-01");
            }

          
            ViewData["from"] = Request["from"];
            ViewData["to"] = Request["to"];
            ViewData["reserves"] = reserveAplication.SearchPublications(user.Id, start, end);

            return View("~/Views/Reserves/Publications.cshtml");
        }

        [HttpPost]
        public ActionResult CancelReserve()
        {

            TempData["message"] = "Se ha anulado la reserva con éxito";
            int reserve_id = Convert.ToInt32(Request["element_id"]);


            if (reserve_id != 0)
            {
                reserveAplication.cancel(reserve_id);
            }

            return RedirectToAction("Publications");
        }

        [HttpPost]
        public ActionResult CancelCubicle()
        {

            TempData["message"] = "Se ha anulado la reserva con éxito";
            int reserve_id = Convert.ToInt32(Request["element_id"]);


            if (reserve_id != 0)
            {
                reserveAplication.cancel(reserve_id);
            }

            return RedirectToAction("Cubicles");
        }

        [HttpPost]
        public ActionResult SearchCubiclesReserves()
        { 
            var user = (UserAccount)Session["usuario"];
           

            DateTime start, end;

            try
            {
                start = DateTime.ParseExact(Request["from"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                end = DateTime.ParseExact(Request["to"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                start = Convert.ToDateTime("1901-01-01");
                end = Convert.ToDateTime("2099-01-01");
            }


            ViewData["from"] = Request["from"];
            ViewData["to"] = Request["to"];

            ViewBag.Reserves =  reserveAplication.SearchReservesCubicles(user.Id, start, end);

            return View("Cubicles");
        
        }
        public ActionResult Cubicles()
        {
            var user = (UserAccount)Session["usuario"];

            ViewBag.Reserves = reserveAplication.CubiclesQueryAll(user.Id);

            return View();
        }

        public ActionResult ReserveCubicleView()
        {
            ViewData["date"] = "";
            ViewData["iniHour"] = 8;
            ViewData["cantHour"] = 1;
            ViewData["quantity"] = -1;
            ViewBag.pageIndex = 1;
            ViewBag.cubicles = new List<Asset>();
            return View();
        }

        [HttpPost]
        public ActionResult SearchCubicles(FormCollection form)
        {
            DateTime start;
            DateTime end;
            var a = form["hour"];
            var user = (UserAccount)Session["usuario"];
            int pageIndex;
            int iniHour = Int32.TryParse(form["hour"], out iniHour) ? Convert.ToInt32(form["hour"]) : 8;
            int cantHours = Int32.TryParse(form["cantHours"], out cantHours) ? Convert.ToInt32(form["cantHours"]) : 1;
            int quantity = Int32.TryParse(form["quantity"], out quantity) ? Convert.ToInt32(form["quantity"]) : -1;
            string datestring = form["from"];

            try
            {
                pageIndex = Convert.ToInt32(form["pageIndex"]);
            }
            catch (Exception)
            {
                pageIndex = 1;
            }

            ViewData["date"] = datestring;
            ViewData["iniHour"] = iniHour;
            ViewData["cantHour"] = cantHours;
            ViewData["quantity"] = quantity;

            if (datestring != "")
            {

                try
                {
                    start = DateTime.ParseExact(datestring, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    start = Convert.ToDateTime("1901-01-01");
                }

                start = start.AddHours(iniHour);
                end = start;
                end = end.AddHours(cantHours);

                if (!reserveAplication.userCanReserveAtThisTime(user, start, end))
                {
                    TempData["alert"] = "El usuario ya ha reservado un cubículo en este periodo de tiempo";
                    ViewBag.cubicles = new List<Asset>();
                    return View("~/Views/Reserves/ReserveCubicleView.cshtml");
                }


                List<Asset> _list =  (List<Asset>)reserveAplication.searchCubicles((DateTime?)start, (DateTime?)end, quantity);
                int _pageSize = 10;
                int _totalRecords = _list.Count();

                ViewBag.CountCubicles = _totalRecords;


                int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
                if (pageIndex < 1) pageIndex = 1;
                if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
      
                ViewBag.pageIndex = pageIndex;
                var cubicles = _list.Skip((pageIndex - 1) * _pageSize).Take(_pageSize);
                ViewBag.cubicles = cubicles;
            }
            else
            {
                ViewBag.cubicles = new List<Asset>();
            }

            
            return View("~/Views/Reserves/ReserveCubicleView.cshtml");
        }

        public ActionResult ReserveCubicle(FormCollection form)
        {
            DateTime start;
            DateTime end;
            int iniHour = Convert.ToInt32(form["hour"]);
            int cantHours = Convert.ToInt32(form["cantHours"]);
            string datestring = form["from"];
            int cubicle_id = Convert.ToInt32(Request["element_id"]);

            var user = (UserAccount)Session["usuario"];

            if (user == null)
            {
                TempData["message"] = "Debe ingresar al sistema para reservar un cubículo";

                return RedirectToAction("Login", "Account");
            }



            if (reserveAplication.isSanctionUser(user))
            {

                TempData["alert"] = "El usuario no puede reservar items por estar sancionado";

                return RedirectToAction("Cubicles");

            }

            

            if (datestring != "")
            {

                try
                {
                    start = DateTime.ParseExact(datestring, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    start = Convert.ToDateTime("1901-01-01");
                }

                start = start.AddHours(iniHour);
                end = start;
                end = end.AddHours(cantHours);

                var reserve = new Reserve();

                reserve.Asset = new Asset() { Id = cubicle_id };
                reserve.StartDate = start;
                reserve.EndDate = end;
                reserve.User = user;


                reserveAplication.reserveCubicle(reserve);

                TempData["message"] = "Se ha reservado el cubículo satisfactoriamente";


            }
            else
            {
                ViewBag.cubicles = new List<Asset>();
            }



            return RedirectToAction("Cubicles");
        }

        public ActionResult ReservePublication(int id)
        {
            var user = (UserAccount)Session["usuario"];
            
            if (user == null)
            {
                TempData["message"] = "Debe ingresar al sistema para reservar un libro";



                return Redirect(Url.Action("Login", "Account", new { ReturnUrl = "/Catalog/Show/" + id }));
            }

            int id_biblioteca = Convert.ToInt32(this.Request.QueryString["id_biblioteca"]); 

            List<PublicationItem> items = (List<PublicationItem>)reserveAplication.getItemsOfPublication(id,id_biblioteca);

            int cantReserves = reserveAplication.getQuantityReservesCanDo(user);


            if (cantReserves < 1)
            {
                TempData["alert"] = "El usuario ya no puede reservar nuevas publicaciones porque ha llegado al límite de reservas permitidas.";

                return RedirectToAction("Show", "Catalog", new { id = id });
            }
            
            if ( reserveAplication.isSanctionUser(user) )
            {
            
                TempData["alert"] = "El usuario no puede reservar items por estar sancionado";

                return RedirectToAction("Show", "Catalog", new { id = id });
            
            }

            if (items == null || items.Count < 1)
            {
                TempData["alert"] = "No hay items disponibles";

                return RedirectToAction("Show", "Catalog", new { id = id });
            }



            int cantDays = reserveAplication.getReserveCantDay(user);
            DateTime from = DateTime.Now;
            DateTime to = from.AddDays(cantDays);


            var  reserve =  new Reserve();
            reserve.Item = new PublicationItem();
            reserve.Item.Id = items[0].Id;
            reserve.StartDate = from;
            reserve.EndDate = to;
            reserve.User = user;

            reserveAplication.makeReserve(reserve);



            TempData["message"] = "Se ha reservado la publicacion satisfactoriamente por " + cantDays + " días";

            return RedirectToAction("Publications");
        }
    }
}