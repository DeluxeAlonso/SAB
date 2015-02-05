using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base.Library;
using SAB.Domain.Library;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace SAB.Infraestructure.Library
{
    public class LocalRepository : ILocalRepository
    {
        public IEnumerable<Local> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Local_QueryAll"))
            {
                List<Local> lista = new List<Local>();
                while (reader.Read())
                {
                    Local l = new Local();
                    l.Id = Convert.ToInt32(reader["ID"]);
                    l.Name = Convert.ToString(reader["NOMBRE"]);
                    l.Opening_Date = (reader["FECHA_APERTURA"]==DBNull.Value)?DateTime.MinValue:Convert.ToDateTime(reader["FECHA_APERTURA"]);
                    l.Address = Convert.ToString(reader["DIRECCION"]);
                    l.Distric = Convert.ToString(reader["DISTRITO"]);
                    l.City = Convert.ToString(reader["CIUDAD"]);
                    l.Phone = Convert.ToString(reader["TELEFONO"]);
                    l.Mail = Convert.ToString(reader["CORREO"]);
                    lista.Add(l);
                }
                return lista;
            }
        }

        public void Insert(Local entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Local_Insert", entity.Name, entity.Opening_Date, entity.Address,
                entity.Distric, entity.City, entity.Phone, entity.Mail);
        }

        public void Delete(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Local_Delete", id);
        }

        public int Delete(Local local)
        {
            return 0;
        }

        public Local QueryById(int id)
        {
            Local local = new Local();
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Local_QueryById",id))
            {
                while (reader.Read())
                {
                    local.Id = Convert.ToInt32(reader["ID"]);
                    local.Name = Convert.ToString(reader["NOMBRE"]);
                    local.Opening_Date = (reader["FECHA_APERTURA"]==DBNull.Value)?DateTime.MinValue:Convert.ToDateTime(reader["FECHA_APERTURA"]);
                    local.Address = Convert.ToString(reader["DIRECCION"]);
                    local.Distric = Convert.ToString(reader["DISTRITO"]);
                    local.City = Convert.ToString(reader["CIUDAD"]);
                    local.Phone = Convert.ToString(reader["TELEFONO"]);
                    local.Mail = Convert.ToString(reader["CORREO"]);
                }
            }
            return local;
        }

        public void Update(Local entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Local_Update", entity.Id, entity.Name, entity.Address, entity.Distric, entity.City, 
                entity.Phone, entity.Mail);
        }

        public IEnumerable<Local> Search(Local parametrosBusqueda)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Local_Search",parametrosBusqueda.Id,parametrosBusqueda.Name,
                parametrosBusqueda.City,parametrosBusqueda.Distric))
            {
                List<Local> locales = new List<Local>();
                while (reader.Read())
                {
                    Local l = new Local();
                    l.Id = Convert.ToInt32(reader["ID"]);
                    l.Name = Convert.ToString(reader["NOMBRE"]);
                    l.Opening_Date = (reader["FECHA_APERTURA"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(reader["FECHA_APERTURA"]);
                    l.Address = Convert.ToString(reader["DIRECCION"]);
                    l.Distric = Convert.ToString(reader["DISTRITO"]);
                    l.City = Convert.ToString(reader["CIUDAD"]);
                    l.Phone = Convert.ToString(reader["TELEFONO"]);
                    l.Mail = Convert.ToString(reader["CORREO"]);
                    locales.Add(l);
                }
                return locales;
            }
        }
    }
}
