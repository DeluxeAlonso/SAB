
using SAB.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.User
{
    public interface IUserAccountRepository:IRepository<UserAccount>
    {
        UserAccount GetUser(string username, string pw);


        int Exits(string dni);

        IEnumerable<UserAccount> Search(int searchCode, string searchDNI, string searchName, string searchApellido, string from, string to, int tipo, string estado,int biblioteca,int tipousuario);

        int ExitsCorreo(string p);

        IEnumerable<UserAccount> QueryAll(int p);

        UserAccount QueryByCorreo(string correo);

        UserAccount QueryByDni(string p);
        UserAccount QueryByDocumentNumber(string doc_number);
    }
}
