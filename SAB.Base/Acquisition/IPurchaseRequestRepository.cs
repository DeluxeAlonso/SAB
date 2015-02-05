using SAB.Domain.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.Acquisition
{
    public interface IPurchaseRequestRepository:IRepository<PurchaseRequest>
    {
        void UpdateById(int id,int idorder);
        IEnumerable<PurchaseRequest> QueryByOrder(int id);
        void Insert(string[] publicaciones, string[] cantidades, string descripcion,int id);

        void InsertN(string[] isbns,string[] titulos, string [] proveedores, string  [] precios, int id);
        
        IEnumerable<PurchaseRequest> Search(int id, DateTime fechaD, DateTime fechaH, int idProveedor, int Solicitante, string state);

        IEnumerable<PurchaseRequest> Search2(int id, DateTime fechaD, DateTime fechaH, string estado,string solicitante);

        int Delete(int id);

        void Approve(int id);
        void Reject(int id);


    }

}
