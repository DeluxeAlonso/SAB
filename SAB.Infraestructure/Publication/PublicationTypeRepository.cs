using SAB.Base.Publication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
namespace SAB.Infraestructure.Publication
{
    public class PublicationTypeRepository : IPublicationTypeRepository
    {
        public List<PublicationType> queryAllByIdPerfil(int idPerfil) {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.TipoPublicacion_QueryAllByIdPerfil",idPerfil))
            {
                List<PublicationType> list = new List<PublicationType>();
                while (reader.Read())
                {
                    PublicationType u =new PublicationType();
                    
                        u.Id = Convert.ToInt32(reader["ID"]);
                        u.Name = Convert.ToString(reader["NOMBRE"]);
                        u.Description = Convert.ToString(reader["DESCRIPCION"]);

                        list.Add(u);

                }
                return list;
            }
        
        
        }
        public IEnumerable<PublicationType> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.TipoPublicacion_QueryAll"))
            {
                List<PublicationType> lista = new List<PublicationType>();
                while (reader.Read())
                {
                    PublicationType t = new PublicationType();
                    t.Id = Convert.ToInt32(reader["ID"]);
                    t.Name = Convert.ToString(reader["NOMBRE"]);
                    t.Description = Convert.ToString(reader["DESCRIPCION"]);
                    lista.Add(t);
                }
                return lista;
            }
        }

        /***************************************************************************************/
        public void Insert(PublicationType entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");         
            database.ExecuteNonQuery("dbo.TipoPublicacion_Insert",entity.Name,entity.Description);
        }

        /***************************************************************************************/

        public int Delete(PublicationType entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.TipoPublicacion_Delete", entity.Id);
            return 0;
        }

        /***************************************************************************************/

        public PublicationType QueryById(int id)
        {

            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.TipoPublicacion_Search", id, null))
            {
                reader.Read();
                PublicationType p = new PublicationType();
                p.Id = Convert.ToInt32(reader["ID"]);
                p.Name = Convert.ToString(reader["NOMBRE"]);
                p.Description = Convert.ToString(reader["DESCRIPCION"]);
                 
                return p;
            }
        }

        /***************************************************************************************/

        public void Update(PublicationType entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.TipoPublicacion_Update", entity.Id, entity.Name, entity.Description);
        }


        public void DeleteById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.UserProfile_DeletePublication", id);

        }

        public IEnumerable<PublicationType> Search(int codigo, string nombre)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.TipoPublicacion_Search", codigo, nombre))
            {
                List<PublicationType> lista = new List<PublicationType>();
                while (reader.Read())
                {
                    PublicationType t = new PublicationType();
                    t.Id = Convert.ToInt32(reader["ID"]);
                    t.Name = Convert.ToString(reader["NOMBRE"]);
                    t.Description = Convert.ToString(reader["DESCRIPCION"]);
                    lista.Add(t);
                }
                return lista;
            }
        }
    }
}
