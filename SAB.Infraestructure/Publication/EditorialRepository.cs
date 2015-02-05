using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;
using SAB.Base.Publication;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace SAB.Infraestructure.Publication
{
    public class EditorialRepository : IEditorialRepository
    {
        /***************************************************************************************/

        public IEnumerable<Editorial> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Editorial_QueryAll"))
            {
                while (reader.Read())
                {
                    yield return new Editorial
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        RUC = Convert.ToString(reader["RUC"]),
                        Company_Name = Convert.ToString(reader["RAZON_SOCIAL"]),
                        RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]),
                        State = Convert.ToString(reader["ESTADO"]),
                    };

                }
            }
        }

        /***************************************************************************************/

        public IEnumerable<Editorial> Search(int codigo, string razonSocial, DateTime fechaInicio, DateTime fechaFin)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Editorial_Search",
                codigo, razonSocial, fechaInicio, fechaFin))
            {
                while (reader.Read())
                {
                    yield return new Editorial
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        RUC = Convert.ToString(reader["RUC"]),
                        Company_Name = Convert.ToString(reader["RAZON_SOCIAL"]),
                        RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]),
                        State = Convert.ToString(reader["ESTADO"]),
                    };

                }
            }
        }

        /***************************************************************************************/

        public void Insert(Editorial entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteReader("dbo.Editorial_Insert", entity.Company_Name, entity.RUC) ;
        }

        /***************************************************************************************/

        public int Delete(Editorial entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            return database.ExecuteNonQuery("dbo.Editorial_Delete", entity.Id);
        }

        /***************************************************************************************/

        public Editorial QueryById(int id)
        {
            Editorial editorial = new Editorial();
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Editorial_QueryById", id))
            {
                if (reader.Read())
                {
                    editorial.Id = Convert.ToInt32(reader["ID"]);
                    editorial.Company_Name = Convert.ToString(reader["RAZON_SOCIAL"]);
                    editorial.RUC = Convert.ToString(reader["RUC"]);
                    editorial.RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    editorial.State = Convert.ToString(reader["ESTADO"]);
                }
            }
            return editorial;
        }


        public void Update(Editorial entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Editorial_Update",
                entity.Id, entity.Company_Name, entity.RUC))
            {

            }
        }
    }
}
