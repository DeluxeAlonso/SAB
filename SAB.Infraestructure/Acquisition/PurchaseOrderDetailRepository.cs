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
    public class PurchaseOrderDetailRepository: IPurchaseOrderDetailRepository


    {

        public void Insert(PurchaseOrderDetail entity)
        {
            PurchaseOrderDetail p = new PurchaseOrderDetail();
            
            
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.PurchaseOrderDetail_Insert", entity.IdPurchaseOrder, entity.PublicationName,entity.Cantidad);
        }

        public int Delete(PurchaseOrderDetail entity)
        {
            throw new NotImplementedException();
        }

        public PurchaseOrderDetail QueryById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PurchaseOrderDetail entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PurchaseOrderDetail> QueryAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PurchaseOrderDetail> QueryByOrder(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int i = 1;
            using (IDataReader reader = database.ExecuteReader("dbo.PurchaseOrderDetail_QueryByOrder",id))
            {
                while (reader.Read())
                {
                    
                    yield return new PurchaseOrderDetail
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        IdPublication = Convert.ToInt32(reader["ID_PUBLICACION"]),
                        IdPurchaseOrder = id,
                        PublicationName = Convert.ToString(reader["TITULO"]),
                        Cantidad = Convert.ToInt32(reader["CANTIDAD"]),
                        CantidadRecibida = Convert.ToInt32(reader["CANTRECIBIDA"]),
                        LineNumber = i,
                    };
                    i++;
                }
            }
        }


        public IEnumerable<PurchaseOrderDetail> QueryByDate(int biblioteca, DateTime fechaD, DateTime fechaH)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int i = 1;
            using (IDataReader reader = database.ExecuteReader("dbo.MaterialesNuevos",biblioteca, fechaD, fechaH))
            {
                while (reader.Read())
                {

                    yield return new PurchaseOrderDetail
                    {
                        IdPublication = Convert.ToInt32(reader["ID_PUBLICACION"]),
                        IdPurchaseOrder = 1,
                        PublicationName = Convert.ToString(reader["TITULO"]),
                        AuthorName = Convert.ToString(reader["NOMBRE"]) + ' ' +
                                        Convert.ToString(reader["AP_PATERNO"]) + ' ' +
                                        Convert.ToString(reader["AP_MATERNO"]),
                        Cantidad = Convert.ToInt32(reader["CANTIDAD"]),
                        LineNumber = i,
                    };
                    i++;
                }
            }
        }
    }
}
