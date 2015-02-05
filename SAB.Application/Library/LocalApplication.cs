using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base.Library;
using SAB.Domain.Library;

namespace SAB.Application.Libray
{
    public class LocalApplication
    {
        private readonly ILocalRepository localRepository;

        public LocalApplication(ILocalRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public IEnumerable<Local> QueryAll()
        {
            IEnumerable<Local> _localesList = null;
            try
            {
                _localesList = localRepository.QueryAll();
            }
            catch (Exception)
            {

            }
            return _localesList;
        }

        public void Insert(Local local)
        {
            try
            {
                localRepository.Insert(local);
            }
            catch (Exception)
            {

            }
        }

        public Local QueryById(int id)
        {
            Local local = null;
            try
            {
                local = localRepository.QueryById(id);
            }
            catch (Exception) {}
            return local;
        }

        public void Update(Local local)
        {
            try
            {
                localRepository.Update(local);
            }
            catch (Exception) { }
        }

        public void Delete(int id)
        {
            localRepository.Delete(id);
        }

        public IEnumerable<Local> Search(Local parametrosBusqueda)
        {
            IEnumerable<Local> resultado = null;
            try
            {
                resultado = localRepository.Search(parametrosBusqueda);
            }
            catch (Exception) { }
            return resultado;
        }
    }
}
