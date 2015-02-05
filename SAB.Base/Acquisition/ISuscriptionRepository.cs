using SAB.Domain.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.Acquisition
{
    public interface ISuscriptionRepository:IRepository<Suscription>
    {
        IEnumerable<Suscription> Search(Suscription parametros, string titulo);
    }
}
