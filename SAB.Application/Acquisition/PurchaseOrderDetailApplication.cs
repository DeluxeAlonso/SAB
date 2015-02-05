using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;


namespace SAB.Application.Acquisition
{
    public class PurchaseOrderDetailApplication
    {
        private readonly IPurchaseOrderDetailRepository purchaseOrderDetailRepository;

        public PurchaseOrderDetailApplication(IPurchaseOrderDetailRepository purchaseOrderDetailRepository)
        {
            this.purchaseOrderDetailRepository = purchaseOrderDetailRepository;
        }

        public IEnumerable<PurchaseOrderDetail> QueryAll()
        {
            IEnumerable<PurchaseOrderDetail> _purchaseOrderDetailList = null;
            try
            {
                _purchaseOrderDetailList = purchaseOrderDetailRepository.QueryAll();
            }
            catch (Exception)
            {

            }

            return _purchaseOrderDetailList;
        }

        public IEnumerable<PurchaseOrderDetail> QueryByOrder(int id)
        {
            IEnumerable<PurchaseOrderDetail> _purchaseOrderDetailList = null;
            try
            {
                _purchaseOrderDetailList = purchaseOrderDetailRepository.QueryByOrder(id);
            }
            catch (Exception)
            {

            }

            return _purchaseOrderDetailList;
        }

        public IEnumerable<PurchaseOrderDetail> QueryByDate(int biblioteca,DateTime fechaD, DateTime fechaH)
        {
            IEnumerable<PurchaseOrderDetail> _purchaseOrderDetailList = null;
            try
            {
                _purchaseOrderDetailList = purchaseOrderDetailRepository.QueryByDate(biblioteca,fechaD, fechaH);
            }
            catch (Exception)
            {

            }

            return _purchaseOrderDetailList;
        }


        public void Insert(PurchaseOrderDetail purchaseOrderDetail)
        {
            try
            {
                purchaseOrderDetailRepository.Insert(purchaseOrderDetail);
            }
            catch (Exception)
            {

            }
        }

        
        
    }
}
