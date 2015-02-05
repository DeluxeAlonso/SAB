using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SAB.Domain.Publication
{
    public class PublicationTitle
    {
        public int Id { get; set; }

        [DataType(DataType.Text, ErrorMessage = "El campo solo acepta números de 13 dígitos")]
        [Range(1000000000000, 9999999999999, ErrorMessage = "El campo solo acepta números de 13 dígitos")]
        public string ISBN { get; set; }

        
        [Required(ErrorMessage = "El campo Titulo es obligatorio")]
        [StringLength(30, ErrorMessage = "Solo se puede ingresar máximo 30 caracteres")]
        public string Title { get; set; }


        [StringLength(100, ErrorMessage = "Solo se puede ingresar máximo 30 caracteres")]
        public string Description { get; set; }


        [Required(ErrorMessage = "El campo Año de Publicación es obligatorio")]
        [DataType(DataType.Text, ErrorMessage = "El campo solo acepta un número")]
        [Range(1500, 2014, ErrorMessage = "El campo solo acepta desde el año 1500")]
        public int Year_Publication { get; set; }


        [StringLength(100, ErrorMessage = "Solo se puede ingresar máximo 30 caracteres")]
        public string Imprint { get; set; }

        public string State { get; set; }

        public string Front { get; set; }

        [Required(ErrorMessage = "El campo Tipo de Publicación es obligatorio")]
        [DataType(DataType.Text, ErrorMessage = "El campo Tipo de Publicación es obligatorio")]
        public int Id_Type { get; set; }

        [Required(ErrorMessage = "El campo Editorial es obligatorio")]
        [Range(1, 9999, ErrorMessage = "El campo Editorial es obligatorio")]
        public int Id_Editorial { get; set; }

        [Required(ErrorMessage = "El campo Autor es obligatorio")]
        [Range(1, 9999, ErrorMessage = "El campo Editorial es obligatorio")]
        public int Id_Author { get; set; }


        public string typeImage { get; set; }
        public string nameType { get; set; }
        public string nameEditorial { get; set; }
        public string nameAuthor { get; set; }
        public int providerId { get; set; }

        public IEnumerable<PublicationItem> ItemList { get; set; }
        public PublicationType Type { get; set; }
        

    }
}
