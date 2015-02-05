using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;

namespace SAB.Base.Publication
{
    public interface IPublicationTitleRepository : IRepository<PublicationTitle>
    {

        IEnumerable<PublicationTitle> Search(PublicationTitle entity);

        IEnumerable<PublicationTitle> Search(int id, int id_tipo, int id_editorial, int id_autor, string autor, string editorial);
        IEnumerable<PublicationTitle> SearchPublicationsByFilters(PublicationTitle tableRow);       
        IEnumerable<PublicationTitle> Search2(string searchText);
        IEnumerable<PublicationTitle> Search3(string autor, string titulo, string editorial, int? anio, int? tipoPublicacion);
        bool PublicationExist(int id);

    }
}
