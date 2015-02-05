using SAB.Base.User;

using SAB.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.User
{
    public class UserAccountApplication
    {
        private readonly IUserAccountRepository userAccountRepository;

        public UserAccountApplication(IUserAccountRepository userAccountRepository)
        {
            this.userAccountRepository = userAccountRepository;
        }

        public UserAccount GetUser(string username, string pw)
        {
            UserAccount _useraccount = null;
            try
            {
                _useraccount = userAccountRepository.GetUser(username, pw);

            }
            catch { }

            return _useraccount;
        }

        public void Insert(UserAccount u)
        {
            try
            {
                userAccountRepository.Insert(u);
            }
            catch (Exception ex){
                Console.WriteLine(ex.ToString());
            }

        }

        public UserAccount QueryById(int p)
        {

            UserAccount u = null;
            try
            {
                u=userAccountRepository.QueryById(p);
            }
            catch { }
            return u;
           

        }

        public void Update(UserAccount uNuevo)
        {
            try
            {
                userAccountRepository.Update(uNuevo);
            }
            catch { }
        }

        public int Exits(string dni)
        {
            int count=-1;
            try
            {
                count = userAccountRepository.Exits(dni);
            }
            catch { }

            return count;
        }

        

        public object Search(int searchCode, string searchDNI, string searchName, string searchApellido, string from, string to, int tipo, string estado,int biblioteca,int tipousuario )
        {
            IEnumerable<UserAccount> u = null;
            try
            {
                u = userAccountRepository.Search( searchCode,  searchDNI,  searchName,  searchApellido,  from,  to,  tipo,  estado,biblioteca, tipousuario);
            }
            catch { }
            return u;
        }

        public void Delete(int id)
        {
            UserAccount u = null;

            try
            {   u=userAccountRepository.QueryById(id);
                userAccountRepository.Delete(u);
            }
            catch { }
           
           
        }

        public int ExitsCorreo(string p)
        {
            int count = -1;
            try
            {
                count = userAccountRepository.ExitsCorreo(p);
            }
            catch { }

            return count;
        }

        public IEnumerable<UserAccount> QueryAll(int p)
        {
            IEnumerable<UserAccount> u = null;
            try
            {
                u = userAccountRepository.QueryAll(p);
            }
            catch { }
            return u;
        }



        public IEnumerable<UserAccount> QueryAll()
        {
            IEnumerable<UserAccount> u = null;
            try
            {
                u = userAccountRepository.QueryAll(1);
            }
            catch { }
            return u;
        }

        public UserAccount QueryByCorreo(string correo)
        {
            UserAccount _user = null;
            try
            {
                _user = userAccountRepository.QueryByCorreo(correo);
            }
            catch (Exception)
            {

            }

            return _user;
        }

        public UserAccount QueryByDNI(string p)
        {
            UserAccount _user = null;
            try
            {
                _user = userAccountRepository.QueryByDni(p);
            }
            catch (Exception)
            {

            }

            return _user;
        }

        public UserAccount QueryByDocumentNumber(string p)
        {
            UserAccount _user = null;
            try
            {
                _user = userAccountRepository.QueryByDocumentNumber(p);
            }
            catch (Exception)
            {

            }

            return _user;
        }


       
    }
}
