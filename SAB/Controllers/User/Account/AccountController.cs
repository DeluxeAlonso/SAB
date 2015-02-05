
﻿using SAB.Application;
using SAB.Application.User;
﻿using Microsoft.Owin.Security;
using SAB.Base.User;
using SAB.Domain.User;
using SAB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SAB.Application.Politica;
using SAB.Base.Politica;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;
using SAB.Application.Libray;
using SAB.Base.Library;
using System.Text.RegularExpressions;



namespace SAB.Controllers.User.Account
{
    public class AccountController: Controller
    {
        readonly private UserAccountApplication _userAccountApplication =
            new UserAccountApplication(InstanceFactory.Instance.GetInstance<IUserAccountRepository>());
        readonly private DocumentTypeApplication _documentTypeApplication =
           new DocumentTypeApplication(InstanceFactory.Instance.GetInstance<IDocumentTypeRepository>());
        readonly private LocalApplication _localApplication =
           new LocalApplication(InstanceFactory.Instance.GetInstance<ILocalRepository>());
       
        readonly private ActionTypeApplication _actionApplication =
            new ActionTypeApplication(InstanceFactory.Instance.GetInstance<IActionRepository>());

        readonly private UserProfileApplication _userProfileApplication = new UserProfileApplication(InstanceFactory.Instance.GetInstance<IUserProfileRepository>());

        private static string _key = "COMMITERS";

        public static string Encrypt(string strToEncrypt)
         {
             try
             {
                 return Encrypt(strToEncrypt, _key);
             }

             catch (Exception ex)
             {
                 return "Wrong Input. " + ex.Message;
             }
         }
        public static string Decrypt(string strEncrypted)
         {
             try
             {
                 return Decrypt(strEncrypted, _key);
             }
             catch (Exception ex)
             {
                 return "Wrong Input. " + ex.Message;
             }
         }
        public static string Encrypt(string strToEncrypt, string strKey)
         {
             try
             {
                 TripleDESCryptoServiceProvider objDESCrypto =
                     new TripleDESCryptoServiceProvider();
                 MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                 byte[] byteHash, byteBuff;
                 string strTempKey = strKey;
                 byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                 objHashMD5 = null;
                 objDESCrypto.Key = byteHash;
                 objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                 byteBuff = ASCIIEncoding.ASCII.GetBytes(strToEncrypt);
                 return Convert.ToBase64String(objDESCrypto.CreateEncryptor().
                     TransformFinalBlock(byteBuff, 0, byteBuff.Length));
             }
             catch (Exception ex)
             {
                 return "Wrong Input. " + ex.Message;
             }
         }
        public static string Decrypt(string strEncrypted, string strKey)
         {
             try
             {
                 TripleDESCryptoServiceProvider objDESCrypto =
                     new TripleDESCryptoServiceProvider();
                 MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                 byte[] byteHash, byteBuff;
                 string strTempKey = strKey;
                 byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                 objHashMD5 = null;
                 objDESCrypto.Key = byteHash;
                 objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                 byteBuff = Convert.FromBase64String(strEncrypted);
                 string strDecrypted = ASCIIEncoding.ASCII.GetString
                 (objDESCrypto.CreateDecryptor().TransformFinalBlock
                 (byteBuff, 0, byteBuff.Length));
                 objDESCrypto = null;
                 return strDecrypted;
             }
             catch (Exception ex)
             {
                 return "Wrong Input. " + ex.Message;
             }
         }
        public ActionResult ProfileLogin(string username, string password, string returnUrl)
        {
            UserAccount u= new UserAccount();
            if (username == "")
            {
                TempData["message"] = "Ingrese su usuario";
                return RedirectToAction("Login");
            }
            if (password == "")
            {
                TempData["message"] = "Ingrese su contraseña";
                return RedirectToAction("Login");
            }
            u=_userAccountApplication.GetUser(username, Encrypt(password));
            if (u != null) {
                if (_userProfileApplication.QueryById(u.IdPerfil).Estado != "Eliminado")
                {
             
                    Session["usuario"] = u;
                    ((UserAccount)Session["usuario"]).Document = _documentTypeApplication.QueryById(u.IdTipoDocumento);
                    ((UserAccount)Session["usuario"]).TipoPerfil = _userProfileApplication.QueryById(u.Id);
                   IEnumerable<ActionType> acciones = _actionApplication.QueryAllByIdPerfil(u.IdPerfil);
                    List<int> accionesEnInt =new List<int>();
                    foreach(ActionType a in acciones){
                        accionesEnInt.Add(a.Id);
                    
                    }
                    Session["acciones"] = accionesEnInt;


                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return View("~/Views/Home/Index.cshtml");
                }
            }

           
            TempData["message"] = "Usuario y/o Contraseñas invalidos";

            return RedirectToAction("Login");
            

        }
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View("~/Views/Account/Login.cshtml");
        
        }
        public ActionResult Logout()
        {
           Session["usuario"] = null;
            return View("~/Views/Home/Index.cshtml");  

        }
        public ActionResult Update(SAB.Domain.User.UserAccount user) {

            UserAccount usuario = (UserAccount)Session["usuario"];
            usuario.Name = user.Name;
            usuario.Lastname1 = user.Lastname1;
            usuario.Lastname2 = user.Lastname2;
            _userAccountApplication.Update(usuario);
            TempData["message"] = "Se han guardado los cambios con éxito";
            ViewData["user"] = usuario;
            return View("~/Views/User/LibraryUser/Profile.cshtml");
            
           

        }


