using SAB.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.User
{
    public interface IActionRepository:IRepository<ActionType>
    {

        List<ActionType> QueryAllByIdPerfil(int id);
        void DeleteById(int id);

    }
}
