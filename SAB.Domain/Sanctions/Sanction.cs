using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;
using SAB.Domain.Loan;
using SAB.Domain.User;

namespace SAB.Domain.Sanctions
{
    public class Sanction
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Id_Prestamo { get; set; }
        public int Tipo_Sancion { get; set; }
        public int Id_User { get; set; }
        public int Detalle { get; set; }
    }
}
