using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Reserves;
using SAB.Base.Reserves;

namespace SAB.Application.Reserves
{    
    public class ReserveAplication
    {
        private readonly IReserveRepository reserveRepository;

        public ReserveAplication(IReserveRepository reserveRepository)
        {
            this.reserveRepository = reserveRepository;
        }


        public IEnumerable<Reserve> PublicationQueryAll()
        {
            IEnumerable<Reserve>  _publicationRepository = null;

            try
            {
                _publicationRepository = reserveRepository.PublicationQueryAll();
            }
            catch (Exception)
            { 
            
            }

            return _publicationRepository;
        }
    }
}
