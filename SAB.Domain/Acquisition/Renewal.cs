using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Acquisition
{
    public class Renewal
    {
        public int Id { get; set; }
        public int Id_Suscription { get; set; }

        public int Amount { get; set; }

        [Display(Name="Fecha de inicio")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime),"01/01/1900","31/12/9999",ErrorMessage="La fecha de inicio debe estar entre 01/01/1900 y 31/12/9999")]
        public DateTime Start_date { get; set; }

        [Display(Name = "Fecha de fin")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "01/01/1900", "31/12/9999", ErrorMessage = "La fecha de fin debe estar entre 01/01/1900 y 31/12/9999")]
        public DateTime End_date { get; set; }
        public string Frequency { get; set; }

        public double? Cost { get; set; }

        public string State { get; set; }
    }
}
