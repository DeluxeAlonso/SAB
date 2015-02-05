using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base.Publication;
using SAB.Domain.Publication;

namespace SAB.Application.Publication
{
    public class EditorialApplication
    {
        /***************************************************************************************/

        private readonly IEditorialRepository editorialRepository;

        /***************************************************************************************/

        public EditorialApplication(IEditorialRepository editorialRepository)
        {
            this.editorialRepository = editorialRepository;
        }

        /***************************************************************************************/

        public IEnumerable<Editorial> QueryAll()
        {
            IEnumerable<Editorial> _EditorialList = null;
            try
            {
                _EditorialList = editorialRepository.QueryAll();
            }
            catch (Exception)
            {

            }

            return _EditorialList;
        }

        /***************************************************************************************/

        public Editorial QueryById(int id)
        {
            Editorial _Editorial = null;
            try
            {
                _Editorial = editorialRepository.QueryById(id);
            }
            catch (Exception)
            {

            }

            return _Editorial;
        }

        /***************************************************************************************/

        public IEnumerable<Editorial> Search(int codigo, string razonSocial, DateTime fechaInicio, DateTime fechaFin)
        {
            IEnumerable<Editorial> _EditorialList = null;
            try
            {
                _EditorialList = editorialRepository.Search(codigo, razonSocial, fechaInicio, fechaFin);
            }
            catch (Exception)
            {

            }

            return _EditorialList;
        }

        /***************************************************************************************/

        public void Insert(Editorial editorial)
        {
            try
            {
                editorialRepository.Insert(editorial);
            }
            catch (Exception)
            {

            }
        }

        /***************************************************************************************/

        public void Update(Editorial editorial)
        {
            try
            {
                editorialRepository.Update(editorial);
            }
            catch (Exception)
            {

            }
        }

        /***************************************************************************************/

        public int Delete(Editorial editorial)
        {
            int resultado = 0;

            try
            {
                resultado = editorialRepository.Delete(editorial);
            }
            catch (Exception)
            {

            }

            return resultado;
        }

        /***************************************************************************************/

    }
}
