using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Library;

namespace SAB.Base.Library
{
    public interface ILocalRepository:IRepository<Local>
    {
        void Delete(int id);
        IEnumerable<Local> Search(Local parametrosBusqueda);
    }
}
