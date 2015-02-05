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
    public class AssetsRepository : IAssetsRepository
    {
        public IEnumerable<Asset> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Assets_QueryAll"))
            {
                List<Asset> activos = new List<Asset>();
                while (reader.Read())
                {
                    Asset a = new Asset();
                    a.Id = Convert.ToInt32(reader["ID"]);
                    a.Name = Convert.ToString(reader["NOMBRE"]);
                    a.Description = Convert.ToString(reader["DESCRIPCION"]);
                    a.State = Convert.ToString(reader["ESTADO"]);
                    a.IdAssetType = Convert.ToInt32(reader["ID_TIPO_ACTIVO"]);
                    a.RegistryDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    a.Quantity = Convert.ToInt32(reader["CAPACIDAD"]);
                    a.Location = Convert.ToInt32(reader["ID_BIBLIOTECA"]);
                    activos.Add(a);
                }
                return activos;
            }

        }

        public void Insert(Asset activo)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Assets_Insert", activo.Name, activo.Description, activo.IdAssetType, activo.Location,activo.Quantity);
        }


        public void Delete(int asset_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Assets_Delete", asset_id);
        }

        public int Delete(Asset activo)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Assets_Delete", activo.Id);
            return 0;
        }
        public Asset QueryById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Assets_QueryById", id))
            {
                if (!reader.Read()) return null;
                return new Asset
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    Name = Convert.ToString(reader["NOMBRE"]),
                    Description = Convert.ToString(reader["DESCRIPCION"]),
                    State = Convert.ToString(reader["ESTADO"]),
                    Quantity = Convert.ToInt32(reader["CAPACIDAD"]),
                    IdAssetType = Convert.ToInt32(reader["ID_TIPO_ACTIVO"]),
                    RegistryDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]),
                    Location = Convert.ToInt32(reader["ID_BIBLIOTECA"]),
                };


            }

        }

        public void Update(Asset activo)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Assets_Update", activo.Id, activo.State, activo.Location, activo.Description,activo.IdAssetType,activo.Quantity);

        }


        public IEnumerable<Asset> Search(int id, DateTime fechaD, DateTime fechaH,int tipo)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Assets_Search", id, fechaD, fechaH, tipo))
            {
                List<Asset> activos = new List<Asset>();
                while (reader.Read())
                {
                    Asset a = new Asset();
                    a.Id = Convert.ToInt32(reader["ID"]);
                    a.Name = Convert.ToString(reader["NOMBRE"]);
                    a.Description = Convert.ToString(reader["DESCRIPCION"]);
                    a.State = Convert.ToString(reader["ESTADO"]);
                    a.Quantity = Convert.ToInt32(reader["CAPACIDAD"]);
                    a.IdAssetType = Convert.ToInt32(reader["ID_TIPO_ACTIVO"]);
                    a.RegistryDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    a.Location = Convert.ToInt32(reader["ID_BIBLIOTECA"]);
                    activos.Add(a);
                }
                return activos;

            }
        }
    }
}
