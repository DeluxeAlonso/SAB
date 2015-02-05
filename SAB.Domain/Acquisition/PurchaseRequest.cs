using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Acquisition
{
    public class PurchaseRequest
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Estado { get; set; }
        public int IdRequestType { get; set; }
        public int IdPurchaseOrder { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public int IdProvider { get; set; }

        public string ProviderName { get; set; }
    }
}
