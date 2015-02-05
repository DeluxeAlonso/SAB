using SAB.Base.User;
using SAB.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.User
{
    public class DocumentTypeApplication
    {
        private readonly IDocumentTypeRepository documentTypeRepository;

        public DocumentTypeApplication(IDocumentTypeRepository documentTypeRepository)
        {
            this.documentTypeRepository = documentTypeRepository;
        }

        public IEnumerable<DocumentType> QueryAll()
        {

            IEnumerable<DocumentType> _list = null;
            try
            {
                _list = documentTypeRepository.QueryAll();
            }
            catch (Exception)
            {

            }
            return _list;
        }


        public DocumentType QueryById(int id)
        {

            DocumentType d = null;
            try
            {
                d = documentTypeRepository.QueryById(id);
            }
            catch (Exception)
            {

            }
            return d;
        }
    }
}
