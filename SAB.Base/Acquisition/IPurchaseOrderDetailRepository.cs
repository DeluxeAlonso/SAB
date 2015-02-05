using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Acquisition;

namespace SAB.Base.Acquisition
{
    public interface IPurchaseOrderDetailRepository : IRepository<PurchaseOrderDetail>
    {
        IEnumerable<PurchaseOrderDetail> QueryByOrder(int id);
        IEnumerable<PurchaseOrderDetail> QueryByDate(int biblioteca, DateTime fechaD, DateTime fechaH);
    }
}
