using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.User
{
    public class DocumentType
    {
        public int Id { set; get; }
        public string Name { get; set; }
        public int IsNumerico { get; set; }
        public int DigitQuantity { get; set; }
    }
}
