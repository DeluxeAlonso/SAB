using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using SAB.Base.Sanctions;
using SAB.Domain.Sanctions;

namespace SAB.Infraestructure.Sanctions
{
    public class SanctionRepository : ISanctionRepository
    {
        public bool isSanctioned(int user_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            bool isSanctioned = false;

            using (IDataReader reader = database.ExecuteReader("dbo.isSanctionedUser", user_id, DateTime.Now))
            {
                while (reader.Read())
                {
                    isSanctioned = true;
                }
            }
            return isSanctioned;
        }

        public IEnumerable<Sanction> GetSanctionsByUser(int user_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");

            var sanctions = new List<Sanction>();

            using (IDataReader reader = database.ExecuteReader("dbo.Sanctions_SearchByUser", user_id))
            {
                while (reader.Read())
                {
                    Sanction s = new Sanction();
                    s.Id = Convert.ToInt32(reader["ID"]);
                    s.Fecha = Convert.ToDateTime(reader["FECHA"]);
                    s.Id_Prestamo = Convert.ToInt32(reader["ID_PRESTAMO"]);
                    s.Id_User = user_id;
                    s.Tipo_Sancion = Convert.ToInt32(reader["ID_TIPO_SANCION"]);

                    sanctions.Add(s);
                }
            }
            return sanctions;
        }

        public IEnumerable<Sanction> GetSanctionsByDates(DateTime start, DateTime end){
            var database = DatabaseFactory.CreateDatabase("SAB");

            var sanctions = new List<Sanction>();

            using (IDataReader reader = database.ExecuteReader("dbo.Sanctions_SearchByDates",  start,  end))
            {
                while (reader.Read())
                {
                    Sanction s = new Sanction();
                    s.Id = Convert.ToInt32(reader["ID"]);
                    s.Fecha = Convert.ToDateTime(reader["FECHA"]);
                    s.Id_Prestamo = Convert.ToInt32(reader["ID_PRESTAMO"]);
                    s.Id_User = Convert.ToInt32(reader["ID_USER"]);
                    s.Tipo_Sancion = Convert.ToInt32(reader["ID_TIPO_SANCION"]);

                    sanctions.Add(s);
                }
            }

            return sanctions;
        }

        public SanctionType GetSanctionType(int id_tiposancion)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");

            SanctionType tipo_sancion=new SanctionType();
            using (IDataReader reader = database.ExecuteReader("dbo.SanctionType_QueryById", id_tiposancion))
            {
                if (reader.Read())
                {
                    tipo_sancion.Id = Convert.ToInt32(reader["ID"]);
                    tipo_sancion.FechaDesde = Convert.ToDateTime(reader["FECHADESDE"]);
                    tipo_sancion.FechaHasta = Convert.ToDateTime(reader["FECHAHASTA"]);
                    tipo_sancion.Nombre = Convert.ToString(reader["NOMBRE"]);
                    tipo_sancion.Unidad = Convert.ToString(reader["UNIDAD"]);
                    tipo_sancion.Cantidad = Convert.ToInt32(reader["CANTIDAD"]);
                }
            }
            return tipo_sancion;
        }
    }
}