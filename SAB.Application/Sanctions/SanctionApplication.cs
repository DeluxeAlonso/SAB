using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base.Sanctions;
using SAB.Domain.Sanctions;

namespace SAB.Application.Sanctions
{
    public class SanctionApplication
    {
        
        private readonly ISanctionRepository sanctionRepository;

        public SanctionApplication(ISanctionRepository sanctionRepository)
        {
            this.sanctionRepository = sanctionRepository;
        }

        public IEnumerable<Sanction> GetSanctionsByUser(int user_id)
        {
            IEnumerable<Sanction> _SanctionRepository = null;

            try
            {
                _SanctionRepository = sanctionRepository.GetSanctionsByUser(user_id);
            }
            catch (Exception)
            {

            }

            return _SanctionRepository;
        }

        public IEnumerable<Sanction> GetSanctionsByDates(DateTime start, DateTime end)
        {
            IEnumerable<Sanction> _SanctionRepository = null;

            try
            {
                _SanctionRepository = sanctionRepository.GetSanctionsByDates(start, end);
            }
            catch (Exception)
            {

            }

            return _SanctionRepository;
        }

        public SanctionType GetSanctionType(int id_tiposancion)
        {
            SanctionType _SanctionType = null;

            try
            {
                _SanctionType = sanctionRepository.GetSanctionType(id_tiposancion);
            }
            catch (Exception)
            {

            }

            return _SanctionType;
        }
    }
}
