using SAB.Application.Publication;
using SAB.Base.Publication;
using SAB.Domain.Publication;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Publication.Topic
{
    [BasicAuthAttribute]
    public class TopicController : Controller
    {
        /***************************************************************************************/

        private readonly TopicApplication _topicApplication =
            new TopicApplication(InstanceFactory.Instance.GetInstance<ITopicRepository>());

        private readonly PublicationTitleApplication _publicationTitleApplication =
            new PublicationTitleApplication(InstanceFactory.Instance.GetInstance<IPublicationTitleRepository>());

        private readonly PublicationItemApplication _publicationItemApplication =
            new PublicationItemApplication(InstanceFactory.Instance.GetInstance<IPublicationItemRepository>());

        /***************************************************************************************/

        public ActionResult Index()
        {
            return View();
        }

        /***************************************************************************************/

        public ActionResult TopicRegister()
        {
            return View("~/Views/Publication/Topic/TopicRegisterView.cshtml");
        }

        /***************************************************************************************/

        public ActionResult Save(SAB.Domain.Publication.Topic topic)
        {
            _topicApplication.Insert(topic);
            TempData["message"] = "Se ha registrado un nuevo Tema";

            return RedirectToAction("TopicSearch");
        }

        /***************************************************************************************/

        public ActionResult TopicSearch()
        {
            return View("~/Views/Publication/Topic/TopicSearchView.cshtml");
        }

        /***************************************************************************************/

        public ActionResult TopicSearchResult()
        {
            string codigo, nombre;

            codigo = Convert.ToString(Request["codigo"]);
            nombre = Convert.ToString(Request["nombre"]);

            int id = codigo.Equals("") ? 0 : Convert.ToInt32(Request["codigo"]);
            string name = nombre.Equals("") ? null : nombre;

            ViewData["codigo"] = codigo;
            ViewData["nombre"] = nombre;

            IEnumerable<SAB.Domain.Publication.Topic> topicList = _topicApplication.Search(id, name);

            int pageIndex = Int32.Parse(Request["pageIndex"]);

            int _pageSize = 10;
            int _totalRecords = topicList.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
            topicList = topicList.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);

            ViewBag.pageIndex = pageIndex;

            return View("~/Views/Publication/Topic/TopicSearchResultView.cshtml", topicList);
        }

        /***************************************************************************************/

        public ActionResult TopicDetail(int id)
        {
            SAB.Domain.Publication.Topic topic = _topicApplication.QueryById(id);
            if (topic == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return TopicSearch();
            }
            return View("~/Views/Publication/Topic/TopicDetailView.cshtml", topic);
        }

        /***************************************************************************************/

        public ActionResult TopicModify(int id)
        {
            SAB.Domain.Publication.Topic topic = _topicApplication.QueryById(id);
            if (topic == null)
            {
                TempData["alert"] = "Ha ocurrido un error. Intente de nuevo.";
                return TopicSearch();
            }
            return View("~/Views/Publication/Topic/TopicModifyView.cshtml", topic);
        }

        /***************************************************************************************/

        [HttpPost]
        public ActionResult Update(SAB.Domain.Publication.Topic topic)
        {
            _topicApplication.Update(topic);
            TempData["message"] = "Se ha guardado los cambios en la Publicacion " + topic.Id + " con éxito";
            return RedirectToAction("TopicSearch");
        }

        /***************************************************************************************/

        [HttpPost]
        public ActionResult Delete(SAB.Domain.Publication.Topic topic)
        {
            // Para realizar una eliminacion en cascada 
            // Primeto se debe buscar las publicaciones relacionadas con el tema
            // Segundo se debe verificar que estas no tengas items prestados ni reservados
            // Tercero se procede a la eliminacion de todo lo anterior encontrado

            IEnumerable<PublicationTitle> listaPublication = _publicationTitleApplication.QueryAll();
            int resultado = 0;

            if (listaPublication.Count() == 0)
            {
                for (int i = 0; i < listaPublication.Count(); i++)
                {
                    IEnumerable<PublicationItem> listaItem = _publicationItemApplication.QueryAll();

                    if (listaPublication.Count() == 0)
                    {
                        // Eliminar los items y publicacion.......
                        resultado = _topicApplication.Delete(topic);
                        TempData["alert"] = "Se ha eliminado el Tema " + topic.Name + " con éxito";
                    }
                    else
                    {
                        TempData["alert"] = "No se ha logrado eliminar el Tema " + topic.Name + " con éxito, ya que la publicacion "
                           + listaPublication.ElementAt(i).Title + " tiene items prestados o reservados";
                    }
                }
            }
            else 
            {
                // No hay ninguna publicacion asociada al tema
                resultado = _topicApplication.Delete(topic);
                TempData["alert"] = "Se ha eliminado el Tema " + topic.Name + " con éxito";
            }

            return Json(new { Url = Url.Action("PublicationSearch", "Publication") });
        }

        /***************************************************************************************/

    }
}