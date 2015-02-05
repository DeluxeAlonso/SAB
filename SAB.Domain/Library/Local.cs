using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SAB.Domain.Library
{
    public class Local
    {
        public int Id { get; set; }

        [Required(ErrorMessage="El campo Nombre es obligatorio.")]
        [StringLength(50,ErrorMessage="Ingrese máximo 50 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Fecha de apertura es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime Opening_Date { get; set; }

        [Required(ErrorMessage = "El campo Distrito es obligatorio.")]
        [StringLength(50, ErrorMessage = "Ingrese máximo 50 caracteres.")]
        public string Distric { get; set; }

        [Required(ErrorMessage = "El campo Ciudad es obligatorio.")]
        [StringLength(50, ErrorMessage = "Ingrese máximo 50 caracteres.")]
        public string City { get; set; }

        [Required(ErrorMessage = "El campo Dirección es obligatorio.")]
        [StringLength(100, ErrorMessage = "Ingrese máximo 100 caracteres.")]
        public string Address { get; set; }

        [StringLength(20, ErrorMessage = "Ingrese máximo 20 caracteres.")]
        public string Phone { get; set; }
        
        //[EmailAddress(ErrorMessage="Ingrese una dirección de correo electrónico válida.")]
        [StringLength(50, ErrorMessage = "Ingrese máximo 50 caracteres.")]
        public string Mail { get; set; }
    }
}
