using SAB.Domain.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.Assets
{
    public interface ITypeAssetRepository:IRepository<TypeAsset>
    {
        IEnumerable<TypeAsset> Search(int id, string name);
    }
}
