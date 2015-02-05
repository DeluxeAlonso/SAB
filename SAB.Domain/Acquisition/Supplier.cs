using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Acquisition
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public string Status { get;set; }
        public string Ruc { get; set; }

        public string Email { get;set; }

        public string Contacto { get; set; }

        public string Address { get; set; }

        public DateTime RegisterDate { get; set; }

        public string Telephone { get; set; }


    }
}
