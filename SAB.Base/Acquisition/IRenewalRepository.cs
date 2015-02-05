using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Acquisition;

namespace SAB.Base.Acquisition
{
    public interface IRenewalRepository:IRepository<Renewal>
    {
        IEnumerable<Renewal> QueryAll(int id_suscripcion);
        void Caducar(Renewal entity);
    }
}
