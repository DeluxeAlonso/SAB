using SAB.Domain.Publication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.Publication
{
    public interface IPublicationItemRepository:IRepository<PublicationItem>
    {
        IEnumerable<PublicationItem> QueryByPublication(int idPublication, int idBiblioteca);
        IEnumerable<PublicationItem> QueryByPublication_Biblioteca(int idPublication, int idBiblioteca);
        IEnumerable<PublicationItem> QueryByState(int idPublication);

        void Insert_Donacion(PublicationItem publication);

        void Insert_Donacion_Activo(PublicationItem publication);
    }
}
