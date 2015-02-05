using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Acquisition
{
    public class PurchaseRequestDetailApplication
    {

        private readonly IPurchaseRequestDetailRepository purchaseRequestDetailRepository;

        public  PurchaseRequestDetailApplication(IPurchaseRequestDetailRepository PurchaseRequestDetailRepository)
        {
            this.purchaseRequestDetailRepository = PurchaseRequestDetailRepository;

        }


        public void Insert(PurchaseRequestDetail purchaseRequestDetail)
        {
            try
            {
                purchaseRequestDetailRepository.Insert(purchaseRequestDetail);
            }
            catch (Exception)
            {

            }
        }

        public IEnumerable<PurchaseRequestDetail> QueryByRequest(int id)
        {
            IEnumerable<PurchaseRequestDetail> _purchaseRequestDetailList = null;
            try
            {
                _purchaseRequestDetailList = purchaseRequestDetailRepository.QueryByRequest(id);
            }
            catch (Exception)
            {

            }

            return _purchaseRequestDetailList;
        }

        public IEnumerable<PurchaseRequestDetailN> QueryByRequestN(int id)
        {
            IEnumerable<PurchaseRequestDetailN> _purchaseRequestDetailList = null;
            try
            {
                _purchaseRequestDetailList = purchaseRequestDetailRepository.QueryByRequestN(id);
            }
            catch (Exception)
            {

            }

            return _purchaseRequestDetailList;
        }

    }
}
