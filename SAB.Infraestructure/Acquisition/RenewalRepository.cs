using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace SAB.Infraestructure.Acquisition
{
    public class RenewalRepository:IRenewalRepository
    {
        public IEnumerable<Renewal> QueryAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Renewal> QueryAll(int id_suscription)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Renovacion_QueryAll", id_suscription))
            {
                List<Renewal> renovaciones = new List<Renewal>();
                while (reader.Read())
                {
                    Renewal r = new Renewal();
                    r.Id = Convert.ToInt32(reader["ID"]);
                    r.Id_Suscription = Convert.ToInt32(reader["ID_SUSCRIPCION"]);
                    r.Amount = Convert.ToInt32(reader["CANTIDAD"]);
                    r.Start_date = (reader["FECHA_INICIO"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(reader["FECHA_INICIO"]);
                    r.End_date = (reader["FECHA_FIN"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(reader["FECHA_FIN"]);
                    r.Frequency = Convert.ToString(reader["FRECUENCIA"]);
                    r.Cost = (reader["COSTO"] == DBNull.Value) ? null : (double?)Convert.ToDouble(reader["COSTO"]);
                    r.State = Convert.ToString(reader["ESTADO"]);
                    renovaciones.Add(r);
                }
                return renovaciones;
            }
        }

        public void Insert(Renewal entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Renovacion_Insert", entity.Id_Suscription, entity.Amount, entity.Start_date,
                entity.End_date, entity.Frequency, entity.Cost);
        }

        public int Delete(Renewal entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Renovacion_Delete", entity.Id);
            return 0;
        }

        public Renewal QueryById(int id)
        {
            Renewal renewal = new Renewal();
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Renovacion_QueryById",id))
            {
                while (reader.Read())
                {
                    renewal.Id = Convert.ToInt32(reader["ID"]);
                    renewal.Id_Suscription = Convert.ToInt32(reader["ID_SUSCRIPCION"]);
                    renewal.Amount = Convert.ToInt32(reader["CANTIDAD"]);
                    renewal.Start_date = (reader["FECHA_INICIO"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(reader["FECHA_INICIO"]);
                    renewal.End_date = (reader["FECHA_FIN"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(reader["FECHA_FIN"]);
                    renewal.Frequency = Convert.ToString(reader["FRECUENCIA"]);
                    renewal.Cost = (reader["COSTO"] == DBNull.Value) ? null : (double?)Convert.ToDouble(reader["COSTO"]);
                    renewal.State = Convert.ToString(reader["ESTADO"]);
                }
            }
            return renewal;
        }

        public void Update(Renewal entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Renovacion_Update", entity.Id, entity.Amount, entity.Start_date,
                entity.End_date, entity.Frequency, entity.Cost);
        }

        public void Caducar(Renewal entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Renovacion_Caducar", entity.Id);
        }
    }
}
