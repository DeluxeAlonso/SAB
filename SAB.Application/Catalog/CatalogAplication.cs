using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base.Publication;
using SAB.Domain.Publication;


namespace SAB.Application.Catalog
{
    public class CatalogAplication
    {
         private readonly IPublicationTitleRepository publicationRepository;

         public CatalogAplication(IPublicationTitleRepository publicationRepository)
         {
                this.publicationRepository = publicationRepository;
         }

         public PublicationTitle find(int publication_id)
         {
             PublicationTitle _publication = null;

             try
             {
                 _publication = publicationRepository.QueryById(publication_id);
             }
             catch
             {
             }

             return _publication;

         }

    }
}
