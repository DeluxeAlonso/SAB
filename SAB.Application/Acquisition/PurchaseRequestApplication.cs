using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Acquisition
{
    public class PurchaseRequestApplication
    {
        private readonly IPurchaseRequestRepository purchaseRequestRepository;

        public  PurchaseRequestApplication(IPurchaseRequestRepository purchaseRequestRepository)
        {
            this.purchaseRequestRepository = purchaseRequestRepository;

        }

        public void Delete(int id)
        {
            try
            {
                purchaseRequestRepository.Delete(id);
            }
            catch (Exception)
            {

            }
        }

        public PurchaseRequest QueryById(int id)
        {
            PurchaseRequest p = null;
            try
            {
                p = purchaseRequestRepository.QueryById(id);
            }
            catch (Exception)
            {

            }
            return p;
        }

        public IEnumerable<PurchaseRequest> QueryAll()
        {
            IEnumerable<PurchaseRequest> _purchaseRequestList = null;
            try
            {
                _purchaseRequestList = purchaseRequestRepository.QueryAll();
            }
            catch (Exception)
            {

            }

            return _purchaseRequestList;
        }



        public void UpdateById(int id, int idorder)
        {
            
            try
            {
                purchaseRequestRepository.UpdateById(id,idorder);
            }
            catch (Exception)
            {

            }

        }

        public IEnumerable<PurchaseRequest> Search(int id, DateTime fechaD, DateTime fechaH, int idProveedor,int idSolicitante, string state)
        {
            IEnumerable<PurchaseRequest> _purchaseRequestList = null;
            try
            {
                _purchaseRequestList = purchaseRequestRepository.Search(id, fechaD, fechaH,idProveedor,idSolicitante, state);
            }
            catch (Exception)
            {

            }

            return _purchaseRequestList;
        }

        public IEnumerable<PurchaseRequest> Search2(int id, DateTime fechaD, DateTime fechaH, string estado, string solicitante)
        {
            IEnumerable<PurchaseRequest> _purchaseRequestList = null;
            try
            {
                _purchaseRequestList = purchaseRequestRepository.Search2(id, fechaD, fechaH, estado, solicitante);
            }
            catch (Exception)
            {

            }

            return _purchaseRequestList;
        }
         

        public IEnumerable<PurchaseRequest> QueryByOrder(int id)
        {
            IEnumerable<PurchaseRequest> _purchaseRequestList = null;
            try
            {
                _purchaseRequestList = purchaseRequestRepository.QueryByOrder(id);
            }
            catch (Exception)
            {

            }

            return _purchaseRequestList;
        }


        public void Insert(string[] publicaciones, string[] cantidades , string descripcion,int id)
        {
            try
            {
                purchaseRequestRepository.Insert(publicaciones,cantidades,descripcion,id);
            }
            catch (Exception)
            {

            }
        }


        public void Approve(int id)
        {
            try
            {
                purchaseRequestRepository.Approve(id);
            }
            catch (Exception)
            {

            }
        }

        public void Reject(int id)
        {
            try
            {
                purchaseRequestRepository.Reject(id);
            }
            catch (Exception)
            {

            }
        }
        public void InsertN(string []isbn, string[] titulo, string[] editorial, string []proveedor, string[] precio, int id)
        {
            try
            {
                purchaseRequestRepository.InsertN(isbn,titulo,proveedor,precio,id);
            }
            catch (Exception)
            {

            }
        }
         
    }
}
