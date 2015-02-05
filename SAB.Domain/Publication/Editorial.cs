using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Publication
{
    public class Editorial
    {
        public int Id { get; set; }
        public string RUC { get; set; }
        public string Company_Name { get; set; }
        public DateTime RegisterDate { get; set; }
        public string State { get; set; }
    }
}
