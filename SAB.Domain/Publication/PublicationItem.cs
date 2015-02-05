using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;
using System.ComponentModel.DataAnnotations;
using SAB.Domain.Library;

namespace SAB.Domain.Publication
{
    public class PublicationItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Estado es obligatorio")]

        public string Estado { get; set; }

        public int Id_Activo { get; set; }

        public int Id_Publication { get; set; }

        [Required(ErrorMessage = "El campo Biblioteca es obligatorio")]
        public int Id_Biblioteca { get; set; }

        public int Id_Recepcion { get; set; }

        public PublicationTitle Publication { get; set; }

        public Local Biblioteca { get; set; }

        public DateTime FechaRecepcion { get; set; }
    }
}
