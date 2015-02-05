using SAB.Application.Acquisition;
using SAB.Application.Publication;
using SAB.Base.Acquisition;
using SAB.Base.Publication;
using SAB.Domain.Acquisition;
using SAB.Domain.Publication;
using SAB.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Adquisiciones.Suscripciones
{
    public class SuscripcionController : Controller
    {
        readonly private SuscriptionApplication _suscriptionApplication =
            new SuscriptionApplication(InstanceFactory.Instance.GetInstance<ISuscriptionRepository>());

        readonly private EditorialApplication _editorialApplication =
            new EditorialApplication(InstanceFactory.Instance.GetInstance<IEditorialRepository>());

        readonly private PublicationTitleApplication _publicationTitleApplication =
            new PublicationTitleApplication(InstanceFactory.Instance.GetInstance<IPublicationTitleRepository>());

        readonly private PublicationTypeApplication _publicationTypeApplication =
            new PublicationTypeApplication(InstanceFactory.Instance.GetInstance<IPublicationTypeRepository>());

        readonly private RenewalApplication _renewalApplication =
            new RenewalApplication(InstanceFactory.Instance.GetInstance<IRenewalRepository>());

        // GET: Suscripcion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterSuscription(FormCollection form)
        {
            ViewData["Publications"] = _publicationTitleApplication.QueryAll();
            ViewData["TipoPublicacion"] = _publicationTypeApplication.QueryAll();
            ViewBag.PublicationTypes = _publicationTypeApplication.QueryAll();
            ViewData["Fecha"] = Convert.ToDateTime(DateTime.Now);
            
            return View("~/Views/Adquisiciones/Suscripciones/SuscriptionRegisterView.cshtml");
        }

        public ActionResult SearchSuscription()
        {
            
            return View("~/Views/Adquisiciones/Suscripciones/SuscriptionSearchView.cshtml");
        }

        public ActionResult SearchSuscriptionResult(Suscription parametros, string tituloPub)
        {
            IEnumerable<SAB.Domain.Publication.PublicationTitle> pubList = _publicationTitleApplication.QueryAll();
            ViewData["publicationList"] = pubList;
            Dictionary<int, string> pubTitle = new Dictionary<int, string>();

            foreach (var titulo in (IEnumerable<PublicationTitle>)ViewData["publicationList"])
            {
                pubTitle.Add(titulo.Id, titulo.Title);
            }
            ViewData["publicacion"] = pubTitle;
            ViewData["suscripciones"] = _suscriptionApplication.QueryAll();
            ViewData["suscriptionID"] = parametros.Id;
            ViewData["suscriptionIDType"] = parametros.Id_TypePublication;
            ViewData["suscriptionState"] = parametros.state;
            ViewData["publicationTitle"] = tituloPub;
            //IEnumerable<Suscription> resultado = _suscriptionApplication.Search(parametros,tituloPub);

            return View("~/Views/Adquisiciones/Suscripciones/SuscriptionSearchResultView.cshtml");
        }

        public ActionResult ModifySuscription()
        {
            return View("~/Views/Adquisiciones/Suscripciones/SuscriptionModifyView.cshtml");
        }

        public ActionResult DeleteSuscription()
        {
            return View("~/Views/Adquisiciones/Suscripciones/SuscriptionDeleteView.cshtml");
        }

        public ActionResult DetailView(int id)
        {
            Suscription suscripcion = _suscriptionApplication.QueryById(id);
            if (suscripcion == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("SearchSuscription", "Suscripcion");
            }
            Editorial editorial = _editorialApplication.QueryById(suscripcion.Id_Editorial);
            ViewData["editorial"] = editorial == null ? null : editorial.Company_Name;
            PublicationTitle publicacion = _publicationTitleApplication.QueryById(suscripcion.Id_Publication);
            ViewData["publicacion"] = publicacion == null ? null : publicacion.Title;
            PublicationType tipo = _publicationTypeApplication.QueryById(suscripcion.Id_TypePublication);
            ViewData["tipo_publicacion"] = tipo == null ? null : tipo.Name;

            /**** Renovaciones ****/
            IEnumerable<Renewal> renovaciones = _renewalApplication.QueryAll(id);
            ViewData["renovaciones"] = renovaciones;
            if (renovaciones == null)
            {
                ViewBag.renovar = false;
                return View("~/Views/Adquisiciones/Suscripciones/SuscriptionDetailView.cshtml", suscripcion);
            }
            IEnumerable<Renewal> renovacionActiva = renovaciones.Where(r => r.State == "Activa");
            if (renovacionActiva.Count() > 0)
            {
                // hay una renovación activa
                Renewal activa = renovacionActiva.ElementAt(0);
                if (DateTime.Today > activa.End_date)
                {
                    // la renovación caducó
                    _renewalApplication.Caducar(activa);
                    activa.State = "Caducada";
                    ViewBag.renovar = true;
                }
                else ViewBag.renovar = false;
            }
            else ViewBag.renovar = true;

            return View("~/Views/Adquisiciones/Suscripciones/SuscriptionDetailView.cshtml", suscripcion);
        }

        [HttpPost]
        public ActionResult Save(Suscription entity, FormCollection form)
        {
            string publicationID = form["publicationID"] == null ? "" : form["publicationID"];
            
            ViewData["publicacion"] = _publicationTitleApplication.QueryById(Convert.ToInt32(publicationID));
            entity.Id_Publication = Convert.ToInt32(publicationID);
            _suscriptionApplication.Insert(entity);
            TempData["message"] = "Se ha registrado una nueva suscripción";
            return RedirectToAction("SearchSuscription", "Suscripcion", new { id = entity.Id });
        }


        [HttpPost]
        public ActionResult Update(int id)
        {
            TempData["message"] = "Se ha guardado los cambio en la suscripción " + id + " con éxito";
            return RedirectToAction("SearchSuscription");
        }


        public ActionResult Delete(int id)
        {
            
            Suscription s = _suscriptionApplication.QueryById(id);
            _suscriptionApplication.Delete(s);

            TempData["alert"] = "Se ha eliminado la suscripción N° " + s.Id + " con éxito";
            return RedirectToAction("SearchSuscription", "Suscripcion", new { id = s.Id });
        }

        public ActionResult Renew(int id)
        {
            ViewData["id_suscripcion"] = id;
            Suscription s = _suscriptionApplication.QueryById(id);
            if (s == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("DetailView", "Suscripcion", new { id = id });
            }
            PublicationTitle p = _publicationTitleApplication.QueryById(s.Id_Publication);
            if (p == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("DetailView", "Suscripcion", new { id = id });
            }
            ViewData["publicacion"] = p.Title;
            return View("~/Views/Adquisiciones/Suscripciones/SuscriptionRenewalView.cshtml");
        }

        [HttpPost]
        public ActionResult Renew(Renewal renewal)
        {
            string costoStr = Request["renewal_cost"];
            costoStr = costoStr.Replace('.', ',');
            double? costo = null;
            if (costoStr != "") costo = double.Parse(costoStr);
            renewal.Cost = costo;
            if (ModelState.IsValid)
            {
                // validar fechas
                if (renewal.Start_date > renewal.End_date)
                {
                    TempData["alert"] = "La fecha de inicio no puede ser mayor a la fecha de fin.";
                }
                else
                {
                    _renewalApplication.Insert(renewal);
                    TempData["message"] = "Se ha renovado la suscripción.";
                    return RedirectToAction("DetailView", "Suscripcion", new { id = renewal.Id_Suscription });
                }          
            }
            ViewData["id_suscripcion"] = Request["id_suscription"];
            ViewData["publicacion"] = Request["publication"];
            ViewData["cantidad"] = Request["amount"];
            ViewData["fecha_inicio"] = Request["start_Date"];
            ViewData["fecha_fin"] = Request["end_Date"];
            ViewData["frecuencia"] = renewal.Frequency;
            ViewData["costo"] = Request["renewal_cost"];
            return View("~/Views/Adquisiciones/Suscripciones/SuscriptionRenewalView.cshtml", renewal);
        }

        public ActionResult ModifyRenewal(int id)
        {
            Renewal renovacion = _renewalApplication.QueryById(id);
            if (renovacion == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("SearchSuscription", "Suscripcion");
            }
            Suscription s = _suscriptionApplication.QueryById(renovacion.Id_Suscription);
            if (s == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("DetailView", "Suscripcion", new { id = renovacion.Id_Suscription });
            }
            PublicationTitle p = _publicationTitleApplication.QueryById(s.Id_Publication);
            if (p == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("DetailView", "Suscripcion", new { id = renovacion.Id_Suscription });
            }
            ViewData["publicacion"] = p.Title;
            ViewData["fecha_inicio"] = renovacion.Start_date.ToShortDateString();
            ViewData["fecha_fin"] = renovacion.End_date.ToShortDateString();
            ViewData["costo"] = renovacion.Cost.ToString().Replace(',', '.');
            return View("~/Views/Adquisiciones/Suscripciones/SuscriptionModifyRenewalView.cshtml", renovacion);
        }

        [HttpPost]
        public ActionResult ModifyRenewal(Renewal renewal)
        {
            string costoStr = Request["renewal_cost"];
            costoStr = costoStr.Replace('.', ',');
            double? costo = null;
            if (costoStr != "") costo = double.Parse(costoStr);
            renewal.Cost = costo;
            if (ModelState.IsValid)
            {
                // validar fechas
                if (renewal.Start_date > renewal.End_date)
                {
                    TempData["alert"] = "La fecha de inicio no puede ser mayor a la fecha de fin.";
                }
                else
                {
                    _renewalApplication.Update(renewal);
                    TempData["message"] = "Se ha modificado la renovación " + renewal.Id + ".";
                    return RedirectToAction("DetailView", "Suscripcion", new { id = renewal.Id_Suscription });
                }           
            }
            ViewData["publicacion"] = Request["publication"];
            ViewData["fecha_inicio"] = Request["start_Date"];
            ViewData["fecha_fin"] = Request["end_Date"];
            ViewData["costo"] = Request["renewal_cost"];
            return View("~/Views/Adquisiciones/Suscripciones/SuscriptionModifyRenewalView.cshtml", renewal);
        }

        public ActionResult DeleteRenewal(int id)
        {
            int id_renovacion = Convert.ToInt32(Request["element_id"]);
            Renewal renewal = new Renewal();
            renewal.Id = id_renovacion;
            _renewalApplication.Delete(renewal);
            TempData["message"] = "Se ha eliminado la renovación " + id_renovacion + " con éxito.";
            return RedirectToAction("DetailView", "Suscripcion", new { id = id });
        }
    }
}