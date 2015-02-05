using Microsoft.Practices.EnterpriseLibrary.Data;
using SAB.Base.Publication;
using SAB.Domain.Publication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Infraestructure.Publication
{
    public class PublicationItemRepository : IPublicationItemRepository
    {
        /***************************************************************************************/

        public IEnumerable<PublicationItem> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.ItemPublicacion_QueryAll"))
            {
                while (reader.Read())
                {
                    yield return new PublicationItem
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Estado = Convert.ToString(reader["ESTADO"]),
                        Id_Activo = Convert.ToInt32(reader["ID_ACTIVO"]),
                        Id_Publication = Convert.ToInt32(reader["ID_PUBLICACION"]),
                        Id_Biblioteca = Convert.ToInt32(reader["ID_BIBLIOTECA"]),
                        Id_Recepcion = Convert.ToInt32(reader["ID_RECEPCION"]),
                    };
                }
            }
        }

        /***************************************************************************************/

        public void Insert(PublicationItem entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.ItemPublicacion_Insert", entity.Estado, entity.Id_Activo,
                entity.Id_Publication, entity.Id_Biblioteca, entity.Id_Recepcion);
        }

        /***************************************************************************************/
        
        public void Insert_Donacion(PublicationItem entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.ItemPublicacion_Insert_Donacion", entity.Estado,
                entity.Id_Activo, entity.Id_Publication, entity.Id_Biblioteca);
        }

        /***************************************************************************************/
        public void Insert_Donacion_Activo(PublicationItem entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.ItemPublicacion_Insert_Donacion_Activo", entity.Estado,
                entity.Id_Publication, entity.Id_Biblioteca);
        }

        /***************************************************************************************/
        public int Delete(PublicationItem entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.ItemPublicacion_Delete", entity.Id);
            throw new NotImplementedException();
            
        }

        /***************************************************************************************/

        public IEnumerable<PublicationItem> QueryByPublication(int idPublication, int idBiblioteca)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.ItemPublicacion_QueryByPublicacion",
                idPublication, idBiblioteca))
            {
                List<PublicationItem> lista = new List<PublicationItem>();

                while (reader.Read())
                {
                    PublicationItem item = new PublicationItem();
                    item.Id = Convert.ToInt32(reader["ID"]);
                    item.Estado = Convert.ToString(reader["ESTADO"]);
                    item.Id_Publication = Convert.ToInt32(reader["ID_PUBLICACION"]);
                    item.Id_Biblioteca = Convert.ToInt32(reader["ID_BIBLIOTECA"]);
                    item.FechaRecepcion = Convert.ToDateTime(reader["FECHA_RECEPCION"]);
                    
                    if (item.Estado.Equals("Sin Ubicar"))
                        item.Id_Activo = 0;
                    else
                        item.Id_Activo = Convert.ToInt32(reader["ID_ACTIVO"]);

                    try
                    {
                        item.Id_Recepcion = Convert.ToInt32(reader["ID_RECEPCION"]);
                    }
                    catch (Exception)
                    {
                        item.Id_Recepcion = 0;
                    }

                    lista.Add(item);
                }
                return lista;
            }
        }

        /***************************************************************************************/

        public IEnumerable<PublicationItem> QueryByState(int idPublication)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");  
            using (IDataReader reader = database.ExecuteReader("dbo.ItemPublicacion_QueryByState",idPublication))
            {
                while (reader.Read())
                {
                    yield return new PublicationItem
                    {
                        Id = Convert.ToInt32(reader["ID"])
                    };
                }
            }
        }

        /***************************************************************************************/

        public PublicationItem QueryById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.ItemPublicacion_QueryByID",id))
            {
                PublicationItem item = new PublicationItem();
                if (reader.Read())
                {
                    item.Id = Convert.ToInt32(reader["ID"]);
                    item.Estado = Convert.ToString(reader["ESTADO"]);
                    if(reader["ID_ACTIVO"]!= DBNull.Value)
                        item.Id_Activo = Convert.ToInt32(reader["ID_ACTIVO"]);
                    else
                        item.Id_Activo = 0;
                    item.Id_Publication = Convert.ToInt32(reader["ID_PUBLICACION"]);
                    item.Id_Biblioteca = Convert.ToInt32(reader["ID_BIBLIOTECA"]);
                    item.Id_Recepcion = Convert.ToInt32(reader["ID_RECEPCION"]);

                }
                return item;
            }
            int a = 5;
        }

        /***************************************************************************************/

        public void Update(PublicationItem entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.ItemPublicacion_Update", entity.Id,entity.Estado,entity.Id_Biblioteca);
        }

        /***************************************************************************************/

        public IEnumerable<PublicationItem> QueryByPublication_Biblioteca(int idPublication, int idBiblioteca)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.ItemPublicacion_QueryByPublicacion_Biblioteca", idPublication,idBiblioteca))
            {
                while (reader.Read())
                {
                    yield return new PublicationItem
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Estado = Convert.ToString(reader["ESTADO"]),
                        Id_Activo = (reader["ID_ACTIVO"] == DBNull.Value)? 0 : Convert.ToInt32(reader["ID_ACTIVO"]),
                        Id_Publication =(reader["ID_PUBLICACION"] == DBNull.Value)? 0 : Convert.ToInt32(reader["ID_PUBLICACION"]),
                        Id_Biblioteca = (reader["ID_BIBLIOTECA"] == DBNull.Value)? 0 : Convert.ToInt32(reader["ID_BIBLIOTECA"]),
                        Id_Recepcion = (reader["ID_RECEPCION"] == DBNull.Value) ? 0 : Convert.ToInt32(reader["ID_RECEPCION"])
                    };
                }
            }
        }

        /***************************************************************************************/
    }
}
