using SAB.Domain.Publication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Acquisition
{
    public class Reception
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<PublicationItem> List_Item { get; set; }
    }
}
