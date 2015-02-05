using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Acquisition;

namespace SAB.Base.Acquisition
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        int Delete(int id);
        void Insert(string idproveedor, string[] publicaciones, string[] cantidades);

        void Reception(string[] detalles, string[] idPublicaciones, string[] cantidadesAntes, string[] cantidadesPlus);
        IEnumerable<PurchaseOrder> Search(int id, DateTime fechaD, DateTime fechaH, string state, string proveedor);

        void Approve(int id);
        void Reject(int id);
    }
}
