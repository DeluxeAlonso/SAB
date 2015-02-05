using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Publication
{
    public class Topic
    {
        // El estado solo es para la eliminacion logica

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
    }
}
