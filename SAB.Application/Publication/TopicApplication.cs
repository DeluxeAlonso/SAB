using SAB.Base.Publication;
using SAB.Domain.Publication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Publication
{
    public class TopicApplication
    {
        /***************************************************************************************/

        private readonly ITopicRepository topicRepository;

        /***************************************************************************************/

        public TopicApplication(ITopicRepository topicRepository)
        {
            this.topicRepository = topicRepository;
        }

        /***************************************************************************************/

        public IEnumerable<Topic> QueryAll()
        {
            IEnumerable<Topic> _topicList = null;
            try
            {
                _topicList = topicRepository.QueryAll();
            }
            catch (Exception)
            {

            }

            return _topicList;
        }

        /***************************************************************************************/

        public IEnumerable<Topic> Search(int id, string name)
        {
            IEnumerable<Topic> _topicList = null;
            try
            {
                _topicList = topicRepository.Search(id, name);
            }
            catch (Exception)
            {

            }
            return _topicList;
        }

        /***************************************************************************************/

        public Topic QueryById(int id)
        {
            Topic topic = null;
            try
            {
                topic = topicRepository.QueryById(id);
            }
            catch (Exception)
            {

            }

            return topic;
        }

        /***************************************************************************************/

        public void Insert(Topic topic)
        {
            try
            {
                topicRepository.Insert(topic);
            }
            catch (Exception)
            {

            }
        }

        /***************************************************************************************/

        public void Update(Topic topic)
        {
            try
            {
                topicRepository.Update(topic);
            }
            catch (Exception)
            {

            }
        }

        /***************************************************************************************/

        public int Delete(Topic topic)
        {
            int resultado = 0;

            try
            {
                resultado = topicRepository.Delete(topic);
            }
            catch (Exception)
            {

            }

            return resultado;
        }

        /***************************************************************************************/
    }
}
