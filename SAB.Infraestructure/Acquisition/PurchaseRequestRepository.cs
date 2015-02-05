using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using SAB.Domain.User;
using System.Web;
namespace SAB.Infraestructure.Acquisition
{
    public class PurchaseRequestRepository:IPurchaseRequestRepository
    {
        public void Insert(PurchaseRequest entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(PurchaseRequest entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.PurchaseRequest_Delete", id);
            return 1;
        }

        

        public void Update(PurchaseRequest entity)
        {
            throw new NotImplementedException();
        }

        public PurchaseRequest QueryById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.PurchaseRequest_QueryById", id))
            {

                reader.Read();

                PurchaseRequest p = new PurchaseRequest();
                p.Id = Convert.ToInt32(reader["ID"]);
                p.Date = Convert.ToDateTime(reader["FECHA"]);
                p.Description =reader["DESCRIPCION"]==DBNull.Value?"": Convert.ToString(reader["DESCRIPCION"]);
                p.ProviderName =reader["NOMBRE"]==DBNull.Value?"": Convert.ToString(reader["NOMBRE"]);
                p.IdProvider =reader["ID_PROVEEDOR"]==DBNull.Value?0: Convert.ToInt32(reader["ID_PROVEEDOR"]);
                p.IdUser = Convert.ToInt32(reader["ID_USUARIO"]);
                p.UserName = Convert.ToString(reader["USUARIO"]);
                p.Estado = Convert.ToString(reader["ESTADO"]);
                return p;
            }
        }

        public IEnumerable<PurchaseRequest> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.PurchaseRequest_QueryAll"))
            {
                while (reader.Read())
                {

                    yield return new PurchaseRequest
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Date=Convert.ToDateTime(reader["FECHA"]),                       
                        Description = Convert.ToString(reader["DESCRIPCION"]),
                        Estado = Convert.ToString(reader["ESTADO"]),
                        IdRequestType = reader["ID_TIPO_SOLICITUD"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_TIPO_SOLICITUD"]),
                        IdPurchaseOrder = reader["ID_ORDEN_COMPRA"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_ORDEN_COMPRA"]),
                        IdUser = reader["ID_USUARIO"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_USUARIO"]),
                        UserName=Convert.ToString(reader["NOMBRE"]),
                        IdProvider = reader["ID_PROVEEDOR"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_PROVEEDOR"])
                    };

                }
            }
        }

        public void UpdateById(int id, int idorder)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.PurchaseRequest_UpdateById", id,idorder);
        }


        public IEnumerable<PurchaseRequest> QueryByOrder(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.PurchaseOrder_QueryRequestByOrder", id))
            {
                while (reader.Read())
                {

                    yield return new PurchaseRequest
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Date = Convert.ToDateTime(reader["FECHA"]),
                        Description = Convert.ToString(reader["DESCRIPCION"]),
                        Estado = Convert.ToString(reader["ESTADO"]),
                        IdRequestType = reader["ID_TIPO_SOLICITUD"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_TIPO_SOLICITUD"]),
                        IdPurchaseOrder = reader["ID_ORDEN_COMPRA"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_ORDEN_COMPRA"]),
                        IdUser = reader["ID_USUARIO"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_USUARIO"]),
                        UserName = Convert.ToString(reader["NOMBRE"]),
                        IdProvider = reader["ID_PROVEEDOR"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_PROVEEDOR"])
                    };

                }
            }
        }


        public IEnumerable<PurchaseRequest> Search(int id, DateTime fechaD, DateTime fechaH, int idProveedor, int Solicitante, string state)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");

            using (IDataReader reader = database.ExecuteReader("dbo.PurchaseRequest_Search", id, fechaD, fechaH, idProveedor, Solicitante,state))
            {
                while (reader.Read())
                {

                    yield return new PurchaseRequest
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Date = Convert.ToDateTime(reader["FECHA"]),
                        Description = Convert.ToString(reader["DESCRIPCION"]),
                        Estado = Convert.ToString(reader["ESTADO"]),
                        IdRequestType = reader["ID_TIPO_SOLICITUD"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_TIPO_SOLICITUD"]),
                        IdPurchaseOrder = reader["ID_ORDEN_COMPRA"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_ORDEN_COMPRA"]),
                        IdUser = reader["ID_USUARIO"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_USUARIO"]),
                        UserName = Convert.ToString(reader["NOMBRE"]),
                        IdProvider = reader["ID_PROVEEDOR"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_PROVEEDOR"])
                    };

                }
            }
        }

        public IEnumerable<PurchaseRequest> Search2(int id, DateTime fechaD, DateTime fechaH, string estado, string solicitante)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.PurchaseRequest_Search2", id, fechaD, fechaH, estado, solicitante))
            {
                while (reader.Read())
                {
                    yield return new PurchaseRequest
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Date = Convert.ToDateTime(reader["FECHA"]),
                        Description = Convert.ToString(reader["DESCRIPCION"]),
                        Estado = Convert.ToString(reader["ESTADO"]),
                        IdRequestType = reader["ID_TIPO_SOLICITUD"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_TIPO_SOLICITUD"]),
                        IdPurchaseOrder = reader["ID_ORDEN_COMPRA"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_ORDEN_COMPRA"]),
                        IdUser = reader["ID_USUARIO"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_USUARIO"]),
                        UserName = Convert.ToString(reader["USUARIO"]),
                        
                    };
                }
            }
        }

        public void Insert(string[] publicaciones, string[] cantidades,string descripcion,int id)
        {
            

             if (publicaciones != null)
            {
                var database = DatabaseFactory.CreateDatabase("SAB");
                int idRequest;
                idRequest = Convert.ToInt32(database.ExecuteScalar("dbo.PurchaseRequest_Insert", descripcion, id));
           
                int n = publicaciones.Length;

                for (int i = 0; i < n; i++)
                {
                    database.ExecuteNonQuery("dbo.PurchaseRequestDetail_Insert", idRequest, Convert.ToInt32(publicaciones[i]), Convert.ToInt32(cantidades[i]));
                }
            }

            //insertPurchaseRequest


        }

        public void Approve(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.PurchaseRequest_Approve", id);

        }

        public void Reject(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.PurchaseRequest_Reject", id);

        }



        public void InsertN(string [] isbns, string [] titulos, string [] proveedores, string []precios,int id)
        {
            if (isbns != null)
            {
                var database = DatabaseFactory.CreateDatabase("SAB");
                int idRequest;
                idRequest = Convert.ToInt32(database.ExecuteScalar("dbo.PurchaseRequest_InsertN", DateTime.Now,id));

                int n = isbns.Length;

                for (int i = 0; i < n; i++)
                {
                    database.ExecuteNonQuery("dbo.PurchaseRequestDetail_InsertN", isbns[i],titulos[i],null,proveedores[i],precios[0],idRequest);
                }
            }
        }
    }
}
