using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SAB.Domain.Publication;

namespace SAB.Domain.Acquisition
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public Supplier PurchaseOrder_Provider { get; set; }

        public List<PurchaseRequest> List_RequestOrder { get; set; }

        public List<PurchaseOrderDetail> List_DetailPublication { get; set; }
        
        public Reception PurchaseOrder_Reception { get; set; }
        public string State { get; set; }
    }
}
