using SAB.Application.Publication;
using SAB.Application.Libray;
using SAB.Base.Publication;
using SAB.Domain.Publication;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAB.Base.Library;

namespace SAB.Controllers.Publication.Item_Publication
{
    public class ItemController : Controller
    {
        /***************************************************************************************/

        readonly private PublicationItemApplication _publicationItemApplication = 
            new PublicationItemApplication(InstanceFactory.Instance.GetInstance<IPublicationItemRepository>());
        readonly private LocalApplication _localApplication = 
            new LocalApplication(InstanceFactory.Instance.GetInstance<ILocalRepository>());
        readonly private PublicationTitleApplication _publicationTitleApplication = 
            new PublicationTitleApplication(InstanceFactory.Instance.GetInstance<IPublicationTitleRepository>());

        /***************************************************************************************/

        public ActionResult Index()
        {
            return View();
        }

        /***************************************************************************************/

        public ActionResult ItemRegisterView(int id)
        {
            PublicationTitle publication = _publicationTitleApplication.QueryById(id);

            ViewData["publicationID"] = id;
            ViewData["publication"] = publication.Title;
            ViewData["author"] = publication.nameAuthor;
            ViewData["bibliotecas"] = _localApplication.QueryAll();

            return View("~/Views/Publication/Item-Publication/ItemRegisterView.cshtml");
        }

        /***************************************************************************************/

        [HttpPost]
     /*   public ActionResult Save(PublicationItem publicationItem, int quantity)
        {
            if (ModelState.IsValid)
            {
                if (publicationItem.Estado.Equals("Sin Ubicar")) publicationItem.Id_Activo = -1;
                publicationItem.Id_Recepcion = -1;

                for (int i = 0; i < quantity; i++) 
                {
                    if (publicationItem.Estado.Equals("Sin Ubicar"))
                        _publicationItemApplication.Insert_Donacion_Activo(publicationItem);
                    else
                        _publicationItemApplication.Insert_Donacion(publicationItem);
                }

                TempData["message"] = "Se ha registrado items de la Publicacion ";
            }

            return RedirectToAction("Detail", "Publication", new { id = publicationItem.Id_Publication, Id_Biblioteca = publicationItem.Id_Biblioteca });
        }
            */
        /***************************************************************************************/

        public ActionResult ItemDetailView(int id)
        {
            ViewData["publicationID"] = id;
            PublicationItem item = _publicationItemApplication.QueryById(id);
            ViewData["item"] = item;
            ViewData["publication"] = _publicationTitleApplication.QueryById(item.Id_Publication).Title;
            ViewData["bibliotecas"] = _localApplication.QueryAll();
            ViewData["Estado"] = item.Estado;
            ViewData["biblioteca"] = _localApplication.QueryById(item.Id_Biblioteca).Name;

            return View("~/Views/Publication/Item-Publication/ItemDetailView.cshtml", item);
        }

        /***************************************************************************************/

        public ActionResult ItemModifyView(int id)
        {
            PublicationItem item = _publicationItemApplication.QueryById(id);
            ViewData["biblioteca"] = _localApplication.QueryById(item.Id_Biblioteca).Name;
            ViewData["Estado"] = item.Estado;
            ViewData["bibliotecas"] = _localApplication.QueryAll();
            return View("~/Views/Publication/Item-Publication/ItemModifyView.cshtml", item);
        }

        /***************************************************************************************/

        public ActionResult Save(PublicationItem publicationItem, int quantity)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < quantity; i++) 
                { 
                    if (publicationItem.Estado.Equals("Sin Ubicar"))
                        _publicationItemApplication.Insert_Donacion_Activo(publicationItem);
                    else
                        _publicationItemApplication.Insert_Donacion(publicationItem);
                }
                    //_publicationItemApplication.Insert(publicationItem);
                TempData["message"] = "Se ha registrado items de la Publicacion ";
            }
            return RedirectToAction("Detail", "Publication", new { id = publicationItem.Id_Publication, Id_Biblioteca = publicationItem.Id_Biblioteca });
            
        }


        [HttpPost]

        public ActionResult Update(PublicationItem publicationItem)
        {
            _publicationItemApplication.Update(publicationItem);
            TempData["message"] = "Se ha guardado los cambio del item " + publicationItem.Id + " con éxito";

            return RedirectToAction("Detail", "Publication", new { id = publicationItem.Id_Publication, Id_Biblioteca = publicationItem.Id_Biblioteca });

            //return RedirectToAction("Detail", "Publication", new { id = publicationItem.Id_Publication });

        }

        /***************************************************************************************/

        public ActionResult Delete(PublicationItem publicationItem)
        {
            int id = Convert.ToInt32(Request["Id"]);
            PublicationItem item = _publicationItemApplication.QueryById(id);
            _publicationItemApplication.Delete(publicationItem);

            TempData["alert"] = "Se ha eliminado el item N° " + item.Id + " con éxito";


            return RedirectToAction("Detail", "Publication", new { id = item.Id_Publication, Id_Biblioteca = publicationItem.Id_Biblioteca });

            //return RedirectToAction("Detail", "Publication", new { id = item.Id_Publication });

        }

        /***************************************************************************************/

        public ActionResult ItemLocationView()
        {
            return View("~/Views/Publication/Item-Publication/ItemLocationView.cshtml");
        }

        /***************************************************************************************/

        public ActionResult ItemSearchView()
        {
            return View("~/Views/Publication/Item-Publication/ItemSearchView.cshtml");
        }

        /***************************************************************************************/

        public ActionResult ItemSearchResultView()
        {
            return View("~/Views/Publication/Item-Publication/ItemSearchResultView.cshtml");
        }

        /***************************************************************************************/

        public ActionResult ItemDeleteView()
        {
            return View("~/Views/Publication/Item-Publication/ItemDeleteView.cshtml");
        }
        
        /***************************************************************************************/

    }
}