        public ActionResult UpdateSystemLector(int id,int tipo)
        {


            UserAccount uNuevo = _userAccountApplication.QueryById(id);
            
                uNuevo.IdPerfil = tipo;
                TempData["message"] = "Se ha guardado los cambios del Lector " + id + " con exito";

                _userAccountApplication.Update(uNuevo);
                ViewData["usuarios"] = _userAccountApplication.QueryAll(1);
                ViewData["perfiles"] = _userProfileApplication.QueryAll(1);
                return View("~/Views/User/LibraryUser/Search.cshtml");


        }


        public ActionResult UpdateSystemPersonal(int id, int tipo, int biblioteca)
        {

                UserAccount uNuevo = _userAccountApplication.QueryById(id);
            
            
                uNuevo.IdPerfil = tipo;
                uNuevo.IdBiblioteca = biblioteca;
                _userAccountApplication.Update(uNuevo);
                TempData["message"] = "Se ha guardado los cambios del Empleado " + id + " con exito";



                ViewData["usuarios"] = _userAccountApplication.QueryAll(2);
                ViewData["bibliotecas"] = _localApplication.QueryAll();
                ViewData["perfiles"] = _userProfileApplication.QueryAll(2);
                foreach (UserAccount r in (IEnumerable<UserAccount>)ViewData["usuarios"])
                {
                    r.TipoPerfil = _userProfileApplication.QueryById(r.IdPerfil);
                }
                return View("~/Views/Personal/PersonalSearchResultView.cshtml");



        }
        public ActionResult Modify()
        {

            // Usuario_Update
            ViewData["user"] = (UserAccount)Session["usuario"];
            return View("~/Views/User/LibraryUser/Modify.cshtml");



        }


        public ActionResult CreateLector()
        {
            ViewData["tipodocumentos"] = _documentTypeApplication.QueryAll();
            ViewData["perfiles"] = _userProfileApplication.QueryAll(1);
            return View("~/Views/User/LibraryUser/Create.cshtml");
        }

        public ActionResult CreatePersonal()
        {
            ViewData["tipodocumentos"] = _documentTypeApplication.QueryAll();
            ViewData["bibliotecas"] = _localApplication.QueryAll();
            ViewData["perfiles"] = _userProfileApplication.QueryAll(2);
            return View("~/Views/Personal/PersonalRegisterView.cshtml");
        }
        public ActionResult UserProfile()
        {
            return View("~/Views/User/LibraryUser/Profile.cshtml");
        
        }
        public ActionResult ChangePassword() {

            return View("~/Views/Account/_ChangePasswordPartial.cshtml");
        }

        public ActionResult ForgetPassword() {

            return View("~/Views/Account/LoginForget.cshtml");
        
        
        
        }


