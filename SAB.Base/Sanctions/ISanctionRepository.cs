using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Sanctions;

namespace SAB.Base.Sanctions
{
    public interface ISanctionRepository
    {
        bool isSanctioned(int user_id);
        IEnumerable<Sanction> GetSanctionsByUser(int user_id);
        IEnumerable<Sanction> GetSanctionsByDates(DateTime start, DateTime end);
        SanctionType GetSanctionType(int id_tiposancion);

    }
}
