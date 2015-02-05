using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Acquisition
{
    public class SuscriptionApplication
    {
        private readonly ISuscriptionRepository suscriptionRepository;

        public SuscriptionApplication(ISuscriptionRepository suscriptionRepository)
        {
            this.suscriptionRepository = suscriptionRepository;
        }

        public IEnumerable<Suscription> QueryAll()
        {
            IEnumerable<Suscription> _SuscriptionList = null;

            try
            {
                _SuscriptionList = suscriptionRepository.QueryAll();
            }
            catch (Exception)
            {

            }
            return _SuscriptionList;
        }

        public void Insert(Suscription suscription)
        {
            try
            {
                suscriptionRepository.Insert(suscription);
            }
            catch (Exception)
            {

            }
        }

        public Suscription QueryById(int id)
        {
            Suscription _suscription = null;
            try
            {
                _suscription = suscriptionRepository.QueryById(id);
            }
            catch (Exception)
            {

            }
            return _suscription;

        }

        public int Delete(Suscription suscription)
        {
            int resultado = 0;
            try
            {
                suscriptionRepository.Delete(suscription);
            }
            catch (Exception)
            {

            }
            return resultado;
        }

        public IEnumerable<Suscription> Search(Suscription parametros, string titulo)
        {
            IEnumerable<Suscription> resultado = null;
            try
            {

            }
            catch (Exception)
            {
                resultado = suscriptionRepository.Search(parametros, titulo);
            }
            return resultado;
        }
    }
}
