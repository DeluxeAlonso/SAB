using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Acquisition
{
    public class SupplierApplication
    {


        private readonly ISupplierRepository supplierRepository;

        public SupplierApplication(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        public void Insert(Supplier supplier)
        {
            try
            {
                supplierRepository.Insert(supplier);
            }
            catch (Exception)
            {

            }
        }

        public IEnumerable<Supplier> QueryAll()
        {
            IEnumerable<Supplier> _supplirList = null;
            try
            {
                _supplirList = supplierRepository.QueryAll();
            }
            catch (Exception)
            {

            }

            return _supplirList;
        }



        public IEnumerable<Supplier> Search(string searchName, string searchCode, string from, string to, string searchContacto, string searchRUC)
        {
            IEnumerable<Supplier> _supplirList = null;
            try
            {
                _supplirList = supplierRepository.Search(searchName, searchCode, from, to, searchContacto, searchRUC);
            }
            catch (Exception)
            {

            }

            return _supplirList;
        }


        public Supplier QueryById(int id)
        {
            Supplier _supplir = null;
            try
            {
                _supplir = supplierRepository.QueryById(id);
            }
            catch (Exception)
            {

            }

            return _supplir;
        }




        public void Delete(int id)
        {
            try{
            Supplier u = this.QueryById(id);
            supplierRepository.Delete(u);}
             catch (Exception)
            {

            }
        }

        public void Actualiza(int id, string nombre, string contacto, string direccion, string telefono, string correo)
        {
            try
            {

                supplierRepository.Actualiza(id, nombre, contacto, direccion, telefono, correo);
            }
            catch (Exception)
            {

            }
        }
    }
}
