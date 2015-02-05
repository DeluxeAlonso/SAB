using Microsoft.Practices.EnterpriseLibrary.Data;
using SAB.Base.Assets;
using SAB.Domain.Assets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Infraestructure.Assets
{
    public class TypeAssetRepository : ITypeAssetRepository
    {
        public void Insert(Domain.Assets.TypeAsset entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.TypeAsset_Insert", entity.Name, entity.Description);
        }

        public int Delete(Domain.Assets.TypeAsset entity)
        {

            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.TypeAsset_Delete", entity.Id);
            return 0;
        }

        public Domain.Assets.TypeAsset QueryById(int id)
        {

            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.TypeAsset_QueryById", id))
            {
                if (!reader.Read()) return null;
                return new TypeAsset
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    Name = Convert.ToString(reader["NOMBRE"]),
                    Description = Convert.ToString(reader["DESCRIPCION"]),
                    Status = Convert.ToString(reader["ESTADO"]),
                    
                };


            }
        }

        public void Update(Domain.Assets.TypeAsset entity)
        {

            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.TypeAsset_Update", entity.Id, entity.Status, entity.Description, entity.Name);

        }

        public IEnumerable<Domain.Assets.TypeAsset> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.TypeAsset_QueryAll"))
            {
                List<TypeAsset> activos = new List<TypeAsset>();
                while (reader.Read())
                {
                    TypeAsset a = new TypeAsset();
                    a.Id = Convert.ToInt32(reader["ID"]);
                    a.Name = Convert.ToString(reader["NOMBRE"]);
                    a.Description = Convert.ToString(reader["DESCRIPCION"]);
                    a.Status = Convert.ToString(reader["ESTADO"]);
                    activos.Add(a);
                }
                return activos;
            }

        }


        public IEnumerable<Domain.Assets.TypeAsset> Search(int codigo, string name)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.TypeAsset_Search",codigo,name))
            {
                List<TypeAsset> activos = new List<TypeAsset>();
                while (reader.Read())
                {
                    TypeAsset a = new TypeAsset();
                    a.Id = Convert.ToInt32(reader["ID"]);
                    a.Name = Convert.ToString(reader["NOMBRE"]);
                    a.Description = Convert.ToString(reader["DESCRIPCION"]);
                    a.Status = Convert.ToString(reader["ESTADO"]);
                    activos.Add(a);
                }
                return activos;
            }

        }
    }
}
