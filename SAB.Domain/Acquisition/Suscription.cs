using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;

namespace SAB.Domain.Acquisition
{
    public class Suscription
    {
        public int Id { get; set; }
        public DateTime RegTime { get; set; }
        public int Id_Editorial { get; set; }
        public int Id_TypePublication { get; set; }
        public string state { get; set; }
        public string description { get; set; }
        public int Id_Publication { get; set; }
        
    }
}
