using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Acquisition
{
    public class PurchaseRequestDetailN
    {
        public int IdPurchaseRequest { get; set; }
        public string ISBN { get; set; }

        public string PublicationName { get; set; }
        public string Proveedor { get; set; }
        public string Precio { get; set; }
        public int LineNumber { get; set; }
    }
}
