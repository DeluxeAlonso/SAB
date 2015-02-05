using Microsoft.Practices.EnterpriseLibrary.Data;
using SAB.Base.Publication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;
using System.IO;

namespace SAB.Infraestructure.Publication
{
    public class PublicationTitleRepository : IPublicationTitleRepository
    {
        /***************************************************************************************/

        public IEnumerable<PublicationTitle> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Publicacion_QueryAll"))
            {
                while (reader.Read())
                {

                    yield return new PublicationTitle
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        ISBN = Convert.ToString(reader["ISBN"]),
                        Title = Convert.ToString(reader["TITULO"]),
                        Description = Convert.ToString(reader["DESCRIPCION"]),
                        Year_Publication = Convert.ToInt32(reader["ANIO_PUBLICACION"]),
                        Imprint = Convert.ToString(reader["PIE_IMPRENTA"]),
                        Id_Type = Convert.ToInt32(reader["ID_TIPO_PUBLICACION"]),
                        Id_Editorial = Convert.ToInt32(reader["ID_EDITORIAL"]),
                        Id_Author = Convert.ToInt32(reader["ID_AUTOR"]),
                        State = Convert.ToString(reader["ESTADO"]),

                    };

                }
            }
        }

        /***************************************************************************************/

        public IEnumerable<PublicationTitle> Search(PublicationTitle entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Publicacion_Search", 
                entity.Id, entity.Id_Type, entity.Id_Editorial, entity.Id_Author, entity.nameAuthor, entity.nameEditorial))
            {
                while (reader.Read())
                {
                    yield return new PublicationTitle
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Title = Convert.ToString(reader["TITULO"]),
                        nameType = Convert.ToString(reader["NOMBRE_TIPO"]),
                        nameEditorial = Convert.ToString(reader["NOMBRE_EDITORIAL"]),
                        nameAuthor = Convert.ToString(reader["NOMBRE_AUTOR"])
                    };

                }
            }
        }

        /***************************************************************************************/

        public IEnumerable<PublicationTitle> Search(int id, int id_tipo, int id_editorial, int id_autor, string autor, string editorial )
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Publicacion_Search",
                id, id_tipo, id_editorial, id_autor, autor, editorial))
            {
                while (reader.Read())
                {
                    yield return new PublicationTitle
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Title = Convert.ToString(reader["TITULO"]),
                        nameType = Convert.ToString(reader["NOMBRE_TIPO"]),
                        nameEditorial = Convert.ToString(reader["NOMBRE_EDITORIAL"]),
                        nameAuthor = Convert.ToString(reader["NOMBRE_AUTOR"])
                    };

                }
            }
        }

        /***************************************************************************************/

        public PublicationTitle QueryById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Publicacion_QueryByID", id))
            {
                PublicationTitle publication = new PublicationTitle();

                if (reader.Read())
                {              
                    publication.Id= Convert.ToInt32(reader["ID"]);
                    publication.ISBN = Convert.ToString(reader["ISBN"]);
                    publication.Title = Convert.ToString(reader["TITULO"]);
                    publication.State = Convert.ToString(reader["ESTADO"]);
                    publication.Description = Convert.ToString(reader["DESCRIPCION"]);
                    publication.Year_Publication = Convert.ToInt32(reader["ANIO_PUBLICACION"]);
                    publication.Imprint = Convert.ToString(reader["PIE_IMPRENTA"]);
                    publication.Front = Convert.ToString(reader["PORTADA"]);

                    publication.Id_Type = Convert.ToInt32(reader["ID_TIPO_PUBLICACION"]);
                    publication.Id_Editorial = Convert.ToInt32(reader["ID_EDITORIAL"]);
                    publication.Id_Author = Convert.ToInt32(reader["ID_AUTOR"]);

                    publication.nameType = Convert.ToString(reader["TIPO_PUBLICACION"]);
                    publication.nameEditorial = Convert.ToString(reader["EDITORIAL"]);
                    publication.nameAuthor = Convert.ToString(reader["AUTOR"]);
                    
                }
                return publication;
                
            }
        }


        /***************************************************************************************/

        public void Insert(PublicationTitle entity)
        {

            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Publicacion_Insert",
                entity.ISBN, entity.Title, entity.Description,
                entity.Year_Publication, entity.Imprint,
                entity.Id_Type, entity.Id_Editorial, entity.Id_Author,
                entity.Front
             );
        }

        /***************************************************************************************/

        public void Update(PublicationTitle entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Publicacion_Update",
                entity.Id, entity.ISBN, entity.Title, entity.Description,
                entity.Year_Publication, entity.Imprint,
                entity.Id_Type, entity.Id_Editorial, entity.Id_Author,
                entity.Front
             );
        }

        /***************************************************************************************/

        public int Delete(PublicationTitle entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            return database.ExecuteNonQuery("dbo.Publicacion_Delete", entity.Id);
        }


        /***************************************************************************************/
        public IEnumerable<PublicationTitle> SearchPublicationsByFilters(PublicationTitle entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            Object Year=null;
            Object Id_Type=null;
            if(entity.Year_Publication!=0)
                Year=entity.Year_Publication;
            if(entity.Id_Type!=0)
                Id_Type = entity.Id_Type;
            using (IDataReader reader = database.ExecuteReader("dbo.Publicacion_Search3",
                entity.nameAuthor,entity.Title,entity.nameEditorial,Year,Id_Type,entity.providerId))
            {
                while (reader.Read())
                {
                    yield return new PublicationTitle
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        nameAuthor = Convert.ToString(reader["AUTOR"]),
                        Title = Convert.ToString(reader["TITULO"]),
                        Year_Publication=Convert.ToInt32(reader["ANIO_PUBLICACION"]),
                        ISBN=Convert.ToString(reader["ISBN"]),
                        State=Convert.ToString(reader["ESTADO"])
                    };

                }
            }
        }

        /***************************************************************************************/

        public IEnumerable<PublicationTitle> Search2(string searchText)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Publicacion_Search2", searchText))
            {
                List<PublicationTitle> lista = new List<PublicationTitle>();
                while (reader.Read())
                {
                    PublicationTitle p = new PublicationTitle();
                    p.Id = Convert.ToInt32(reader["ID"]);
                    p.Title = Convert.ToString(reader["TITULO"]);
                    p.Year_Publication = Convert.ToInt32(reader["ANIO_PUBLICACION"]);
                    p.nameAuthor = Convert.ToString(reader["AUTOR"]);
                    p.Front = (reader["PORTADA"] == DBNull.Value) ? "" : Convert.ToString(reader["PORTADA"]);
                    lista.Add(p);
                }
                return lista;
            }
        }

        /***************************************************************************************/

        public IEnumerable<PublicationTitle> Search3(string autor, string titulo, string editorial, int? anio, int? tipoPublicacion)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Publicacion_Search3",autor,titulo,editorial,anio,tipoPublicacion,0))
            {
                List<PublicationTitle> lista = new List<PublicationTitle>();
                while (reader.Read())
                {
                    PublicationTitle p = new PublicationTitle();
                    p.Id = Convert.ToInt32(reader["ID"]);
                    p.Title = Convert.ToString(reader["TITULO"]);
                    p.Year_Publication = Convert.ToInt32(reader["ANIO_PUBLICACION"]);
                    p.nameAuthor = Convert.ToString(reader["AUTOR"]);
                    p.Front = (reader["PORTADA"] == DBNull.Value) ? "" : Convert.ToString(reader["PORTADA"]);
                    lista.Add(p);
                }
                return lista;
            }
        }

        /***************************************************************************************/

        public bool PublicationExist(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Publicacion_QueryByID", id))
            {
                bool existPub = true;

                if (!reader.Read())
                {
                    existPub = false;
                }

                return existPub;

            }
        }

        /***************************************************************************************/
        
    }
}
