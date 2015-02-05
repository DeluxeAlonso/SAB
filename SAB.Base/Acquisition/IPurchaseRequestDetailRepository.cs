using SAB.Domain.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.Acquisition
{
    public interface IPurchaseRequestDetailRepository:IRepository<PurchaseRequestDetail>
    {
        IEnumerable<PurchaseRequestDetail> QueryByRequest(int id);

        IEnumerable<PurchaseRequestDetailN> QueryByRequestN(int id);
    }
}
