using SAB.Application.Politica;
using SAB.Application.Publication;
using SAB.Application.User;
using SAB.Base.Politica;
using SAB.Base.Publication;
using SAB.Base.User;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Controllers.Politica.UserProfile
{
    public class UserProfileController : Controller
    {
        // GET: UserProfile

        readonly private ActionTypeApplication _actionApplication =
           new ActionTypeApplication(InstanceFactory.Instance.GetInstance<IActionRepository>());
        readonly private PublicationTypeApplication _publicationTypeApplication = new PublicationTypeApplication(InstanceFactory.Instance.GetInstance<IPublicationTypeRepository>());
        readonly private UserProfileApplication _userProfileApplication = new UserProfileApplication(InstanceFactory.Instance.GetInstance<IUserProfileRepository>());
        readonly private DevolutionProfileApplication _devolutionProfileApplication = new DevolutionProfileApplication(InstanceFactory.Instance.GetInstance<IDevolutionProfileRepository>());
        
        public ActionResult Index()
        {
            return View();
        }
        /*public ActionResult UserProfileRegister()

        {
            ViewData["acciones"] = _actionApplication.QueryAll();
            ViewData["publicaciones"] = _publicationTypeApplication.QueryAll();
            return View("~/Views/Politica/UserProfile/UserProfileRegisterView.cshtml");
        }*/



        public ActionResult UserProfileSearch()
        {
            ViewData["searchCode"] = "";
            ViewData["searchName"] = "";
            ViewData["ActionSelected"] = 0;
            ViewData["PublicactionSelected"] = 0;
            ViewData["acciones"] = _actionApplication.QueryAll();;
            ViewData["publicaciones"] = _publicationTypeApplication.QueryAll();
            ViewData["allprofiles"] = _userProfileApplication.QueryAll();
            return View("~/Views/Politica/UserProfile/UserProfileSearchView.cshtml");
        }


        public ActionResult UserProfileDetail(int id)
            
        {
            ViewData["userProfile"] = _userProfileApplication.QueryById(id);
            ViewData["accionById"] = _actionApplication.QueryAllByIdPerfil(id);
            ViewData["publicacionById"] = _publicationTypeApplication.QueryAllByIdPerfil(id);
            ViewData["devolutionById"] = _devolutionProfileApplication.QueryByIdPerfil(id);
            return View("~/Views/Politica/UserProfile/UserProfileDetailView.cshtml");
        }


        public ActionResult UserProfileList(string ActionSelected, string PublicactionSelected, string searchCode, string searchName)
        {
            if (searchCode != "") ViewData["searchCode"] = Convert.ToInt32(searchCode);
            else ViewData["searchCode"] = "";
            ViewData["searchName"] = searchName;
            if (ActionSelected != "") ViewData["ActionSelected"] = Convert.ToInt32(ActionSelected);
            else ViewData["ActionSelected"] = 0;
            if (PublicactionSelected != "") ViewData["PublicactionSelected"] = Convert.ToInt32(PublicactionSelected);
            else ViewData["PublicactionSelected"] = 0;
            
            
            ViewData["acciones"] = _actionApplication.QueryAll();
            ViewData["publicaciones"] = _publicationTypeApplication.QueryAll();
            ViewData["allprofiles"] = _userProfileApplication.Search(ActionSelected, PublicactionSelected, searchCode, searchName);
            if (ViewData["allprofiles"] == null) TempData["alert"] = "No hay resultados";
            return View("~/Views/Politica/UserProfile/UserProfileSearchView.cshtml");
        }
        public ActionResult UserProfileModify(int id)

        {
            ViewData["acciones"] = _actionApplication.QueryAll();
            ViewData["publicaciones"] = _publicationTypeApplication.QueryAll();
            ViewData["userProfile"] = _userProfileApplication.QueryById(id);
            ViewData["accionById"] = _actionApplication.QueryAllByIdPerfil(id);
            ViewData["publicacionById"] = _publicationTypeApplication.QueryAllByIdPerfil(id);
            ViewData["devolutionById"] = _devolutionProfileApplication.QueryByIdPerfil(id);
            return View("~/Views/Politica/UserProfile/UserProfileModifyView.cshtml");
        }
        public ActionResult UserProfileRegister()

        {
            ViewData["acciones"] = _actionApplication.QueryAll();
            ViewData["publicaciones"] = _publicationTypeApplication.QueryAll();
            return View("~/Views/Politica/UserProfile/UserProfileRegisterView.cshtml");
        }

      
     
        [HttpPost]
        public ActionResult Save(int[] selectedPublication, int[] selectedAction, SAB.Domain.Politica.UserProfile userProfile, string[] descriptions, int[] days, string[] fechaHasta, string[] fechaDesde, int tipoUSuario)
        {
           
                if (selectedAction != null && selectedPublication != null)
                {
                    if (descriptions == null && days == null && fechaDesde == null && fechaHasta == null)
                    {
                        userProfile.IdTipoUsuario = tipoUSuario;
                        _userProfileApplication.Insert(userProfile);
                       

                        _userProfileApplication.InsertPrivilegios(userProfile.Name,selectedAction );
                        _userProfileApplication.InsertPublication(userProfile.Name, selectedPublication);

                        TempData["message"] = "Se ha registrado el nuevo Tipo de Perfil";
                        return RedirectToAction("UserProfileSearch");
                    }
                    else
                    {
                        if (descriptions.Length == days.Length && days.Length == fechaHasta.Length && fechaHasta.Length == fechaDesde.Length)
                        {
                            userProfile.IdTipoUsuario = tipoUSuario;
                            _userProfileApplication.Insert(userProfile);
                            _userProfileApplication.InsertDevolucion(userProfile.Name, descriptions, days, fechaHasta, fechaDesde);
                            _userProfileApplication.InsertPrivilegios(userProfile.Name, selectedAction);
                            _userProfileApplication.InsertPublication(userProfile.Name, selectedPublication);

                            TempData["message"] = "Se ha registrado el nuevo Tipo de Perfil";
                            return RedirectToAction("UserProfileSearch");
                        }
                        else
                        {
                            TempData["alertDevolucion"] = "Ingrese una fila completa";
                            
                            ViewData["acciones"] = _actionApplication.QueryAll();
                            ViewData["publicaciones"] = _publicationTypeApplication.QueryAll();
                            return View("~/Views/Politica/UserProfile/UserProfileRegisterView.cshtml", userProfile);

                        }
                    }

                }
                else {
                    if (descriptions == null) TempData["alertDevolucion"] = "Ingrese una fila completa";
                    if (selectedAction == null) TempData["alertAction"] = "Seleccione por lo menos una accion";
                    if (selectedPublication == null) TempData["alertPublication"] = "Seleccione por lo menos un tipo de publicación";
                    ViewData["acciones"] = _actionApplication.QueryAll();
                    ViewData["publicaciones"] = _publicationTypeApplication.QueryAll();
        
                    return View("~/Views/Politica/UserProfile/UserProfileRegisterView.cshtml", userProfile);
                
                
                }
                
            
      
           
        }

        

        [HttpPost]
        public ActionResult Update(int id, string name,int maxMaterial,int day, string description,int[] selectedPublication, int[] selectedAction, string[] descriptions,int[] days,string[] fechaHasta, string[] fechaDesde)
        {
            
            _userProfileApplication.Actualiza(id, name, maxMaterial, day, description);
            _publicationTypeApplication.DeleteAllByIdPerfil(id);
            _actionApplication.DeleteAllByIdPerfil(id);


            _userProfileApplication.InsertPrivilegios(name, selectedAction);
            _userProfileApplication.InsertPublication(name,selectedPublication );


            _userProfileApplication.InsertDevolucion(name, descriptions, days, fechaHasta, fechaDesde);        

            TempData["message"] = "Se han guardado los cambios en el perfil de usuario " + id + " con éxito";

            return RedirectToAction("UserProfileSearch");
        }


        public ActionResult Delete(int id)
        {
            TempData["alert"] = "Se ha eliminado el perfil de usuario " + id + " con éxito";
            _userProfileApplication.Delete(id);


            return Json(new { Url = Url.Action("UserProfileSearch", "UserProfile") });
        }

    }
}