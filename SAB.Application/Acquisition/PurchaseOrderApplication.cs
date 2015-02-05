using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;

namespace SAB.Application.Acquisition
{
    public class PurchaseOrderApplication
    {
        private readonly IPurchaseOrderRepository purchaseOrderRepository;

        public PurchaseOrderApplication(IPurchaseOrderRepository purchaseOrderRepository)
        {
            this.purchaseOrderRepository = purchaseOrderRepository;
        }

        public PurchaseOrder QueryById(int id)
        {
            PurchaseOrder _purchaseOrder = null;
            try
            {
                _purchaseOrder = purchaseOrderRepository.QueryById(id);
            }
            catch (Exception)
            {

            }

            return _purchaseOrder;
        }

        public IEnumerable<PurchaseOrder> QueryAll()
        {
            IEnumerable<PurchaseOrder> _purchaseOrderList = null;
            try
            {
                _purchaseOrderList = purchaseOrderRepository.QueryAll();
            }
            catch (Exception)
            {

            }

            return _purchaseOrderList;
        }


        public IEnumerable<PurchaseOrder> Search(int id, DateTime fechaD, DateTime fechaH, string state, string proveedor,ref int pageIndex)
        {
            IEnumerable<PurchaseOrder> _purchaseOrderList = null;
            try
            {
                _purchaseOrderList = purchaseOrderRepository.Search(id, fechaD, fechaH, state, proveedor);
                //var query = _purchaseOrderList.AsQueryable();                
                int _pageSize =  10;
                int _totalRecords = _purchaseOrderList.Count();
                int _totalPages = (int)Math.Ceiling((decimal)_totalRecords / (decimal)_pageSize);
                if (pageIndex < 1) pageIndex = 1;
                if (pageIndex > _totalPages && _totalPages != 0) pageIndex = _totalPages;
                //IEnumerable<PurchaseOrder> list = _purchaseOrderList.ToList();
                _purchaseOrderList = _purchaseOrderList.
                    Skip((pageIndex - 1) * _pageSize).
                    Take(_pageSize);
                
            }
            catch (Exception)
            {

            }

            return _purchaseOrderList;
        }

        public void Insert(PurchaseOrder purchaseOrder)
        {
            try
            {
                purchaseOrderRepository.Insert(purchaseOrder);
            }
            catch (Exception)
            {

            }
        }
        public void Insert(string idproveedor, string[] publicaciones, string[] cantidades)
        {
            try
            {
                purchaseOrderRepository.Insert(idproveedor,publicaciones,cantidades);
            }
            catch (Exception)
            {

            }
        }

        public void Delete(int id)
        {
            try
            {
                purchaseOrderRepository.Delete(id);
            }
            catch (Exception)
            {

            }
        }

        public void Approve(int id)
        {
            try
            {
                purchaseOrderRepository.Approve(id);
            }
            catch (Exception)
            {

            }
        }

        public void Reject(int id)
        {
            try
            {
                purchaseOrderRepository.Reject(id);
            }
            catch (Exception)
            {

            }
        }

        public void ReceptionUpdate(string[] detalles, string[] idPublicaciones, string[] cantidadesAntes, string[] cantidadesPlus)
        {
            try
            {
                purchaseOrderRepository.Reception(detalles,idPublicaciones,cantidadesAntes,cantidadesPlus);
            }
            catch (Exception)
            {

            }
        }




    }
}
