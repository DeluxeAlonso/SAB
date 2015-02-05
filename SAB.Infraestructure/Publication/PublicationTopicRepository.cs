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
    public class PublicationTopicRepository : IPublicationTopicRepository
    {
        /***************************************************************************************/

        public IEnumerable<PublicationTopic> QueryAll()
        {
            throw new NotImplementedException();
        }

        /***************************************************************************************/

        public PublicationTopic QueryById(int id)
        {
            throw new NotImplementedException();
        }

        /***************************************************************************************/

        public IEnumerable<Topic> Search(int idPublication)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.TemaXPublicacion_Search", idPublication))
            {
                while (reader.Read())
                {
                    yield return new Topic
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Name = Convert.ToString(reader["NOMBRE"]),
                        Description = Convert.ToString(reader["DESCRIPCION"])
                    };

                }
            }
        }

        /***************************************************************************************/

        public void Insert(PublicationTopic entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.TemaXPublicacion_Insert", entity.IdPublication, entity.IdTopic);
        }

        /***************************************************************************************/

        public int Delete(PublicationTopic entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            return database.ExecuteNonQuery("dbo.TemaXPublicacion_Delete", entity.IdPublication, entity.IdTopic);
        }
        
        /***************************************************************************************/

        public void Update(PublicationTopic entity)
        {
            throw new NotImplementedException();
        }

        /***************************************************************************************/
    }
}
