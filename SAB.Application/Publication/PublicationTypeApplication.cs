using SAB.Base.Publication;
using SAB.Domain.Publication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Publication
{
    public class PublicationTypeApplication
    {
        private readonly IPublicationTypeRepository publicationTypeRepository;


        public PublicationTypeApplication(IPublicationTypeRepository publicationTypeRepository)
        {
            this.publicationTypeRepository = publicationTypeRepository;
        }

        public IEnumerable<PublicationType> QueryAllByIdPerfil(int id)
        {
            IEnumerable<PublicationType> _publicationTypeList = null;
            try
            {
                _publicationTypeList = publicationTypeRepository.queryAllByIdPerfil(id);
            }
            catch (Exception)
            {

            }
            return _publicationTypeList;
        }

        public IEnumerable<PublicationType> QueryAll()
        {
            IEnumerable<PublicationType> _publicationTypeList = null;
            try
            {
                _publicationTypeList = publicationTypeRepository.QueryAll();
            }
            catch (Exception)
            {

            }

            return _publicationTypeList;
        }

        public PublicationType QueryById(int id)
        {
            PublicationType _publicationType = null;
            try
            {
                _publicationType = publicationTypeRepository.QueryById(id);
            }
            catch (Exception)
            {

            }

            return _publicationType;
        }

        public void Insert(PublicationType publicationType)
        {
            try
            {
                publicationTypeRepository.Insert(publicationType);
            }
            catch (Exception)
            {

            }
        }

        public void Update(PublicationType publicationType)
        {
            try
            {
                publicationTypeRepository.Update(publicationType);
            }
            catch (Exception)
            {

            }
        }

        public void Delete(PublicationType publicationType)
        {
            try
            {
                publicationTypeRepository.Delete(publicationType);
            }
            catch (Exception)
            {

            }
        }

        public void DeleteAllByIdPerfil(int id)
        {
            try
            {
                publicationTypeRepository.DeleteById(id);
            }
            catch (Exception)
            { }


        }

        public IEnumerable<PublicationType> Search(int codigo, string nombre)
        {
            IEnumerable<PublicationType> _publicationTypeList = null;
            try
            {
                _publicationTypeList = publicationTypeRepository.Search(codigo, nombre);
            }
            catch (Exception)
            {

            }

            return _publicationTypeList;
        }
    }
}
             
