using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Publication
{
    public class PublicationType
    {
        
        public int Id { get; set; }

        //[DataType(DataType.Text,ErrorMessage="El campo solo acepta letras")]
        [Required(ErrorMessage="El campo nombre es obligatorio")]
        //[StringLength(30,ErrorMessage="Solo se puede ingresar máximo 30 caracteres")]
        public string Name { get; set; }

        //[StringLength(100,ErrorMessage="Solo se puede ingresar máximo 30 caracteres")]
        public string Description { get; set; }
    }
}
