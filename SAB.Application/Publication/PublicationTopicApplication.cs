using SAB.Base.Publication;
using SAB.Domain.Publication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Publication
{
    public class PublicationTopicApplication
    {
        /***************************************************************************************/

        private readonly IPublicationTopicRepository publicationTopicRepository;

        /***************************************************************************************/

        public PublicationTopicApplication(IPublicationTopicRepository publicationTopicRepository)
        {
            this.publicationTopicRepository = publicationTopicRepository;
        }

        /***************************************************************************************/

        public IEnumerable<PublicationTopic> QueryAll()
        {
            IEnumerable<PublicationTopic> _publicationTopicList = null;
            try
            {
                _publicationTopicList = publicationTopicRepository.QueryAll();
            }
            catch (Exception)
            {
            }

            return _publicationTopicList;
        }

        /***************************************************************************************/

        public PublicationTopic QueryById(int id)
        {
            PublicationTopic _publicationTopic = null;
            try
            {
                _publicationTopic = publicationTopicRepository.QueryById(id);
            }
            catch (Exception)
            {
            }

            return _publicationTopic;
        }

        /***************************************************************************************/

        public IEnumerable<Topic> Search(int idPublucation)
        {
            IEnumerable<Topic> _topicList = null;
            try
            {
                _topicList = publicationTopicRepository.Search(idPublucation);
            }
            catch (Exception) 
            { 
            }

            return _topicList;
        }

        /***************************************************************************************/

        public void Insert(PublicationTopic publicationTopic)
        {
            try
            {
                publicationTopicRepository.Insert(publicationTopic);
            }
            catch (Exception)
            {
            }
        }

        /***************************************************************************************/

        public int Delete(PublicationTopic publicationTopic)
        {
            int resultado = 0;
            try
            {
                resultado = publicationTopicRepository.Delete(publicationTopic);
            }
            catch (Exception)
            {
            }
            return resultado;
        }

        /***************************************************************************************/

        public void Update(PublicationTopic entity)
        {
            try
            {
                publicationTopicRepository.Update(entity);
            }
            catch (Exception)
            {
            }
        }

        /***************************************************************************************/
    }
}
