using SAB.Domain.Publication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.Publication
{
    public interface IPublicationTopicRepository : IRepository<PublicationTopic>
    {
        IEnumerable<Topic> Search(int idPublucation);
    }
}
