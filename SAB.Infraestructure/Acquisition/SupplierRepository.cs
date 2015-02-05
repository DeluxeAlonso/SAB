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
    public class SupplierRepository:ISupplierRepository
    {

        public void Insert(Supplier entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Supplier_Insert", entity.Name, entity.Contacto, entity.Ruc, entity.Address, entity.Email, entity.Telephone);
        
        
        }

        public void Actualiza(int id, string nombre, string contacto, string direccion, string telefono, string correo) {

            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Supplier_Update", id, nombre, contacto, direccion, telefono, correo);
        
        
        }

        public int Delete(Supplier e)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Supplier_Delete", e.Id);
            return 1;
        }

        public Supplier QueryById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Supplier_QueryById", id))
            {

                if (!reader.Read()) return null;
                return new Supplier
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    Name = Convert.ToString(reader["NOMBRE"]),
                    Address = Convert.ToString(reader["DIRECCION"]),
                    Telephone = Convert.ToString(reader["TELEFONO"]),
                    Status = Convert.ToString(reader["ESTADO"]),
                    Contacto = Convert.ToString(reader["CONTACTO"]),
                    Ruc = Convert.ToString(reader["RUC"]),
                    RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]),
                    Email= Convert.ToString(reader["EMAIL"]),
                };


            }
        }

        public void Update(Supplier entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Supplier> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var suppliers = new List<Supplier>();

            using (IDataReader reader = database.ExecuteReader("dbo.Supplier_QueryAll"))
            {
                while (reader.Read())
                {
                    Supplier u = new Supplier();
                    u.Id = Convert.ToInt32(reader["ID"]);
                    u.Name = Convert.ToString(reader["NOMBRE"]);
                    u.Address = Convert.ToString(reader["DIRECCION"]);
                    u.Telephone = Convert.ToString(reader["TELEFONO"]);
                    u.Status = Convert.ToString(reader["ESTADO"]);
                    u.Contacto = Convert.ToString(reader["CONTACTO"]);
                    u.Ruc = Convert.ToString(reader["RUC"]);
                    u.RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    u.Email = Convert.ToString(reader["EMAIL"]);
                    suppliers.Add(u);

                }
            }
            return suppliers;
        }



        public IEnumerable<Supplier> Search(string searchName, string searchCode, string from, string to, string searchContacto, string searchRUC)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var suppliers = new List<Supplier>();
            // searchName, searchCode, from, to, searchContacto, searchRUC);
            IDataReader reader = null;
            if (searchCode != "") reader = database.ExecuteReader("dbo.Supplier_Search", null,Int32.Parse(searchCode), null, null, null,null);
            else
            {
                if (searchRUC != "") reader = database.ExecuteReader("dbo.Supplier_Search", null, null, null, null, null, searchRUC);
                else
                {
                    
                 
                    if ((from == "" || from ==" ") && to == "" && searchName == "" && searchContacto == "") return this.QueryAll();
                    if (from == "" && to == "" && searchName == "" && searchContacto != "") reader = database.ExecuteReader("dbo.Supplier_Search", null, null, null, null, searchContacto, null);
                    if (from == "" && to == "" && searchName != "" && searchContacto == "") reader = database.ExecuteReader("dbo.Supplier_Search", searchName, null, null, null, null, null);
                    if (from == "" && to == "" && searchName != "" && searchContacto != "") reader = database.ExecuteReader("dbo.Supplier_Search", searchName, null, null, null, searchContacto, null);
                    if (from != "" && to == "" && searchName == "" && searchContacto == "") { 
                         DateTime desde= DateTime.ParseExact(from, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        reader = database.ExecuteReader("dbo.Supplier_Search", null, null, desde.ToString("yyyy-MM-dd"), null, null, null); 
                    }
                    if (from == "" && to != "" && searchName == "" && searchContacto == " "){
                        DateTime hasta = DateTime.ParseExact(to, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        reader = database.ExecuteReader("dbo.Supplier_Search", null, null, null, hasta.ToString("yyyy-MM-dd"), null, searchRUC);
                    }
                }
            }
            if (reader == null) return null;
            using (reader)
            {
                while (reader.Read())
                {
                    Supplier u = new Supplier();
                    u.Id = Convert.ToInt32(reader["ID"]);
                    u.Name = Convert.ToString(reader["NOMBRE"]);
                    u.Address = Convert.ToString(reader["DIRECCION"]);
                    u.Telephone = Convert.ToString(reader["TELEFONO"]);
                    u.Status = Convert.ToString(reader["ESTADO"]);
                    u.Contacto = Convert.ToString(reader["CONTACTO"]);
                    u.Ruc = Convert.ToString(reader["RUC"]);
                    u.Email = Convert.ToString(reader["EMAIL"]);
                    u.RegisterDate = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    suppliers.Add(u);

                }
            }
            return suppliers;

        }
    }
}
