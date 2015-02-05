using SAB.Base.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Acquisition;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
namespace SAB.Infraestructure.Acquisition
{
    public class PurchaseRequestDetailRepository : IPurchaseRequestDetailRepository
    {
        public void Insert(PurchaseRequestDetail entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(PurchaseRequestDetail entity)
        {
            throw new NotImplementedException();
        }

        public PurchaseRequestDetail QueryById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PurchaseRequestDetail entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PurchaseRequestDetail> QueryAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PurchaseRequestDetail> QueryByRequest(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int i = 1;
            using (IDataReader reader = database.ExecuteReader("dbo.PurchaseRequestDetail_QueryByRequest", id))
            {
                while (reader.Read())
                {

                    yield return new PurchaseRequestDetail
                    {
                        IdPublication = Convert.ToInt32(reader["ID_PUBLICACION"]),
                        IdPurchaseRequest = id,
                        PublicationName = Convert.ToString(reader["TITULO"]),
                        Cantidad = Convert.ToInt32(reader["CANTIDAD"]),
                        LineNumber = i,
                    };
                    i++;
                }
            }
        }


        public IEnumerable<PurchaseRequestDetailN> QueryByRequestN(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int i = 1;
            using (IDataReader reader = database.ExecuteReader("dbo.PurchaseRequestDetailN_QueryByRequest", id))
            {
                while (reader.Read())
                {
                    
                    yield return new PurchaseRequestDetailN
                    {
                        ISBN = Convert.ToString(reader["ISBN"]),
                        IdPurchaseRequest = id,
                        PublicationName = Convert.ToString(reader["TITULO"]),
                        Proveedor = Convert.ToString(reader["PROVEEDOR"]),
                        Precio = Convert.ToString(reader["PRECIOT"]),
                        LineNumber =i
                    };
                    i++;
                }
            }
        }
    }
}
