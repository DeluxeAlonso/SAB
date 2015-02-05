using SAB.Base.Publication;
using SAB.Domain.Publication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Publication
{
    public class PublicationItemApplication
    {
        private readonly IPublicationItemRepository publicationItemRepository;

        /***************************************************************************************/

        public PublicationItemApplication(IPublicationItemRepository publicationItemRepository)
        {
            this.publicationItemRepository = publicationItemRepository;
        }

        /***************************************************************************************/

        public IEnumerable<PublicationItem> QueryAll()
        {
            IEnumerable<PublicationItem> _publicationItemList = null;

            try
            {
                _publicationItemList = publicationItemRepository.QueryAll();
            }
            catch (Exception)
            {

            }
            return _publicationItemList;
        }

        /***************************************************************************************/

        public IEnumerable<PublicationItem> QueryByPublication(int idPublication, int idBiblioteca)
        {
            IEnumerable<PublicationItem> _publicationItemList = null;

            try
            {
                _publicationItemList = publicationItemRepository.QueryByPublication(idPublication, idBiblioteca);
            }
            catch (Exception)
            {

            }
            return _publicationItemList;
        }

        /***************************************************************************************/

        public IEnumerable<PublicationItem> QueryByPublication_Biblioteca(int idPublication, int idBiblioteca)
        {
            IEnumerable<PublicationItem> _publicationList = null;

            try
            {
                _publicationList = publicationItemRepository.QueryByPublication_Biblioteca(idPublication, idBiblioteca);
            }
            catch (Exception)
            {

            }
            return _publicationList;
        }

        /***************************************************************************************/

        public IEnumerable<PublicationItem> QueryByState(int idPublication)
        {
            IEnumerable<PublicationItem> _publicationItemList = null;

            try
            {
                _publicationItemList = publicationItemRepository.QueryByState(idPublication);
            }
            catch (Exception)
            {

            }
            return _publicationItemList;
        }


        /***************************************************************************************/

        public void Insert(PublicationItem publicationItem)
        {
            try
            {
                publicationItemRepository.Insert(publicationItem);
            }
            catch (Exception)
            {

            }
        }

        /***************************************************************************************/

        public void Insert_Donacion(PublicationItem publicationItem)
        {
            try
            {
                publicationItemRepository.Insert_Donacion(publicationItem);
            }
            catch (Exception)
            {

            }
        }

        /***************************************************************************************/

        public void Insert_Donacion_Activo(PublicationItem publicationItem)
        {
            try
            {
                publicationItemRepository.Insert_Donacion_Activo(publicationItem);
            }
            catch (Exception)
            {

            }
        }

        /***************************************************************************************/

        public void Update(PublicationItem publicationItem)
        {
            try
            {
                publicationItemRepository.Update(publicationItem);
            }
            catch (Exception)
            {

            }
        }

        /***************************************************************************************/

        public int Delete(PublicationItem publicationItem)
        {
            int resultado = 0;
            try
            {
                publicationItemRepository.Delete(publicationItem);
            }
            catch (Exception)
            {

            }
            return resultado;
        }

        /***************************************************************************************/

        public PublicationItem QueryById(int id)
        {
            PublicationItem _publicationItem = null;
            try
            {
                _publicationItem = publicationItemRepository.QueryById(id);
            }
            catch(Exception)
            {

            }
            return _publicationItem;
        }

        /***************************************************************************************/

       


    }
}
