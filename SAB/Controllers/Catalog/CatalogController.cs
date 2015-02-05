using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAB.Base.Publication;
using SAB.Application.Catalog;
using SAB.Shared;
using SAB.Domain.Publication;
using SAB.Application.Libray;
using SAB.Base.Library;
using SAB.Domain.Library;
using SAB.Application.Publication;
using System.Text;
using System.Text.RegularExpressions;
using SAB.Application.Reserves;
using SAB.Base.Reserves;
using SAB.Base.Sanctions;



namespace SAB.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CatalogAplication _catalogAplication =
            new CatalogAplication(InstanceFactory.Instance.GetInstance<IPublicationTitleRepository>());

        private readonly LocalApplication _localApplication =
            new LocalApplication(InstanceFactory.Instance.GetInstance<ILocalRepository>());

        private readonly PublicationTitleApplication _publicationApplication =
            new PublicationTitleApplication(InstanceFactory.Instance.GetInstance<IPublicationTitleRepository>());

        private readonly PublicationItemApplication _itemApplication =
            new PublicationItemApplication(InstanceFactory.Instance.GetInstance<IPublicationItemRepository>());

        private readonly PublicationTypeApplication _publicationTypeApplication =
            new PublicationTypeApplication(InstanceFactory.Instance.GetInstance<IPublicationTypeRepository>());

        private ReserveAplication reserveAplication = 
            new ReserveAplication(InstanceFactory.Instance.GetInstance<IReserveRepository>(), InstanceFactory.Instance.GetInstance<ISanctionRepository>());


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FastSearch()
        {
            bibliotecas = new List<Biblioteca>();
            IEnumerable<Local> locales = _localApplication.QueryAll();
            if (locales == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("Index", "Home");
            }
            foreach (Local l in locales)
            {
                bibliotecas.Add(new Biblioteca(l.Id,l.Name));
            }
            ViewBag.local = new SelectList(bibliotecas,"valor","nombre");

            return View("~/Views/Catalog/FastSearch.cshtml");             
        }

        public ActionResult AdvanceSearch()
        {
            bibliotecas = new List<Biblioteca>();
            IEnumerable<Local> locales = _localApplication.QueryAll();
            if (locales == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("Index", "Home");
            }
            foreach (Local l in locales)
            {
                bibliotecas.Add(new Biblioteca(l.Id, l.Name));
            }
            ViewBag.local = new SelectList(bibliotecas,"valor", "nombre");

            tiposPublicacion = new List<TipoPublicacion>();
            IEnumerable<PublicationType> tipos = _publicationTypeApplication.QueryAll();
            if (tipos == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("Index", "Home");
            }
            foreach (PublicationType t in tipos)
            {
                tiposPublicacion.Add(new TipoPublicacion(t.Id, t.Name));
            }
            ViewBag.publicationType = new SelectList(tiposPublicacion, "valor", "nombre");

            return View("~/Views/Catalog/AdvanceSearch.cshtml");     
        }

        [HttpPost]
        public ActionResult FastSearchResult(string searchText, int? local, int searchFor)
        {
            searchText = searchText.Trim();
            if (searchText == "")
            {
                TempData["alert"] = "Introduzca un término de búsqueda.";
                return RedirectToAction("FastSearch");   
            }

            ViewData["searchText"] = searchText;
            ViewData["local"] = local;
            ViewData["searchFor"] = searchFor;

            IEnumerable<PublicationTitle> publicaciones=null;
            List<PublicationTitle> resultado = new List<PublicationTitle>();
            Dictionary<int, int> itemsDisponibles = new Dictionary<int, int>();
            if (searchFor == 0) // búsqueda por todos los criterios
            {
                publicaciones = _publicationApplication.Search2(searchText);
            }
            if (searchFor == 1) // búsqueda por titulo
            {
                publicaciones = _publicationApplication.Search3(null, searchText, null, null, null);
            }
            if (searchFor == 2) // búsqueda por autor
            {
                publicaciones = _publicationApplication.Search3(searchText, null, null, null, null);
            }

            if (publicaciones == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("FastSearch");
            }

            int l;
            l = (local != null) ? local.Value : 0;
            foreach (PublicationTitle p in publicaciones)
            {
                IEnumerable<PublicationItem> items = _itemApplication.QueryByPublication_Biblioteca(p.Id, l);
                if (items.Count() > 0) // sí hay items de la publicacion en esta biblioteca
                {
                    int i_disponibles = items.Count(i => i.Estado.ToLower() == "en estante");
                    itemsDisponibles[p.Id] = i_disponibles;
                    resultado.Add(p);
                }
            }
            ViewBag.cantidades = itemsDisponibles;
            ViewBag.cantResultado = resultado.Count;
            ViewBag.tipoBusqueda = 0;
            int pageIndex;

            try
            {
                pageIndex = Int32.Parse(Request["pageIndex"]);
            }
            catch(Exception)
            {
                pageIndex = 1;
            }
            


            int _pageSize = 10;
            int _totalRecords = resultado.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            //IEnumerable<PurchaseOrder> list = _purchaseOrderList.ToList();
            IEnumerable<PublicationTitle> _list = resultado.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);
            ViewBag.pageIndex = pageIndex;
            return View("~/Views/Catalog/SearchResult.cshtml", _list);
        }

        [HttpPost]
        public ActionResult AdvancedSearchResult(string author, string title, string editorial, string year, int? local,
            int? publicationType, int loanType, int orderBy)
        {
            ViewData["author"] = author;
            ViewData["_title"] = title;
            ViewData["editorial"] = editorial;
            ViewData["year"] = year;
            ViewData["local"] = local;
            ViewData["publicationType"] = publicationType;
            ViewData["loanType"] = loanType;
            ViewData["orderBy"] = orderBy;

            author = author == "" ? null : author.Trim();
            title = title == "" ? null : title.Trim();
            editorial = editorial == "" ? null : editorial.Trim();
            year = year.Trim();
            if (author == null && title == null && editorial == null && year == "" && publicationType == null)
            {
                TempData["alert"] = "Introduzca un término de búsqueda.";
                return RedirectToAction("AdvanceSearch");
            }


            int? anio = null;
            if (year != "")
            {
                try
                {
                    anio = int.Parse(year);
                }
                catch (Exception) 
                {
                    anio = 0;
                }
            }         
            IEnumerable<PublicationTitle> publicaciones = _publicationApplication.Search3(author,title,editorial,anio,publicationType);
            if (publicaciones == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return RedirectToAction("AdvanceSearch");
            }
            List<PublicationTitle> resultado = new List<PublicationTitle>();
            Dictionary<int, int> itemsDisponibles = new Dictionary<int, int>();
            int l;
            l = (local != null) ? local.Value : 0;
            foreach (PublicationTitle p in publicaciones)
            {
                IEnumerable<PublicationItem> items = _itemApplication.QueryByPublication_Biblioteca(p.Id, l);
                if (items.Count() > 0) // sí hay items de la publicacion en esta biblioteca
                {
                    int i_disponibles = items.Count(i => i.Estado.ToLower() == "en estante");
                    itemsDisponibles[p.Id] = i_disponibles;
                    resultado.Add(p);
                }
            }
            ViewBag.cantidades = itemsDisponibles;
            ViewBag.cantResultado = resultado.Count;
            ViewBag.tipoBusqueda = 1;

            int pageIndex = Int32.Parse(Request["pageIndex"]);
            int _pageSize = 10;
            int _totalRecords = resultado.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            //IEnumerable<PurchaseOrder> list = _purchaseOrderList.ToList();
            IEnumerable<PublicationTitle> _list = resultado.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);
            ViewBag.pageIndex = pageIndex;
            return View("~/Views/Catalog/SearchResult.cshtml", _list);             
        }

        public ActionResult Show(int id)
        {
            List<string> tags;
            PublicationTitle  publication = _catalogAplication.find(id);
            var items = reserveAplication.getItemsOfPublication(id);

            try
            {
                tags = reserveAplication.getTags(id);
            }
            catch (Exception ex)
            {
                tags = new List<string>();
            }


            ViewBag.tags = tags;

            ViewData["Items"] = items;
            ViewData["Disponibles"] = items.Count();


            return View("~/Views/Catalog/PublicationShow.cshtml", publication);
        }

        private List<Biblioteca> bibliotecas;
        private class Biblioteca
        {
            public int valor { get; set; }
            public string nombre { get; set; }
            public Biblioteca(int id, string nombre)
            {
                this.valor = id;
                this.nombre = nombre;
            }
        }

        private List<TipoPublicacion> tiposPublicacion;
        private class TipoPublicacion
        {
            public int valor { get; set; }
            public string nombre { get; set; }
            public TipoPublicacion(int id, string nombre)
            {
                this.valor = id;
                this.nombre = nombre;
            }
        }
    }
}