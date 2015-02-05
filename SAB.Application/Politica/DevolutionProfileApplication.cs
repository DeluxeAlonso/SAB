using SAB.Base.Politica;
using SAB.Domain.Politica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Politica
{
    public class DevolutionProfileApplication
    {
         private readonly IDevolutionProfileRepository devolutionProfileRepository;

         public DevolutionProfileApplication(IDevolutionProfileRepository devolutionProfileRepository)
        {
            this.devolutionProfileRepository = devolutionProfileRepository;
        }

        


             public IEnumerable<DevolutionProfile> QueryByIdPerfil(int id)
        {
            IEnumerable<DevolutionProfile> _devolutionProfileList = null;
            try
            {
                _devolutionProfileList = devolutionProfileRepository.QueryByIdPerfil(id);
            }
            catch (Exception)
            {

            }

            return _devolutionProfileList;
        }
    }
}
