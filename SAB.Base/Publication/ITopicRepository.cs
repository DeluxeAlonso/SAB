using SAB.Domain.Publication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.Publication
{
    public interface ITopicRepository : IRepository<Topic>
    {
        IEnumerable<Topic> Search(int id, string name);
    }
}
