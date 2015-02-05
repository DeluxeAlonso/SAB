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
    public class DevolutionProfileRepository :IDevolutionProfileRepository
    {

         void Base.IRepository<Domain.Politica.DevolutionProfile>.Insert(Domain.Politica.DevolutionProfile entity)
        {
            throw new NotImplementedException();
        }

         int Base.IRepository<Domain.Politica.DevolutionProfile>.Delete(Domain.Politica.DevolutionProfile entity)
        {
            throw new NotImplementedException();
        }

         Domain.Politica.DevolutionProfile Base.IRepository<Domain.Politica.DevolutionProfile>.QueryById(int id)
        {
            throw new NotImplementedException();
        }

         void Base.IRepository<Domain.Politica.DevolutionProfile>.Update(Domain.Politica.DevolutionProfile entity)
        {
            throw new NotImplementedException();
        }

          IEnumerable<Domain.Politica.DevolutionProfile> Base.IRepository<Domain.Politica.DevolutionProfile>.QueryAll()
        {
            throw new NotImplementedException();
        }



        public IEnumerable<DevolutionProfile> QueryByIdPerfil(int i)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var devolutions = new List<DevolutionProfile>();

            using (IDataReader reader = database.ExecuteReader("dbo.UserProfile_QueryAllDevolutions",i))
            {
                while (reader.Read())
                {
                    DevolutionProfile u = new DevolutionProfile();
             
                    u.Id = Convert.ToInt32(reader["ID"]);
                    u.Description = Convert.ToString(reader["DESCRIPCION"]);
                    u.From = Convert.ToDateTime(reader["FECHA_DESDE"]);
                    u.To = Convert.ToDateTime(reader["FECHA_HASTA"]);
                    u.Days = Convert.ToInt32(reader["CANT_DIAS"]);
                   
                    devolutions.Add(u);

                }
            }
            return devolutions;
        }
    }
}
