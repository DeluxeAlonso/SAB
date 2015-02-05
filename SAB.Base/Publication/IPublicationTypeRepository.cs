using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;
namespace SAB.Base.Publication
{
    public interface IPublicationTypeRepository:IRepository<PublicationType>
    {
        List<PublicationType> queryAllByIdPerfil(int idPerfil);
        void DeleteById(int id);
        IEnumerable<PublicationType> Search(int codigo, string nombre);
    }
}
