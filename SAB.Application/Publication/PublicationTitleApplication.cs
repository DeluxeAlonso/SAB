using SAB.Base.Publication;
using SAB.Domain.Publication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Publication
{
    public class PublicationTitleApplication
    {
        /***************************************************************************************/

        private readonly IPublicationTitleRepository publicationTitleRepository;

        /***************************************************************************************/

        public PublicationTitleApplication(IPublicationTitleRepository publicationTitleRepository)
        {
            this.publicationTitleRepository = publicationTitleRepository;
        }

        /***************************************************************************************/

        public IEnumerable<PublicationTitle> QueryAll()
        {
            IEnumerable<PublicationTitle> _publicationTileList = null;
            try
            {
                _publicationTileList = publicationTitleRepository.QueryAll();
            }
            catch (Exception)
            {

            }

            return _publicationTileList;
        }

        /***************************************************************************************/

        public IEnumerable<PublicationTitle> Search(PublicationTitle entity) 
        {
            IEnumerable<PublicationTitle> _publicationTileList = null;
            try
            {
                _publicationTileList = publicationTitleRepository.Search(entity);
            }
            catch (Exception)
            {

            }

            return _publicationTileList;
        }

        /***************************************************************************************/

        public IEnumerable<PublicationTitle> Search(int id, int id_tipo, int id_editorial, int id_autor, string autor, string editorial )
        {
            IEnumerable<PublicationTitle> _publicationTileList = null;
            try
            {
                _publicationTileList = publicationTitleRepository.Search(id, id_tipo, id_editorial, id_autor, autor, editorial );
            }
            catch (Exception)
            {

            }

            return _publicationTileList;
        }

        /***************************************************************************************/

        public void Insert(PublicationTitle publicationTitle)
        {
            try
            {
                publicationTitleRepository.Insert(publicationTitle);
            }
            catch (Exception)
            {

            }
        }

        /***************************************************************************************/

        public void Update(PublicationTitle publicationTitle)
        {
            try
            {
                publicationTitleRepository.Update(publicationTitle);
            }
            catch (Exception)
            {

            }
        }

        /***************************************************************************************/

        public PublicationTitle QueryById(int id)
        {
            PublicationTitle _publicationTile = null;
            try
            {
                _publicationTile = publicationTitleRepository.QueryById(id);
            }
            catch (Exception)
            {

            }
            return _publicationTile;
        }

        /***************************************************************************************/

        public int Delete(PublicationTitle publicationTitle)
        {
            int resultado = 0;
            try
            {
                resultado = publicationTitleRepository.Delete(publicationTitle);
            }
            catch (Exception)
            {

            }
            return resultado;
        }

        /***************************************************************************************/

        public IEnumerable<PublicationTitle> SearchPublicationsByFilters(PublicationTitle entity)
        {
            IEnumerable<PublicationTitle> _publicationTileList = null;
            try
            {
                _publicationTileList=publicationTitleRepository.SearchPublicationsByFilters(entity);
                
            }
            catch (Exception )
            {

            }
            return _publicationTileList;
        }

        /***************************************************************************************/

        public IEnumerable<PublicationTitle> Search2(string busqueda)
        {
            IEnumerable<PublicationTitle> _publicationTitleList = null;
            try
            {
                _publicationTitleList = publicationTitleRepository.Search2(busqueda);
            }
            catch (Exception) { }
            return _publicationTitleList;
        }

        /***************************************************************************************/

        public IEnumerable<PublicationTitle> Search3(string autor, string titulo, string editorial, int? anio, int? tipoPublicacion)
        {
            IEnumerable<PublicationTitle> _publicationTitleList = null;
            try
            {
                _publicationTitleList = publicationTitleRepository.Search3(autor, titulo, editorial, anio,tipoPublicacion);
            }
            catch (Exception) { }
            return _publicationTitleList;
        }

        /***************************************************************************************/

        public bool PublicationExist(int id)
        {
            bool _isPublication = true;
            try
            {
                _isPublication = publicationTitleRepository.PublicationExist(id);
            }
            catch (Exception)
            {

            }
            return _isPublication;
        }

        /***************************************************************************************/
       
    }
}
