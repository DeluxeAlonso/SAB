using SAB.Domain.Library;
using SAB.Domain.Politica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.User
{
    public class UserAccount
    {
        public int Id { get; set; }
        public Local Biblioteca { get; set; }
        public string Username{get;set;}
        public string Password { get; set; }
        public int IdPerfil { get; set; }

        public string Lastname1 { get; set; }
        public string Lastname2 { get; set; }
        public string Name { get; set; }

        public string DNI { get; set; }

        public int IdBiblioteca { get; set; }

        public int IdTipoUsuario { get; set; }

        public string Correo { get; set; }

        public string Telephone { get; set; }
        public string Address { get; set; }

        public DateTime RegisterDate { get; set; }
        public string Status { get; set; }

        public Boolean IsLector { get; set; }

        public UserProfile TipoPerfil { get; set; }

        public int IdTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }

        public DocumentType Document { get; set; }
      
    }

    
}
