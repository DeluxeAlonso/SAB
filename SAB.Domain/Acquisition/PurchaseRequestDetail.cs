using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Acquisition
{
    public class PurchaseRequestDetail
    {
        public int IdPurchaseRequest { get; set; }
        public int IdPublication { get; set; }

        public string PublicationName { get; set; }
        public int Cantidad { get; set; }
        public int LineNumber { get; set; }
    }
}
