using SAB.Domain.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Assets
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public int IdAssetType { get; set; }
        public DateTime RegistryDate { get; set; }
        public int Location { get; set; }
        public Local Biblioteca { get; set; }
        public TypeAsset TypeAsset { get; set; }

        public int Quantity { get; set; }
    }
}
