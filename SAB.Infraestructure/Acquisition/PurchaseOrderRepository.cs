using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Acquisition;
using SAB.Base.Acquisition;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace SAB.Infraestructure.Acquisition
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        public void Insert(PurchaseOrder entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(PurchaseOrder entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.PurchaseOrder_Delete",id);
            return 1;
        }

        public IEnumerable<PurchaseOrder> Search(int id, DateTime fechaD,DateTime fechaH,string state, string proveedor)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");

            using (IDataReader reader = database.ExecuteReader("dbo.PurchaseOrderSearch",id,fechaD,fechaH,state,proveedor))
            {
                while (reader.Read())
                {
                    Supplier p = new Supplier();

                    p.Id = Convert.ToInt32(reader["ID_PROVEEDOR"]);
                    p.Name = Convert.ToString(reader["NOMBRE"]);
                    yield return new PurchaseOrder
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Date = Convert.ToDateTime(reader["FECHA"]),
                        PurchaseOrder_Provider = p,
                        List_RequestOrder = null,
                        List_DetailPublication = null,
                        PurchaseOrder_Reception = null,
                        State = Convert.ToString(reader["ESTADO"]),
                    };

                }
            }
        }


        public PurchaseOrder QueryById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.PurchaseOrder_QueryById",id))
            {
                int m = reader.FieldCount;
                int n = reader.Depth;
                //int o = reader.IsClosed;
                int z = reader.RecordsAffected;
                reader.Read();
                Supplier pro = new Supplier();

                pro.Id = Convert.ToInt32(reader["ID_PROVEEDOR"]);
                pro.Name = Convert.ToString(reader["NOMBRE"]);

                PurchaseOrder p = new PurchaseOrder();
                p.Id = Convert.ToInt32(reader["ID"]);
                p.Date = Convert.ToDateTime(reader["FECHA"]);
                p.PurchaseOrder_Provider = pro;
                p.List_RequestOrder = null;
                p.List_DetailPublication = null;
                p.PurchaseOrder_Reception = null;
                p.State = Convert.ToString(reader["ESTADO"]);
                return p;
            }
        }

        public void Update(PurchaseOrder entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PurchaseOrder> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            
            using (IDataReader reader = database.ExecuteReader("dbo.PurchaseOrder_QueryAll"))
            {
                while (reader.Read())
                {
                    Supplier p = new Supplier();

                    p.Id = Convert.ToInt32(reader["ID_PROVEEDOR"]);
                    p.Name = Convert.ToString(reader["NOMBRE"]);
                    yield return new PurchaseOrder
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Date = Convert.ToDateTime(reader["FECHA"]),
                        PurchaseOrder_Provider=p,
                        List_RequestOrder = null,
                        List_DetailPublication = null,        
                        PurchaseOrder_Reception =null,
                        State = Convert.ToString(reader["ESTADO"]),
                    };

                }
            }
        }


        public void Insert(string idproveedor, string[] publicaciones,string[] cantidades)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
           
            int idorder;
            idorder =  Convert.ToInt32(database.ExecuteScalar("dbo.PurchaseOrder_Insert", Convert.ToInt32(idproveedor), DateTime.Today));

           
            if (publicaciones != null)
            {
                int n = publicaciones.Length;

                for (int i = 0; i < n; i++)
                {
                    database.ExecuteNonQuery("dbo.PurchaseOrderDetail_Insert", idorder, Convert.ToInt32(publicaciones[i]), Convert.ToInt32(cantidades[i]));
                }
            }

            //insertPurchaseORder
            
            
        }




        public void Approve(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.PurchaseOrder_Approve", id);
            
        }

        public void Reject(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.PurchaseOrder_Reject", id);
            
        }


        public void Reception(string[] detalles, string[] idPublicaciones ,string[] cantidadesAntes, string[] cantidadesPlus)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");

            
            if (detalles != null)
            {
                int n = detalles.Length;

                for (int i = 0; i < n; i++)
                {
                    database.ExecuteNonQuery("dbo.PurchaseOrderDetail_ReceptionUpdate", Convert.ToInt32(detalles[i]), Convert.ToInt32(cantidadesAntes[i])+ Convert.ToInt32(cantidadesPlus[i]));
                }
            }
            if (idPublicaciones != null)
            {
                int n = idPublicaciones.Length;
                for (int i = 0; i < n; i++)
                {
                    int cant = Convert.ToInt32(cantidadesPlus[i]);
                    for (int j = 0; j<cant;j++ )
                        database.ExecuteNonQuery("dbo.ItemPublicacion_Insert2", "Sin Ubicar", null, Convert.ToInt32(idPublicaciones[i]),null,null, DateTime.Now);
                }
            }

        }
    }
}
