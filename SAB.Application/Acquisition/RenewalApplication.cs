using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base.Acquisition;
using SAB.Domain.Acquisition;

namespace SAB.Application.Acquisition
{
    public class RenewalApplication
    {
        private readonly IRenewalRepository renewalRepository;

        public RenewalApplication(IRenewalRepository rr)
        {
            this.renewalRepository = rr;
        }

        public IEnumerable<Renewal> QueryAll(int id_suscripcion)
        {
            IEnumerable<Renewal> _renewalList = null;
            try
            {
                _renewalList = renewalRepository.QueryAll(id_suscripcion);
            }
            catch (Exception)
            {

            }
            return _renewalList;
        }

        public Renewal QueryById(int id)
        {
            Renewal _renewal = null;
            try
            {
                _renewal = renewalRepository.QueryById(id);
            }
            catch (Exception)
            {

            }
            return _renewal;
        }

        public void Insert(Renewal renewal)
        {
            try
            {
                renewalRepository.Insert(renewal);
            }
            catch (Exception)
            {

            }
        }

        public void Update(Renewal renewal)
        {
            try
            {
                renewalRepository.Update(renewal);
            }
            catch (Exception)
            {

            }
        }

        public void Delete(Renewal renewal)
        {
            try
            {
                renewalRepository.Delete(renewal);
            }
            catch (Exception)
            {

            }
        }

        public void Caducar(Renewal renewal)
        {
            try
            {
                renewalRepository.Caducar(renewal);
            }
            catch (Exception)
            {

            }
        }
    }
}
