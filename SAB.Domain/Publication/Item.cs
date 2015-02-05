using SAB.Domain.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Publication
{
    public class Item
    {
        public int Id { get; set; }
        public string State { get; set; }
        public Local Local { get; set; }
        public Assets Assets { get; set; }         
    }
}
