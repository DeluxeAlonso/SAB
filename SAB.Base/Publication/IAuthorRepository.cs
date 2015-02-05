using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;
namespace SAB.Base.Publication
{
    public interface IAuthorRepository: IRepository<Author>
    {
        IEnumerable<Author> Search(int id, string name, string country);
    }
}
