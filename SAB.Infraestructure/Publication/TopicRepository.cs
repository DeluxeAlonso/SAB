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
    public class TopicRepository : ITopicRepository
    {
        /***************************************************************************************/

        public IEnumerable<Topic> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Tema_QueryAll"))
            {
                while (reader.Read())
                {
                    yield return new Topic
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Name = Convert.ToString(reader["NOMBRE"]),
                        Description = Convert.ToString(reader["DESCRIPCION"]),
                        State = Convert.ToString(reader["ESTADO"]),
                    };

                }
            }
        }

        /***************************************************************************************/

        public IEnumerable<Topic> Search(int id, string name)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Tema_Search", id, name))
            {
                while (reader.Read())
                {
                    yield return new Topic
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Name = Convert.ToString(reader["NOMBRE"]),
                        Description = Convert.ToString(reader["DESCRIPCION"]),
                        State = Convert.ToString(reader["ESTADO"]),
                    };

                }
            }
        }

        /***************************************************************************************/

        public Topic QueryById(int id)
        {
            Topic topic = new Topic();
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Tema_QueryById", id))
            {
                if (reader.Read())
                {
                    topic.Id = Convert.ToInt32(reader["ID"]);
                    topic.Name = Convert.ToString(reader["NOMBRE"]);
                    topic.Description = Convert.ToString(reader["DESCRIPCION"]);
                    topic.State = Convert.ToString(reader["ESTADO"]);
                }
            }
            return topic;
        }

        /***************************************************************************************/

        public void Insert(Topic entity)
        {   
            // El estado por defecto sera "Activo" y se colocara en la BD
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Tema_Insert", entity.Name, entity.Description);
        }

        /***************************************************************************************/

        public void Update(Topic entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Tema_Update", entity.Id, entity.Name, entity.Description);
        }

        /***************************************************************************************/

        public int Delete(Topic entity)
        {
            // La eliminacion sera logica, se colocara estado "Eliminado" en la BD
            var database = DatabaseFactory.CreateDatabase("SAB");
            int resultado = database.ExecuteNonQuery("dbo.Tema_Delete", entity.Id);
            return resultado;
        }

        /***************************************************************************************/
    }
}
