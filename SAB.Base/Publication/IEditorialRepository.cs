using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;

namespace SAB.Base.Publication
{
    public interface IEditorialRepository : IRepository<Editorial>
    {
        IEnumerable<Editorial> Search(int codigo, string razonSocial, DateTime fechaInicio, DateTime fechaFin);
    }
}
