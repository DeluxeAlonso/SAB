using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SAB.Domain.Politica
{
    public class UserProfile

    {
        public int Id { get; set; }

        public int IdTipoUsuario { get; set; }
        public string Name { get;set ;}

       
        public string Description { get; set; }
       
        public int MaxMaterial { get; set; }
       

        public int MaxDays { get; set; }

        public string Estado { get; set; }

    }
}