        public ActionResult ForgetPasswordCorreo(string correo)
        {
            if (_userAccountApplication.ExitsCorreo(correo) == 1)
            {
                UserAccount u = _userAccountApplication.QueryByCorreo(correo);
                sendMail("SAB - Recuperacion de Contraseña", u.Correo, "Su usuario es " + u.Username + " y su contraseña es " + Decrypt(u.Password) + " Por favor, vuelva a ingresar al sistema y modifique su contraseña por seguridad.");
                ViewData["message"] = "Contraseña enviada";
                return View("~/Views/Home/Index.cshtml");

            }
            else
            {
                TempData["alert"] = "Este correo es invalido";
                return View("~/Views/Account/LoginForget.cshtml");
            }



        }
        public ActionResult Manage(string oldPassword, string newPassword,string confirmPassword)

        {
            UserAccount usuario = (UserAccount)Session["usuario"];
            if (Encrypt(oldPassword) != usuario.Password)
            {
              TempData["alert"] = "Contraseña inválida";
                return View("~/Views/Account/_ChangePasswordPartial.cshtml");
            }
            else {
                if (newPassword.Length>15)
                {
                     TempData["alert"] = "Contraseña maximo 10 caracteres";
                     return View("~/Views/Account/_ChangePasswordPartial.cshtml");
                }
                else{
                    if(newPassword==confirmPassword ){
                    TempData["message"] = "Contraseña cambiada correctamente";
                    UserAccount uNuevo = usuario;
                    uNuevo.Password = Encrypt(newPassword);
                    _userAccountApplication.Update(uNuevo);
                    Session["usuario"] = uNuevo;
                      return View("~/Views/User/LibraryUser/Profile.cshtml");
                
                }
                else{
                TempData["alert"] = "Contraseñas no coinciden";
                return View("~/Views/Account/_ChangePasswordPartial.cshtml");
                }
                }

            }
        
        }
        public ActionResult SaveLector(SAB.Domain.User.UserAccount u,string tipo,string tipoDocumento)
        {
            u.IdTipoDocumento = Convert.ToInt32(tipoDocumento);
            DocumentType documentType = _documentTypeApplication.QueryById(Convert.ToInt32(tipoDocumento));
            u.IdPerfil = Convert.ToInt32(tipo);
            u.IdBiblioteca = 1;
            u.IdTipoUsuario = 1;
            Boolean isCorrect = false;
            long number;
            if (documentType.IsNumerico == 1) isCorrect = Int64.TryParse(u.NumeroDocumento, out number);
            else isCorrect = isAlphaNumeric(u.NumeroDocumento);
           // if (_userAccountApplication.Exits(u.Username) == 1)

            if (isCorrect && u.NumeroDocumento.Length == documentType.DigitQuantity)
            {

                if (_userAccountApplication.Exits(u.NumeroDocumento) == 1 || _userAccountApplication.ExitsCorreo(u.Correo) == 1)
                {
                    ViewData["user"] = u; 
                    TempData["alert"] = "Este Usuario y/o correo ya han sido utilizados";
                    // idTipoUsuario 1 es lector
                    ViewData["perfiles"] = _userProfileApplication.QueryAll(1);
                    return View("~/Views/User/LibraryUser/Create.cshtml");
                }
                else
                {
                    u.Password = Encrypt(u.NumeroDocumento );
                    _userAccountApplication.Insert(u);
                    TempData["message"] = "Ha sido registrado un nuevo usuario";
                    this.sendMail("Registro SAB - Sistema de Administración de Bibliotecas", u.Correo, "Ha sido registrado existosamente, su usuario es " + u.NumeroDocumento + " y su contraseña " + u.NumeroDocumento + " . Si desea, puede ingresar al sistema a cambiar su contraseña. Saludos");
                   /* ViewData["usuarios"] = _userAccountApplication.QueryAll(1);
                    ViewData["perfiles"] = _userProfileApplication.QueryAll(1);
                    return View("~/Views/User/LibraryUser/Search.cshtml");*/
                    return RedirectToAction("SearchLector");

                }
            }
            else {
                ViewData["user"] = u; 
                TempData["alert"] = "Este DNI es invalido";
                ViewData["perfiles"] = _userProfileApplication.QueryAll(1);
                ViewData["tipodocumentos"] = _documentTypeApplication.QueryAll();
                return View("~/Views/User/LibraryUser/Create.cshtml");
            
            
            }
           
           
        }
        public static Boolean isAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\s,]*$");
            return rg.IsMatch(strToCheck);
        }

        public ActionResult SavePersonal(SAB.Domain.User.UserAccount u, string tipo,string biblioteca,string tipoDocumento)
        {



            u.IdPerfil = Convert.ToInt32(tipo);
            u.IdBiblioteca = Convert.ToInt32(biblioteca);
            u.IdTipoUsuario = 2;
            u.IdTipoDocumento = Convert.ToInt32(tipoDocumento);
            DocumentType documentType = _documentTypeApplication.QueryById(Convert.ToInt32(tipoDocumento));
            long number;
            Boolean isCorrect = false;
            if (documentType.IsNumerico == 1) isCorrect = Int64.TryParse(u.NumeroDocumento, out number);
            else isCorrect = isAlphaNumeric(u.NumeroDocumento);
            if (isCorrect && u.NumeroDocumento.Length == documentType.DigitQuantity)
            {

                if (_userAccountApplication.Exits(u.NumeroDocumento) == 1 || _userAccountApplication.ExitsCorreo(u.Correo) == 1)
                {
                    UserAccount userModificado = _userAccountApplication.QueryByDNI(u.NumeroDocumento);
                    userModificado.IdPerfil = Convert.ToInt32(tipo);
                    userModificado.IdBiblioteca = Convert.ToInt32(biblioteca);
                    _userAccountApplication.Update(userModificado);
                    TempData["message"] = "Ha sido registrado un nuevo usuario";
                    this.sendMail("Registro SAB - Sistema de Administración de Bibliotecas", userModificado.Correo, "Ha sido registrado existosamente, su usuario es " + @userModificado.DNI + " y su contraseña " +Decrypt(@userModificado.Password) + " . Si desea, puede ingresar al sistema a cambiar su contraseña. Saludos");
                   /* ViewData["usuarios"] = _userAccountApplication.QueryAll(2);
                    foreach (UserAccount r in (IEnumerable<UserAccount>)ViewData["usuarios"])
                    {
                        r.TipoPerfil = _userProfileApplication.QueryById(r.IdPerfil);
                    } 

                    ViewData["bibliotecas"] = _localApplication.QueryAll();
                    ViewData["perfiles"] = _userProfileApplication.QueryAll(2);
                    return View("~/Views/Personal/PersonalSearchView.cshtml");*/
                    return RedirectToAction("SearchPersonal");
                }
                else
                {
                    u.IdTipoDocumento = Convert.ToInt32(tipoDocumento);
                    u.Password = Encrypt(u.NumeroDocumento);
                    _userAccountApplication.Insert(u);
                    TempData["message"] = "Ha sido registrado un nuevo usuario";
                    this.sendMail("Registro SAB - Sistema de Administración de Bibliotecas", u.Correo, "Ha sido registrado existosamente, su usuario es " + u.NumeroDocumento + " y su contraseña " + u.NumeroDocumento + " . Si desea, puede ingresar al sistema a cambiar su contraseña. Saludos");
                    /*ViewData["usuarios"] = _userAccountApplication.QueryAll(2);
                    foreach (UserAccount r in (IEnumerable<UserAccount>)ViewData["usuarios"]) {
                        r.TipoPerfil = _userProfileApplication.QueryById(r.IdPerfil);
                    }

                    ViewData["bibliotecas"] = _localApplication.QueryAll();
                    ViewData["perfiles"] = _userProfileApplication.QueryAll(2);
                    return View("~/Views/Personal/PersonalSearchView.cshtml");*/
                    return RedirectToAction("SearchPersonal");

                }
            }
            else
            {
                ViewData["usuario"] = u;
                TempData["alert"] = "Este Número de Documento es invalido";
                ViewData["tipodocumentos"] = _documentTypeApplication.QueryAll();
                ViewData["bibliotecas"] = _localApplication.QueryAll();
                ViewData["perfiles"] = _userProfileApplication.QueryAll(2);
                return View("~/Views/Personal/PersonalRegisterView.cshtml");


            }


        }
        private void sendMail(String subject, String email, String body)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("a20105555@pucp.pe", "gaga46557855jm.");


            MailMessage mm = new MailMessage("sistemasdeplanillas@soltec.com", email, subject, "");
            mm.Body = body;


            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            try
            {
                client.Send(mm);
            }
            catch (System.Net.Mail.SmtpException)
            {
               
            }
        }
        public ActionResult SearchLector(int pageIndex=0)
        {
            ViewData["searchCode"] = "";
            ViewData["searchDNI"] = "";
            ViewData["searchName"] = "";
            ViewData["searchApellido"] = "";
            ViewData["tipo"] = 0;
            ViewData["perfiles"] = _userProfileApplication.QueryAll(1);
            IEnumerable <UserAccount>_list = _userAccountApplication.QueryAll(1);
            foreach (UserAccount r in _list)
            {
                r.TipoPerfil = _userProfileApplication.QueryById(r.IdPerfil);
            }    
            
        
            int _pageSize = 10;
            int _totalRecords = _list.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
           
            _list = _list.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);
            ViewData["pageIndex"] = pageIndex;
            ViewData["usuarios"] =_list;
            return View("~/Views/User/LibraryUser/Search.cshtml");
        
        }


        public ActionResult SearchPersonal(int pageIndex = 0)
        {
            ViewData["searchCode"] = "";
            ViewData["searchDNI"] = "";
            ViewData["searchName"] = "";
            ViewData["searchApellido"] = "";
            ViewData["tipo"] = 0;
            ViewData["biblioteca"] = 0;
            IEnumerable<UserAccount> _list = _userAccountApplication.QueryAll(2);
            ViewData["bibliotecas"] = _localApplication.QueryAll();
            ViewData["perfiles"] = _userProfileApplication.QueryAll(2);
            foreach (UserAccount r in _list)
            {
                r.TipoPerfil = _userProfileApplication.QueryById(r.IdPerfil);
            }


            int _pageSize = 10;
            int _totalRecords = _list.Count();
            int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;

            _list = _list.
                Skip((pageIndex - 1) * _pageSize).
                Take(_pageSize);
            ViewData["pageIndex"] = pageIndex;
            ViewData["usuarios"] = _list;
            return View("~/Views/Personal/PersonalSearchResultView.cshtml");

        }
        public ActionResult Delete(int id) {
           
            _userAccountApplication.Delete(id);
            if (_userProfileApplication.QueryById(_userAccountApplication.QueryById(id).IdPerfil).IdTipoUsuario == 1)
            {

                TempData["alert"] = "Se ha eliminado el usuario " + id + " con éxito";
                ViewData["usuarios"] = _userAccountApplication.QueryAll(1);
                return Json(new { Url = Url.Action("SearchLector", "Account") });
            }
            else {
               
                TempData["alert"] = "Se ha eliminado el Empleado " + id + " con éxito";
                ViewData["usuarios"] = _userAccountApplication.QueryAll(2);
                return Json(new { Url = Url.Action("SearchPersonal", "Account") });
            
            }
          

          
        
        
        }


        
        public ActionResult SearchResult(string searchCode, string searchDNI, string searchName,string searchApellido, string from, string to, string tipo, string estado,int pageIndex=0)
        {
            int codigo = Int32.TryParse(searchCode, out codigo) ? Convert.ToInt32(searchCode) : 0;
            ViewData["id"] = Int32.TryParse(searchCode, out codigo) ? searchCode : "";

            ViewData["searchDNI"] = searchDNI;
            ViewData["searchName"] = searchName;
            ViewData["searchApellido"] = searchApellido;
            if (tipo != "") ViewData["tipo"] = Convert.ToInt32(tipo);
            else ViewData["tipo"] = 0;

            
            int tipoPerfil = tipo.Equals("") ? 0 : Convert.ToInt32(tipo);
            IEnumerable<UserAccount> usuarios = (IEnumerable<UserAccount>)_userAccountApplication.Search(codigo, searchDNI, searchName, searchApellido, from, to, tipoPerfil, estado, 0, 1);
            if (usuarios == null)
            {
                ViewData["usuarios"] = null;
                
            }
            else {
                ViewData["usuarios"] = usuarios; 
                foreach (UserAccount usuario in usuarios){
                    usuario.TipoPerfil = _userProfileApplication.QueryById(usuario.IdPerfil);
                    
                }

                int _pageSize = 10;
                int _totalRecords = usuarios.Count();
                int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
                if (pageIndex < 1) pageIndex = 1;
                if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;

                usuarios = usuarios.
                    Skip((pageIndex - 1) * _pageSize).
                    Take(_pageSize);
                ViewData["pageIndex"] = pageIndex;
                ViewData["usuarios"] = usuarios;
            
            
            }
            ViewData["bibliotecas"] = _localApplication.QueryAll();
         
            ViewData["perfiles"] = _userProfileApplication.QueryAll(1);
            return View("~/Views/User/LibraryUser/Search.cshtml");

        }



        public ActionResult SearchResultPersonal(string searchCode, string searchDNI, string searchName, string searchApellido, string from, string to, string tipo, string estado, string biblioteca,int pageIndex=0)
        {
            int codigo = Int32.TryParse(searchCode, out codigo) ? Convert.ToInt32(searchCode) : 0;
            ViewData["searchCode"] = Int32.TryParse(searchCode, out codigo) ? searchCode : "";
            ViewData["searchDNI"] = searchDNI;
            ViewData["searchName"] = searchName;
            ViewData["searchApellido"] = searchApellido;
            if (tipo != "") ViewData["tipo"] = Convert.ToInt32(tipo);
            else ViewData["tipo"] = 0;
            if (biblioteca != "") ViewData["biblioteca"] = Convert.ToInt32(biblioteca);
            else ViewData["biblioteca"] = 0;

            int tipoBiblioteca = biblioteca.Equals("") ? 0 : Convert.ToInt32(biblioteca);
            int tipoPerfil = tipo.Equals("") ? 0 : Convert.ToInt32(tipo);

            IEnumerable<UserAccount> usuarios = (IEnumerable<UserAccount>)_userAccountApplication.Search(codigo, searchDNI, searchName, searchApellido, from, to, tipoPerfil, "Todos", tipoBiblioteca, 2);
            if (usuarios == null)
            {
                ViewData["usuarios"] = null;
             
            }
            else
            {
               
                foreach (UserAccount usuario in usuarios)
                {
                    usuario.TipoPerfil = _userProfileApplication.QueryById(usuario.IdPerfil);
                  

                }

                int _pageSize = 10;
                int _totalRecords = usuarios.Count();
                int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
                if (pageIndex < 1) pageIndex = 1;
                if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;

                usuarios = usuarios.
                    Skip((pageIndex - 1) * _pageSize).
                    Take(_pageSize);
                ViewData["pageIndex"] = pageIndex;
                ViewData["usuarios"] = usuarios;
               

            }

            ViewData["bibliotecas"] = _localApplication.QueryAll();
            ViewData["perfiles"] = _userProfileApplication.QueryAll(2);
            
            return View("~/Views/Personal/PersonalSearchResultView.cshtml");

           

        }
        public ActionResult SearchAccountResultDetail( int id) {
            UserAccount u = _userAccountApplication.QueryById(id);
            u.TipoPerfil = _userProfileApplication.QueryById(u.IdPerfil);
            u.Document = _documentTypeApplication.QueryById(u.IdTipoDocumento);
            ViewData["usuario"] = u;
            
            return View("~/Views/User/LibraryUser/AccountDetail.cshtml");
        
        
        }


        public ActionResult SearchPersonalResultDetail(int id)
        {
            UserAccount u = _userAccountApplication.QueryById(id);
            u.TipoPerfil = _userProfileApplication.QueryById(u.IdPerfil);
            u.Biblioteca = _localApplication.QueryById(u.IdBiblioteca);
            u.Document = _documentTypeApplication.QueryById(u.IdTipoDocumento);
            ViewData["usuario"] = u;
            return View("~/Views/Personal/PersonalDetailView.cshtml");


        }
        public ActionResult ModifyAccount (int id){
            UserAccount u = _userAccountApplication.QueryById(id);
            u.Document = _documentTypeApplication.QueryById(u.IdTipoDocumento);
            u.TipoPerfil = _userProfileApplication.QueryById(u.IdPerfil);
            if (_userProfileApplication.QueryById(u.IdPerfil).IdTipoUsuario == 1)
            {

                ViewData["usuario"] = u;
                ViewData["perfiles"] = _userProfileApplication.QueryAll(1);
                return View("~/Views/User/LibraryUser/AccountDetailModify.cshtml");
            }
            else {
                ViewData["usuario"] = u;
                ViewData["perfiles"] = _userProfileApplication.QueryAll(2);
                ViewData["bibliotecas"] = _localApplication.QueryAll();
               
                return View("~/Views/Personal/PersonalModifyView.cshtml");
            }
        
        }


       

    }
}