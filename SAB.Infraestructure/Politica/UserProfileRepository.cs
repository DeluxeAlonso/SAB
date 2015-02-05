using Microsoft.Practices.EnterpriseLibrary.Data;
using SAB.Base.Politica;
using SAB.Domain.Politica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Infraestructure.Politica
{
    public class UserProfileRepository : IUserProfileRepository
    {
        public void InsertDevolucionPerfil(string description, DateTime fechaDesde, DateTime fechaHasta, int cantDias, string namePerfil)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.UserProfile_InsertDevolucion", description, fechaDesde.ToString("yyyy-MM-dd"), fechaHasta.ToString("yyyy-MM-dd"), cantDias, namePerfil);
        
        }

        public void Actualiza(int id,string name, int maxMaterial,int  day, string description){
        
               var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.UserProfile_Update",id ,name, description,  day,maxMaterial);
       
        
        }

        public UserProfile QueryById(int id) {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.UserProfile_QueryById", id))
            {

                if (!reader.Read()) return null;
                return new UserProfile
                {
                    Id=Convert.ToInt32(reader["ID"]),
                    Name = Convert.ToString(reader["NOMBRE"]),
                    Description = Convert.ToString(reader["DESCRIPCION"]),
                    MaxDays = Convert.ToInt32(reader["CANTIDAD_DIAS"]),
                    MaxMaterial = Convert.ToInt32(reader["CANTIDAD_MAXIMA_MATERIAL"]),
                    Estado = Convert.ToString(reader["ESTADO"]),
                    IdTipoUsuario=Convert.ToInt32(reader["ID_TIPO_USUARIO"]),
                };


            }
        
        
        
        
        
        }
        public IEnumerable<UserProfile> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var profiles = new List<UserProfile>();

            using (IDataReader reader = database.ExecuteReader("dbo.UserProfile_QueryAll"))
            {
                while (reader.Read())
                {
                    UserProfile u = new UserProfile();
                    u.Id=Convert.ToInt32(reader["ID"]);
                    u.Name = Convert.ToString(reader["NOMBRE"]);
                    u.Description = Convert.ToString(reader["DESCRIPCION"]);
                    u.MaxDays = Convert.ToInt32(reader["CANTIDAD_DIAS"]);
                    u.MaxMaterial = Convert.ToInt32(reader["CANTIDAD_MAXIMA_MATERIAL"]);
                    u.Estado = Convert.ToString(reader["ESTADO"]);
                    u.IdTipoUsuario = Convert.ToInt32(reader["ID_TIPO_USUARIO"]);
                    profiles.Add(u);

                }
            }
            return profiles;
        }

        public IEnumerable<UserProfile> QueryAll(int param)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var profiles = new List<UserProfile>();

            using (IDataReader reader = database.ExecuteReader("dbo.UserProfile_QueryAllByTipoUsuario", param))
            {
                while (reader.Read())
                {
                    UserProfile u = new UserProfile();
                    u.Id = Convert.ToInt32(reader["ID"]);
                    u.Name = Convert.ToString(reader["NOMBRE"]);
                    u.Description = Convert.ToString(reader["DESCRIPCION"]);
                    u.MaxDays = Convert.ToInt32(reader["CANTIDAD_DIAS"]);
                    u.MaxMaterial = Convert.ToInt32(reader["CANTIDAD_MAXIMA_MATERIAL"]);
                    u.Estado = Convert.ToString(reader["ESTADO"]);
                    u.IdTipoUsuario = Convert.ToInt32(reader["ID_TIPO_USUARIO"]);
                    profiles.Add(u);

                }
            }
            return profiles;
        }

        


        public IEnumerable<UserProfile> Search(string ActionSelected, string PublicactionSelected, string searchCode, string searchName)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var profiles = new List<UserProfile>();

            IDataReader reader=null; 
            if (searchCode != "") reader = database.ExecuteReader("dbo.UserProfile_Search", Int32.Parse(searchCode), null, null, null);
            else {

                if (PublicactionSelected == "0" && ActionSelected == "0" && searchName == " ") return this.QueryAll();
                if (PublicactionSelected != "0" && ActionSelected == "0" && searchName == " ") reader = database.ExecuteReader("dbo.UserProfile_Search", null, null, null, Int32.Parse(PublicactionSelected));
                if (PublicactionSelected == "0" && ActionSelected != "0" && searchName == " ") reader = database.ExecuteReader("dbo.UserProfile_Search", null, null, Int32.Parse(ActionSelected), null);
                if (PublicactionSelected == "0" && ActionSelected == "0" && searchName != " ") reader = database.ExecuteReader("dbo.UserProfile_Search", null, searchName, null, null);
                if (PublicactionSelected != "0" && ActionSelected != "0" && searchName == " ") reader = database.ExecuteReader("dbo.UserProfile_Search", null, null, Int32.Parse(ActionSelected), Int32.Parse(PublicactionSelected));
                if (PublicactionSelected != "0" && ActionSelected != "0" && searchName != " ") reader = database.ExecuteReader("dbo.UserProfile_Search", null, searchName, Int32.Parse(ActionSelected), Int32.Parse(PublicactionSelected));
                if (PublicactionSelected == "0" && ActionSelected != "0" && searchName != " ") reader = database.ExecuteReader("dbo.UserProfile_Search", null, searchName, Int32.Parse(ActionSelected), null);
            }
            if (reader==null) return null;
            using (reader)
            {
                while (reader.Read())
                {
                    UserProfile u = new UserProfile();
                    u.Id = Convert.ToInt32(reader["ID"]);
                    u.Name = Convert.ToString(reader["NOMBRE"]);
                    u.Description = Convert.ToString(reader["DESCRIPCION"]);
                    u.MaxDays = Convert.ToInt32(reader["CANTIDAD_DIAS"]);
                    u.MaxMaterial = Convert.ToInt32(reader["CANTIDAD_MAXIMA_MATERIAL"]);
                    u.Estado = Convert.ToString(reader["ESTADO"]);
                    u.IdTipoUsuario = Convert.ToInt32(reader["ID_TIPO_USUARIO"]);
                    profiles.Add(u);

                }
            }
            return profiles;
        }


        public void Insert(UserProfile entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.UserProfile_Insert", entity.Name, entity.Description,
                entity.MaxDays,entity.MaxMaterial,entity.IdTipoUsuario);

        }


        public void InsertPrivilegios(string nameUseProfile,int idAccion)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.UserProfile_InsertPrivilegio", nameUseProfile, idAccion);

        }

        public void InsertPublicaciones(string nameUseProfile, int idTipoPublicacion)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.UserProfile_InsertPublicacion", nameUseProfile, idTipoPublicacion);

        }

        public int Delete(UserProfile entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.UserProfile_Delete", entity.Id);
            return 1;
        }

       

        public void Update(UserProfile entity)
        {
            throw new NotImplementedException();
        }

    }
}
