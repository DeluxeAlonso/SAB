using Microsoft.Practices.EnterpriseLibrary.Data;
using SAB.Base.User;

using SAB.Domain.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Infraestructure.User
{
    public class UserAccountRepository : IUserAccountRepository
    {
        public UserAccount GetUser(string username, string pw)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.[Login_QueryByUsername]", username, pw))
            {
                if (!reader.Read()) return null;
                return new UserAccount
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    Username = Convert.ToString(reader["USERNAME"]),
                    Password = Convert.ToString(reader["PASSWORD"]),
                    IdPerfil = Convert.ToInt32(reader["ID_PERFIL"]),


                    //IdTipoUsuario = Convert.ToInt32(reader["ID_TIPO_USUARIO"]),
                    IdTipoDocumento=Convert.ToInt32(reader["ID_TIPO_DOCUMENTO"]),
                    NumeroDocumento = Convert.ToString(reader["NUMERO_DOCUMENTO"]),
                    IdBiblioteca = Convert.ToInt32(reader["ID_BIBLIOTECA"]),
                   
                    Correo = Convert.ToString(reader["CORREO"]),
                    Lastname1 = Convert.ToString(reader["AP_PATERNO"]),
                    Lastname2=Convert.ToString(reader["AP_MATERNO"]),
                    Name = Convert.ToString(reader["NOMBRE"]),
                    RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]),
                    Status = Convert.ToString(reader["ESTADO"]),
                };


            }
           
        }
        public int Exits(string dni) {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int count = -1;
            using (IDataReader reader = database.ExecuteReader("dbo.[Usuario_Exits]", dni))
            {
                if (!reader.Read()) return -1;
                else count=   Convert.ToInt32(reader["CONTADOR"]);
                return count;


            }
        
        
        }
        public int ExitsCorreo(string s)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int count = -1;
            using (IDataReader reader = database.ExecuteReader("dbo.[Usuario_ExitsCorreo]", s))
            {
                if (!reader.Read()) return -1;
                else count = Convert.ToInt32(reader["CONTADOR"]);
                return count;


            }


        }
        
        public IEnumerable<UserAccount> QueryAll(int p)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
           
           


             using (IDataReader reader = database.ExecuteReader("dbo.Usuario_QueryAll",p))
            {
                 List<UserAccount> usuarios = new List<UserAccount>();
                while (reader.Read())
                {
                  
                     ActionType a = new ActionType();
                        UserAccount u = new UserAccount();
                    u.Id = Convert.ToInt32(reader["ID"]);
                    u.IdPerfil = Convert.ToInt32(reader["ID_PERFIL"]);
                   
                    u.Correo = Convert.ToString(reader["CORREO"]);
                    u.Lastname1 = Convert.ToString(reader["AP_PATERNO"]);
                    u.Lastname2 = Convert.ToString(reader["AP_MATERNO"]);
                    u.IdTipoDocumento = Convert.ToInt32(reader["ID_TIPO_DOCUMENTO"]);
                    u.NumeroDocumento = Convert.ToString(reader["NUMERO_DOCUMENTO"]);
                    u.Name = Convert.ToString(reader["NOMBRE"]);
                    u.RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    u.Status = Convert.ToString(reader["ESTADO"]);
                    usuarios.Add(u);
                    
                }
                return usuarios;
            }
        
        
        }


        public IEnumerable<UserAccount> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");




            using (IDataReader reader = database.ExecuteReader("dbo.Usuario_QueryAll", "Lector"))
            {
                List<UserAccount> usuarios = new List<UserAccount>();
                while (reader.Read())
                {

                    ActionType a = new ActionType();
                    UserAccount u = new UserAccount();
                    u.Id = Convert.ToInt32(reader["ID"]);
                    u.IdPerfil = Convert.ToInt32(reader["ID_PERFIL"]);
                   
                    u.Correo = Convert.ToString(reader["CORREO"]);
                    u.Lastname1 = Convert.ToString(reader["AP_PATERNO"]);
                    u.Lastname2 = Convert.ToString(reader["AP_MATERNO"]);
                    u.IdTipoDocumento=Convert.ToInt32(reader["ID_TIPO_DOCUMENTO"]);
                    u.NumeroDocumento = Convert.ToString(reader["NUMERO_DOCUMENTO"]);
                    u.Name = Convert.ToString(reader["NOMBRE"]);
                    u.RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    u.Status = Convert.ToString(reader["ESTADO"]);
                    usuarios.Add(u);

                }
                return usuarios;
            }


        }
       
        public IEnumerable<UserAccount> Search(int searchCode, string searchDNI, string searchName, string searchApellido, string from, string to, int tipo, string estado,int biblioteca,int tipoUsuario)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            IDataReader reader = null;
           
            if (searchCode != 0 || searchDNI != "")
            {       
                if (searchCode!=0)
                    reader = database.ExecuteReader("dbo.Usuario_Search", searchCode, null, null, null, null, null, tipo, null, biblioteca, tipoUsuario);
                if (searchDNI != "")
                    reader = database.ExecuteReader("dbo.Usuario_Search", searchCode, searchDNI, null, null, null, null, tipo, null, biblioteca, tipoUsuario);
            }

            if (searchCode == 0 && searchDNI == "")
            {
                if (searchName == "" && searchApellido == "" && from == "" && to == ""  && estado == "Todos" && tipo==0 && biblioteca==0) return this.QueryAll(tipoUsuario);
                if (searchName != "" && searchApellido == "" && from == "" && to == "" && estado == "Todos") reader = database.ExecuteReader("dbo.Usuario_Search", searchCode, null, searchName, null, null, null, tipo, null, biblioteca, tipoUsuario);
                if (searchName == "" && searchApellido != "" && from == "" && to == "" && estado == "Todos") reader = database.ExecuteReader("dbo.Usuario_Search", searchCode, null, null, searchApellido, null, null, tipo, null, biblioteca, tipoUsuario);
                if (searchName == "" && searchApellido == "" && from != "" && to == "" && estado == "Todos")
                {
                    DateTime desde = DateTime.ParseExact(from, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    reader = database.ExecuteReader("dbo.Usuario_Search", searchCode, null, null, null, desde, null, tipo, null, biblioteca, tipoUsuario);
                }
                if (searchName == "" && searchApellido == "" && from == "" && to != "" && estado == "Todos")
                {
                    DateTime hasta = DateTime.ParseExact(to, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    reader = database.ExecuteReader("dbo.Usuario_Search", searchCode, null, null, null, null, hasta, tipo, null, biblioteca, tipoUsuario); 
                
                
                }
                if (searchName == "" && searchApellido == "" && from == "" && to == ""  && estado == "Todos" )
                {

                    reader = database.ExecuteReader("dbo.Usuario_Search", searchCode, null, null, null, null, null, tipo, null, biblioteca, tipoUsuario);
                }

                if (searchName == "" && searchApellido == "" && from == "" && to == ""  && estado != "Todos" )
                {

                    reader = database.ExecuteReader("dbo.Usuario_Search", searchCode, null, null, null, null, null, tipo, estado, biblioteca, tipoUsuario);

                }

                if (searchName != "" && searchApellido != "" && from == "" && to == "" && estado == "Todos") reader = database.ExecuteReader("dbo.Usuario_Search", null, null, searchName, searchApellido, null, tipo, null, biblioteca, tipoUsuario);
                if (searchName != "" && searchApellido == "" && from != "" && to == "" && estado == "Todos")
                {
                    DateTime desde = DateTime.ParseExact(from, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    reader = database.ExecuteReader("dbo.Usuario_Search", searchCode, null, searchName, null, desde, null, tipo, null, biblioteca, tipoUsuario);
                }
                if (searchName != "" && searchApellido == "" && from == "" && to != ""  && estado == "Todos" )
                {
                    DateTime hasta = DateTime.ParseExact(to, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    reader = database.ExecuteReader("dbo.Usuario_Search", searchCode, null, searchName, null, null, hasta, tipo, null, biblioteca, tipoUsuario);
                }

                if (searchName == "" && searchApellido == "" && from != "" && to != "" && estado == "Todos")
                {
                    DateTime desde = DateTime.ParseExact(from, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime hasta = DateTime.ParseExact(to, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    reader = database.ExecuteReader("dbo.Usuario_Search", searchCode, null, null, null, desde, hasta, tipo, null, null, tipoUsuario);
                }
              //  if (searchName == "" && searchApellido == "" && from == "" && to == "" && tipo == "" && estado == "") reader = database.ExecuteReader("dbo.Usuario_Search", null, null, null, null, null, null, null, null);
            }
            if (reader == null) return null;

            List<UserAccount> usuarios =null; 
           
                usuarios = new List<UserAccount>(); 
                while (reader.Read())
                {

                    
                    UserAccount u = new UserAccount();
                    u.Id = Convert.ToInt32(reader["ID"]);
                    u.IdPerfil = Convert.ToInt32(reader["ID_PERFIL"]);
                   
                    u.Correo = Convert.ToString(reader["CORREO"]);
                    u.Lastname1 = Convert.ToString(reader["AP_PATERNO"]);
                    u.Lastname2 = Convert.ToString(reader["AP_MATERNO"]);
                    u.Name = Convert.ToString(reader["NOMBRE"]);
                    u.IdTipoDocumento = Convert.ToInt32(reader["ID_TIPO_DOCUMENTO"]);
                    u.NumeroDocumento = Convert.ToString(reader["NUMERO_DOCUMENTO"]);
                    u.RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    u.Status = Convert.ToString(reader["ESTADO"]);
                    usuarios.Add(u);

                }
               
           
            return usuarios;
        }
        public void Insert(UserAccount entity)
        {
             var database = DatabaseFactory.CreateDatabase("SAB");
             if (entity.IdPerfil==2) 
             database.ExecuteNonQuery("dbo.Usuario_Insert",entity.NumeroDocumento,entity.IdTipoDocumento, entity.Password, entity.Name, entity.Lastname1, entity.Lastname2, entity.Telephone, entity.Correo, entity.Address, entity.IdPerfil, 1);
             else database.ExecuteNonQuery("dbo.Usuario_Insert", entity.NumeroDocumento,entity.IdTipoDocumento, entity.Password, entity.Name, entity.Lastname1, entity.Lastname2, entity.Telephone, entity.Correo, entity.Address, entity.IdPerfil, entity.IdBiblioteca);
        }
        public int Delete(UserAccount entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Usuario_Delete", entity.Id);
            return 1;
        }
        public UserAccount QueryById(int id)
        {

            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.[Usuario_QueryById]", id))
            {
                if (!reader.Read()) return null;
                return new UserAccount
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    IdPerfil = Convert.ToInt32(reader["ID_PERFIL"]),
                    IdTipoDocumento = Convert.ToInt32(reader["ID_TIPO_DOCUMENTO"]),
                    NumeroDocumento = Convert.ToString(reader["NUMERO_DOCUMENTO"]),
                   
                    Correo = Convert.ToString(reader["CORREO"]),
                    Lastname1 = Convert.ToString(reader["AP_PATERNO"]),
                    Lastname2 = Convert.ToString(reader["AP_MATERNO"]),
                    Name = Convert.ToString(reader["NOMBRE"]),
                    RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]),
                    Status = Convert.ToString(reader["ESTADO"]),
                    IdBiblioteca = Convert.ToInt32(reader["ID_BIBLIOTECA"]),
                };


            }

 

        }


        public UserAccount QueryByCorreo(string correo)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.[Usuario_QueryByCorreo]", correo))
            {
                if (!reader.Read()) return null;
                return new UserAccount
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    IdPerfil = Convert.ToInt32(reader["ID_PERFIL"]),
                    Password = (Convert.ToString(reader["PASSWORD"])),
                 
                    Correo = Convert.ToString(reader["CORREO"]),
                    Lastname1 = Convert.ToString(reader["AP_PATERNO"]),
                    Lastname2 = Convert.ToString(reader["AP_MATERNO"]),
                    Name = Convert.ToString(reader["NOMBRE"]),
                    IdTipoDocumento = Convert.ToInt32(reader["ID_TIPO_DOCUMENTO"]),
                    NumeroDocumento = Convert.ToString(reader["NUMERO_DOCUMENTO"]),
                    RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]),
                    Status = Convert.ToString(reader["ESTADO"]),
                    IdBiblioteca = Convert.ToInt32(reader["ID_BIBLIOTECA"]),
                };


            }

            
        }


        public UserAccount QueryByDni(string dni)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Usuario_QueryByDni", "77777777"))
            {
                if (!reader.Read()) return null;
                return new UserAccount
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    IdPerfil = Convert.ToInt32(reader["ID_PERFIL"]),
                    Password = (Convert.ToString(reader["PASSWORD"])),
                  
                    Correo = Convert.ToString(reader["CORREO"]),
                    Lastname1 = Convert.ToString(reader["AP_PATERNO"]),
                    Lastname2 = Convert.ToString(reader["AP_MATERNO"]),
                    IdTipoDocumento = Convert.ToInt32(reader["ID_TIPO_DOCUMENTO"]),
                    NumeroDocumento = Convert.ToString(reader["NUMERO_DOCUMENTO"]),
                    Name = Convert.ToString(reader["NOMBRE"]),
                    RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]),
                    Status = Convert.ToString(reader["ESTADO"]),
                    IdBiblioteca = Convert.ToInt32(reader["ID_BIBLIOTECA"]),
                };


            }


        }
        public void Update(UserAccount entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
   
            database.ExecuteNonQuery("dbo.Usuario_Update", entity.Id, entity.Correo, entity.Password, entity.IdPerfil, entity.IdBiblioteca,entity.Name,entity.Lastname1,entity.Lastname2);
       
        }

        public UserAccount QueryByDocumentNumber(string doc_number)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            IDataReader reader = database.ExecuteReader("dbo.Usuario_QueryByDocumentNumber", doc_number);
            UserAccount usuario = new UserAccount();
            using (reader)
            {
                while (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["ID"]);
                    usuario.IdPerfil = Convert.ToInt32(reader["ID_PERFIL"]);
                    usuario.Password = (Convert.ToString(reader["PASSWORD"]));

                    usuario.Correo = Convert.ToString(reader["CORREO"]);
                    usuario.Lastname1 = Convert.ToString(reader["AP_PATERNO"]);
                    usuario.Lastname2 = Convert.ToString(reader["AP_MATERNO"]);
                    usuario.IdTipoDocumento = Convert.ToInt32(reader["ID_TIPO_DOCUMENTO"]);
                    usuario.NumeroDocumento = Convert.ToString(reader["NUMERO_DOCUMENTO"]);
                    usuario.Name = Convert.ToString(reader["NOMBRE"]);
                    usuario.RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    usuario.Status = Convert.ToString(reader["ESTADO"]);
                    usuario.IdBiblioteca = Convert.ToInt32(reader["ID_BIBLIOTECA"]);

                }
            }
            return usuario;
        }
    
    
    }
}
