using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Loan;
using SAB.Domain.Publication;
using SAB.Domain.Reserves;
using SAB.Base.LoanInterface;
using SAB.Domain.User;
using SAB.Base.Sanctions;

namespace SAB.Application.LoanApp
{
    public class LoanApplication
    {
        private readonly ILoanRepository loanRepository;
        private readonly ISanctionRepository sanctionRepository;

        public LoanApplication(ILoanRepository loanRepository, ISanctionRepository sanctionRepository)
        {
            this.loanRepository = loanRepository;
            this.sanctionRepository = sanctionRepository;
        }


        public IEnumerable<Loan>QueryAll()
        {
            IEnumerable<Loan>  _loanRepository = null;

            try
            {
                _loanRepository = loanRepository.QueryAll();
            }
            catch (Exception)
            { 
            
            }

            return _loanRepository;
        }

        public Loan QueryById( int id)
        {
            Loan _loanRepository = null;

            try
            {
                _loanRepository = loanRepository.QueryById(id);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }

        public IEnumerable<Loan> SearchLoan(int user_id, int publication_id, DateTime start, DateTime end, string status, UserAccount u)
        {
            IEnumerable<Loan> _loanRepository = null;

            try
            {
                _loanRepository = loanRepository.SearchLoan(user_id, publication_id, start, end,status,u);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }


        public int ValidateCantPerUser(int user_id)
        {
            int _loanRepository = 0;

            try
            {
                _loanRepository = loanRepository.ValidateCantPerUser(user_id);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }
        public IEnumerable<PublicationItem> GetItemsWithoutReserve(int publication_id, UserAccount u)
        {
            IEnumerable<PublicationItem> _loanRepository = null;

            try
            {
                _loanRepository = loanRepository.GetItemsWithoutReserve(publication_id,u);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }
        public IEnumerable<PublicationItem> GetItemsWithReserve(int user_id, int publication_id, ref Reserve r, UserAccount u)
        {
            IEnumerable<PublicationItem> _loanRepository = null;

            try
            {
                _loanRepository = loanRepository.GetItemsWithReserve(user_id,publication_id, ref r,u);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }
        public  int ValidatePublication(int publication_id)
        {
            int _loanRepository = 0;

            try
            {
                _loanRepository = loanRepository.ValidatePublication(publication_id);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }
        public int ValidateUser(int user_id)
        {
            int _loanRepository = 0;

            try
            {
                _loanRepository = loanRepository.ValidateUser(user_id);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }

        public int ValidatePublicationTypePerUser(int user_id, int publication_id)
        {
               int _loanRepository = 0;

            try
            {
                _loanRepository = loanRepository.ValidatePublicationTypePerUser(user_id,publication_id);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }

        public int GetLoanDays(int user_id)
        {
            int _loanRepository = 0;

            try
            {
                _loanRepository = loanRepository.GetLoanDays(user_id);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }

        public bool isSanctionUser(UserAccount u)
        {
            bool isSanction = false;

            try
            {
                isSanction = sanctionRepository.isSanctioned(u.Id);
            }
            catch
            {

            }
            return isSanction;
        }

        public void Insert(Loan l)
        {
            try
            {
                loanRepository.Insert(l);
            }
            catch (Exception)
            {

            }
        }

        public void returnLoan(Loan l, int n, int days_diff, int user_id ,int l_id)
        {
            try
            {
                loanRepository.returnLoan(l, n, days_diff,user_id, l_id);
            }
            catch (Exception)
            {

            }
        }
        public IEnumerable<Loan> QueryAllP(UserAccount u)
        {
            IEnumerable<Loan> _loanRepository = null;

            try
            {
                _loanRepository = loanRepository.QueryAllP(u);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }


        public IEnumerable<Loan> QueryAllMyLoan(UserAccount u)
        {
            IEnumerable<Loan> _loanRepository = null;

            try
            {
                _loanRepository = loanRepository.QueryAllMyLoan(u);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }

        public IEnumerable<LoanType> QueryAllLoanTypes()
        {
            IEnumerable<LoanType> _loanRepository = null;

            try
            {
                _loanRepository = loanRepository.QueryAllLoanTypes();
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }

        public IEnumerable<Loan> ReportLoan(int user_id, DateTime start, DateTime end, UserAccount u)
        {
            IEnumerable<Loan> _loanRepository = null;

            try
            {
                _loanRepository = loanRepository.ReportLoan(user_id, start, end, u);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }

        public UserAccount GetLoanUser(int id)
        {
            UserAccount _loanRepository = null;

            try
            {
                _loanRepository = loanRepository.GetLoanUser(id);
            }
            catch (Exception)
            {

            }

            return _loanRepository;
        }

    }
}
