using SAB.Domain.Politica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.Politica
{
    public interface IDevolutionProfileRepository: IRepository<DevolutionProfile>
    {

        IEnumerable<DevolutionProfile> QueryByIdPerfil(int i);
    }
}
