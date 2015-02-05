using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Publication
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string First_last_Name { get; set; }
        public string Second_last_Name { get; set; }
        public string Country { get; set; }
        public string Hometown { get; set; }
    }
}
