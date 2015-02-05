using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Sanctions
{
    public class SanctionType
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Unidad { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}
