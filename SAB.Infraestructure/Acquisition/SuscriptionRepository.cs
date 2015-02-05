using Microsoft.Practices.EnterpriseLibrary.Data;
using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Infraestructure.Acquisition
{
    public class SuscriptionRepository : ISuscriptionRepository

    {
        public void Insert(Suscription entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Suscription_Insert", entity.RegTime, entity.Id_Editorial,
                entity.Id_TypePublication, entity.state, entity.description, entity.Id_Publication);

        }

        public int Delete(Suscription entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Suscription_Delete", entity.Id);
            throw new NotImplementedException();
        }

        public Suscription QueryById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Suscription_QueryByID", id))
            
            {
                Suscription suscription = new Suscription();
                if (reader.Read())
                {
                    suscription.Id = Convert.ToInt32(reader["ID"]);
                    suscription.Id_Editorial = Convert.ToInt32(reader["ID_EDITORIAL"]);
                    suscription.Id_Publication = Convert.ToInt32(reader["ID_PUBLICACION"]);
                    suscription.RegTime = Convert.ToDateTime(reader["FECHA_REG"]);
                    suscription.Id_TypePublication = Convert.ToInt32(reader["ID_TIPO_PUBLICACION"]);
                    suscription.state = Convert.ToString(reader["ESTADO"]);
                    suscription.description = Convert.ToString(reader["DESCRIPCION"]);
                }
                return suscription;
            }
        }

        public void Update(Suscription entity)
        {

        
        }
        public IEnumerable<Suscription> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Suscription_QueryAll"))
            {
                while (reader.Read())

                {
                    yield return new Suscription
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        RegTime = Convert.ToDateTime(reader["FECHA_REG"]),
                        Id_Editorial = Convert.ToInt32(reader["ID_EDITORIAL"]),
                        Id_TypePublication = Convert.ToInt32(reader["ID_TIPO_PUBLICACION"]),
                        state = Convert.ToString(reader["ESTADO"]),
                        Id_Publication = Convert.ToInt32(reader["ID_PUBLICACION"]),
                        description = Convert.ToString(reader["DESCRIPCION"])

                    };

                }
            };
        }

        public IEnumerable<Suscription> Search(Suscription parametros, string titulo)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Suscription_Search", parametros.Id, parametros.Id_TypePublication, parametros.state, titulo))
            {
                while (reader.Read())
                {
                    yield return new Suscription
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Id_TypePublication = Convert.ToInt32(reader["ID_TIPO_PUBLICACION"]),
                        Id_Publication = Convert.ToInt32(reader["ID_PUBLICACION"]),
                        Id_Editorial = Convert.ToInt32(reader["ID_EDITORIAL"]),
                        RegTime = Convert.ToDateTime(reader["FECHA_REG"]),
                        state = Convert.ToString(reader["ESTADO"]),
                        description = Convert.ToString(reader["DESCRIPCION"])
                    };
                }
            }
        }

    }
}
