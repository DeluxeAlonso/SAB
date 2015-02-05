using SAB.Application.Libray;
using SAB.Application.Publication;
using SAB.Base.Library;
using SAB.Base.Publication;
using SAB.Domain.Library;
using SAB.Domain.Publication;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Publication.Publication
{
    [BasicAuthAttribute]
    public class PublicationController : Controller
    {
        /***************************************************************************************/

        private readonly PublicationTitleApplication _publicationTitleApplication =
            new PublicationTitleApplication(InstanceFactory.Instance.GetInstance<IPublicationTitleRepository>());

        private readonly PublicationItemApplication _publicationItemApplication = 
            new PublicationItemApplication(InstanceFactory.Instance.GetInstance<IPublicationItemRepository>());

        private readonly PublicationTypeApplication _publicationTypeApplication =
            new PublicationTypeApplication(InstanceFactory.Instance.GetInstance<IPublicationTypeRepository>());

        /***************************************************************************************/

        private readonly AuthorApplication _authorApplication =
            new AuthorApplication(InstanceFactory.Instance.GetInstance<IAuthorRepository>());

        private readonly EditorialApplication _editorialApplication =
            new EditorialApplication(InstanceFactory.Instance.GetInstance<IEditorialRepository>());

        private readonly LocalApplication _localApplication = 
            new LocalApplication(InstanceFactory.Instance.GetInstance<ILocalRepository>());

        private readonly TopicApplication _topicApplication =
           new TopicApplication(InstanceFactory.Instance.GetInstance<ITopicRepository>());

        private readonly PublicationTopicApplication _puclicationTopicApplication =
           new PublicationTopicApplication(InstanceFactory.Instance.GetInstance<IPublicationTopicRepository>());

        /***************************************************************************************/

        public ActionResult Index()
        {
            return View();
        }

        /***************************************************************************************/

        public ActionResult PublicationRegister()
        {
            ViewBag.Author = _authorApplication.QueryAll();
            ViewBag.Editorial = _editorialApplication.QueryAll();
            ViewBag.Topic = _topicApplication.QueryAll();

            ViewData["publicationType"] = _publicationTypeApplication.QueryAll();

            return View("~/Views/Publication/Publication/PublicationRegisterView.cshtml");
        }

        /***************************************************************************************/

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(PublicationTitle publicationTitle, HttpPostedFileBase uploadFile, FormCollection form)
        {
            ViewData["publicationType"] = _publicationTypeApplication.QueryAll();

            // Esto ayuda a capturar los temas asociados

            string[] topicIds = form["topicIds"] == null ? null : form["topicIds"].Split(',');
            string[] topicNames = form["topicNames"] == null ? null : form["topicNames"].Split(',');
            string[] topicDescriptions = form["topicDescriptions"] == null ? null : form["topicDescriptions"].Split(',');
            
            if (topicIds != null) ViewData["numTopics"] = topicIds.Length;

            ViewData["topicIds"] = topicIds;
            ViewData["topicNames"] = topicNames;
            ViewData["topicDescriptions"] = topicDescriptions;


            if (ModelState.IsValid)
            {
                // Se valida y guarda la imagen en la carpeta "Fronts" y se accede a el por el nombre generado

                if (uploadFile != null)
                { 
                    string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(uploadFile.FileName);
                    string uploadUrl = Server.MapPath("~/Fronts");

                    uploadFile.SaveAs(Path.Combine(uploadUrl, fileName));
                    publicationTitle.Front = fileName;
                }

                _publicationTitleApplication.Insert(publicationTitle);

                IEnumerable<PublicationTitle> lista = _publicationTitleApplication.QueryAll();

                // Se agregara uno por uno los temas asociados

                int cantidadTemas = topicIds.Length;
                PublicationTopic publicationTopic = new PublicationTopic();
                publicationTopic.IdPublication = lista.Last().Id;

                for (int i = 0; i < topicIds.Length; i++)
                {
                    publicationTopic.IdTopic = Convert.ToInt32(topicIds[i]);
                    _puclicationTopicApplication.Insert(publicationTopic);
                }

                TempData["message"] = "Se ha registrado la nueva Publicacion";
                return RedirectToAction("PublicationSearch");
            }

            return View("~/Views/Publication/Publication/PublicationRegisterView.cshtml", publicationTitle);

        }

        /***************************************************************************************/

        public ActionResult PublicationSearch()
        {
            ViewData["publicationType"] = _publicationTypeApplication.QueryAll();
            ViewBag.pageIndex = 1;
            return View("~/Views/Publication/Publication/PublicationSearchView.cshtml");
        }


        /***************************************************************************************/

         /*
        [HttpPost]
        public ActionResult ModalAuthorSearch()
        {
            int id = 0;
            string name = null, first_last_name = null, country = null;

            SetupObtainedValues(ref id, ref name, ref first_last_name, ref country);

            ViewData["id"] = Request["id"];
            ViewData["name"] = Request["name"];
            ViewData["first_last_name"] = Request["first_last_name"];
            ViewData["country"] = Request["country"];

            //ViewData["authorList"] = _authorApplication.Search(id, name);

            return View("~/Views/Publication/Publication/PublicationRegisterView.cshtml");
        }
        */
        /***************************************************************************************/

        /*
        private void SetupObtainedValues(ref int id, ref string name, ref string first_last_name, ref string country)
        {
            if (Request["id"] != "") id = Convert.ToInt32(Request["id"]);
            if (Request["name"] != "") name = Convert.ToString(Request["name"]);
            if (Request["first_last_name"] != "") first_last_name = Convert.ToString(Request["first_last_name"]);
            if (Request["country"] != "") country = Convert.ToString(Request["country"]);
        }
        */

        /***************************************************************************************/

        [HttpPost]
        public ActionResult PublicationSearchResult()
        {
            string codigo, tipo;
            int id_editorial = 0, id_autor = 0;
            string autor, editorial;
            int id = 0, id_tipo =0;

            codigo = Convert.ToString(Request["Id"]);
            tipo = Convert.ToString(Request["Id_Type"]);
            autor = Convert.ToString(Request["nameAuthor"]);
            editorial = Convert.ToString(Request["nameEditorial"]);

            ViewData["codigo"] = codigo;
            ViewData["nombre"] = tipo;
            ViewData["nameAuthor"] = autor;
            ViewData["nameEditorial"] = editorial;

            if (codigo == null || codigo.Equals(""))
                ViewData["Id"] = codigo;
            else 
            {
                id = Convert.ToInt32(codigo);
                ViewData["Id"] = codigo;
            }

            if (tipo == null || tipo.Equals(""))
                ViewData["Id"] = null;
            else
            {
                id_tipo = Convert.ToInt32(tipo);
                ViewData["Id"] = tipo;
            }

            /*
            if (publicationTitle.nameAuthor != null) publicationTitle.nameAuthor = publicationTitle.nameAuthor.Trim();
            if (publicationTitle.nameEditorial != null) publicationTitle.nameEditorial = publicationTitle.nameEditorial.Trim();
            */
            //IEnumerable<PublicationTitle> resultado = _publicationTitleApplication.Search(publicationTitle);
            IEnumerable<PublicationTitle> resultado = _publicationTitleApplication.Search(id, id_tipo, id_editorial, id_autor, autor, editorial);

            if (resultado == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return PublicationSearch();
            }

            int pageIndex = Int32.Parse(Request["pageIndex"]);

            int _pageSize = 10;
            int _totalRecords = resultado.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            resultado = resultado.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);

            ViewBag.pageIndex = pageIndex;

            ViewData["publicationType"] = _publicationTypeApplication.QueryAll();

            return View("~/Views/Publication/Publication/PublicationSearchResultView.cshtml", resultado);
            
        }

        /***************************************************************************************/

        public ActionResult Detail(int id, int Id_Biblioteca)
        {
            // Esto es para poder jalar los temas asociados

            ViewData["Topics"] = _puclicationTopicApplication.Search(id);

            // Esto es para poder jalar todas las bibliotecas en el combobox
            
            IEnumerable<SAB.Domain.Library.Local> liberyList = _localApplication.QueryAll();
            ViewData["bibliotecas"] = liberyList;        
            ViewData["Id_Biblioteca"] = Request["Id_Biblioteca"];
            
            // Esto crea un diccionatio de la biblioteca que items estan asignados a que bibliotecas

            Dictionary<int, string> biblioItem = new Dictionary<int, string>();

            foreach (var biblioteca in (IEnumerable<Local>)ViewData["bibliotecas"])
            {
                biblioItem.Add(biblioteca.Id, biblioteca.Name);
            }

            ViewData["listaBiblio"] = biblioItem;

            // Se busca la publicacion por el titulo y los items

            PublicationTitle publicatioTitle = _publicationTitleApplication.QueryById(id);

            IEnumerable<PublicationItem> lista = _publicationItemApplication.QueryByPublication(publicatioTitle.Id, Id_Biblioteca); 

            publicatioTitle.ItemList = new List<PublicationItem>();
            publicatioTitle.ItemList = lista;
            ViewData["ItemList"] = lista;

            //*****

            return View("~/Views/Publication/Publication/PublicationDetailView.cshtml", publicatioTitle);
        }

        /***************************************************************************************/
        public ActionResult ItemPublicationSearchByBiblioteca(int idPublication, int idBiblioteca)
        {
            ViewData["idPublication"] = idPublication;
            ViewData["idBiblioteca"] = idBiblioteca;
            return View("~/Views/Publication/Publication/PublicationSearchDetailView.cshtml");
        }

        /***************************************************************************************/

        public ActionResult Modify(int id)
        {
            // Esto es para la busqueda en los modals

            ViewBag.Author = _authorApplication.QueryAll();
            ViewBag.Editorial = _editorialApplication.QueryAll();
            ViewBag.Topic = _topicApplication.QueryAll();

            // Esto es para poder jalar los tipos de publucacion en el combobox

            ViewData["publicationType"] = _publicationTypeApplication.QueryAll();

            // Esto ayuda a capturar los temas asociados

            IEnumerable<SAB.Domain.Publication.Topic> lista = _puclicationTopicApplication.Search(id);

            int num = lista.Count();
            SAB.Domain.Publication.Topic topic = null;
            string[] topicIds = new string[num];
            string[] topicNames = new string[num];
            string[] topicDescriptions = new string[num];

            for (int i = 0; i < num; i++) 
            { 
                topic = lista.ElementAt(i);
                topicIds[i] = Convert.ToString(topic.Id);
                topicNames[i] = topic.Name;
                topicDescriptions[i] = topic.Description;
            }

            if (topicIds != null) ViewData["numTopics"] = num;

            ViewData["topicIds"] = topicIds;
            ViewData["topicNames"] = topicNames;
            ViewData["topicDescriptions"] = topicDescriptions;
            
            // Se manda a la pantalla

            return View("~/Views/Publication/Publication/PublicationModifyView.cshtml", _publicationTitleApplication.QueryById(id));
        }

        /***************************************************************************************/

        [HttpPost]
        public ActionResult Delete(PublicationTitle publicationTitle)
        {
            IEnumerable<PublicationItem> lista = _publicationItemApplication.QueryByState(publicationTitle.Id);

            if (lista.Count() == 0)
            {
                int resultado = _publicationTitleApplication.Delete(publicationTitle);

                if (resultado != 0)
                    TempData["alert"] = "Se ha eliminado la Publicacion " + publicationTitle.Id + " con éxito";
                else
                    TempData["alert"] = "No se ha logrado eliminar la Publicacion " + publicationTitle.Id + " con éxito";
            }
            else
                TempData["alert"] = "No se ha logrado eliminar la Publicacion " + publicationTitle.Id + " con éxito, debido a que hay Items asociados reservados o prestados";
           
            return Json(new { Url = Url.Action("PublicationSearch", "Publication") });
        }

        /***************************************************************************************/
        /*
        public ActionResult imageURL(HttpPostedFileBase uploadFile)
        {
            string urlFile = null;
            if (uploadFile != null)
            {
                //Change a imagen to url
                urlFile = System.IO.Path.GetFileName(uploadFile.FileName);
                string physicalPath = Server.MapPath("~/images/" + urlFile);
                uploadFile.SaveAs(physicalPath);
            }
            return View("~/Views/Publication/Publication/PublicationRegisterView.cshtml");
        }
        */
        /***************************************************************************************/

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(PublicationTitle publicationTitle, HttpPostedFileBase uploadFile, FormCollection form)
        {
            // Esto es para poder jalar los tipos de publucacion en el combobox

            ViewData["publicationType"] = _publicationTypeApplication.QueryAll();

            // Esto ayuda a capturar los temas asociados

            string[] topicIds = form["topicIds"] == null ? null : form["topicIds"].Split(',');
            string[] topicNames = form["topicNames"] == null ? null : form["topicNames"].Split(',');
            string[] topicDescriptions = form["topicDescriptions"] == null ? null : form["topicDescriptions"].Split(',');

            if (topicIds != null) ViewData["numTopics"] = topicIds.Length;

            ViewData["topicIds"] = topicIds;
            ViewData["topicNames"] = topicNames;
            ViewData["topicDescriptions"] = topicDescriptions;

            /********/

            // Cuando hay una imagen asociada se valida, guarda la imagen en la carpeta "Fronts" 
            // y se accede a el por el nombre generado

            if (uploadFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(uploadFile.FileName);
                string uploadUrl = Server.MapPath("~/Fronts");

                uploadFile.SaveAs(Path.Combine(uploadUrl, fileName));
                publicationTitle.Front = fileName;
            }

            // Se modifican los datos simples
            
            _publicationTitleApplication.Update(publicationTitle);


            /********/

            // Se halla la lista original, solo se necesitaran los ids de los temas asociados

            IEnumerable<SAB.Domain.Publication.Topic> lista = _puclicationTopicApplication.Search(publicationTitle.Id);

            int cantOriginal = lista.Count();
            string[] originalTopicIds = new string[cantOriginal];

            if (cantOriginal != 0)
            {
                for (int i = 0; i < cantOriginal; i++)
                {
                    originalTopicIds[i] = Convert.ToString(lista.ElementAt(i).Id);
                }
            }

            // Se eliminan los ids iguales en ambas listas: los temas q aun estan asociados
            // Con ello se consigue q la lista original esten los temas q seran desaciados
            // y en la lista nueva los temas q seran asociados

            int cantNueva = 0;

            if (topicIds != null)
            {
                cantNueva = topicIds.Length;

                for (int i = 0; i < cantOriginal; i++)
                {
                    for (int j = 0; j < cantNueva; j++)
                    {
                        if (originalTopicIds[i].Equals(topicIds[j]))
                        {
                            originalTopicIds[i] = "";
                            topicIds[j] = "";
                        }
                    }
                }
            }

            // Se desacian los temas de la lista original

            PublicationTopic publicationTopicOriginal = new PublicationTopic();
            publicationTopicOriginal.IdPublication = publicationTitle.Id;

            for (int i = 0; i < cantOriginal; i++)
            {
                if (originalTopicIds[i].Equals(""))
                {
                }
                else
                {
                    publicationTopicOriginal.IdTopic = Convert.ToInt32(originalTopicIds[i]);
                    _puclicationTopicApplication.Delete(publicationTopicOriginal);
                }
            }

            // Se asocian los temas de la lista nueva

            PublicationTopic publicationTopicNew = new PublicationTopic();
            publicationTopicNew.IdPublication = publicationTitle.Id;

            for (int i = 0; i < cantNueva; i++)
            {
                if (topicIds[i].Equals(""))
                {
                }
                else
                {
                    publicationTopicNew.IdTopic = Convert.ToInt32(topicIds[i]);
                    _puclicationTopicApplication.Insert(publicationTopicNew);
                }
            }

            /********/

            // Se manda el mensaje de confirmacion 

            TempData["message"] = "Se ha guardado los cambios en la Publicacion " + publicationTitle.Id + " con éxito";
            return RedirectToAction("PublicationSearch");
        }

        /***************************************************************************************/

        public ActionResult AuthorSearch(string codigo, string nombre, int pageIndex = 1)
        {
            int id = 0;
            string name = null;

            if (codigo != null && !codigo.Equals("")) id = Convert.ToInt32(Request["codigo"]);
            if (nombre != null && !nombre.Equals("")) name = nombre;
 
            IEnumerable<SAB.Domain.Publication.Author> _list = _authorApplication.Search(id, name, "");

            int _pageSize = 5;
            int _totalRecords = _list.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;

            _list = _list.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);
            ViewBag.pageIndex = pageIndex;

            return PartialView("~/Views/Publication/Publication/_PartialAuthorTable.cshtml", _list);
        }

        /***************************************************************************************/

        public ActionResult EditorialSearch(string codigo, string razonSocial)
        {
            int id = codigo.Equals("") ? 0 : Convert.ToInt32(Request["codigo"]);
            string name = razonSocial.Equals("") ? null : razonSocial;
            DateTime fechaDesde = Convert.ToDateTime("1920-01-01");
            DateTime fechaHasta =  DateTime.Now;
            return PartialView("~/Views/Publication/Publication/_PartialEditorialTable.cshtml", _editorialApplication.Search(id, razonSocial, fechaDesde, fechaHasta));
        }

        /***************************************************************************************/

        public ActionResult TopicSearch(string codigo, string nombre)
        {
            int id = codigo.Equals("") ? 0 : Convert.ToInt32(Request["codigo"]);
            string name = nombre.Equals("") ? null : nombre;
            return PartialView("~/Views/Publication/Publication/_PartialTopicTable.cshtml", _topicApplication.Search(id, name));
        }

        /***************************************************************************************/
    }
}