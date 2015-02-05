using SAB.Domain.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.Acquisition
{
    public interface ISupplierRepository:IRepository<Supplier>
    {

        IEnumerable<Supplier> Search(string searchName, string searchCode, string from, string to, string searchContacto, string searchRUC);

        void Actualiza(int id, string nombre, string contacto, string direccion, string telefono, string correo);
    }
}
