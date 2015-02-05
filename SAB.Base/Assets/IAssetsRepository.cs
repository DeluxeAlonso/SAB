using SAB.Domain.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.Assets
{
    public interface IAssetsRepository : IRepository<Asset>
    {
        void Delete(int local_id);

        IEnumerable<Asset> Search(int id,  DateTime fechaD, DateTime fechaH,int tipoActivo);

        
    }
}
