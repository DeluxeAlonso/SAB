using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Loan;
using SAB.Domain.Publication;
using SAB.Domain.User;
using SAB.Domain.Reserves;

namespace SAB.Base.LoanInterface
{
    public interface ILoanRepository : IRepository<Loan>
    {
        IEnumerable<Loan> SearchLoan(int user_id, int publication_id, DateTime start, DateTime end, string status, UserAccount u);
        int ValidateCantPerUser(int user_id);
        IEnumerable<PublicationItem> GetItemsWithoutReserve(int publication_id, UserAccount u);
        IEnumerable<PublicationItem> GetItemsWithReserve(int user_id, int publication_id, ref Reserve r, UserAccount u);
        int ValidatePublication(int publication_id);
        int ValidateUser(int user_id);
        int ValidatePublicationTypePerUser(int user_id, int publication_id);
        int GetLoanDays(int user_id);
        void returnLoan(Loan l, int n, int days_diff, int user_id, int l_id);
        IEnumerable<Loan> QueryAllP(UserAccount u);
        IEnumerable<LoanType> QueryAllLoanTypes();
        IEnumerable<Loan> ReportLoan(int user_id, DateTime start, DateTime end, UserAccount u);
        UserAccount GetLoanUser(int id);
        IEnumerable<Loan> QueryAllMyLoan(UserAccount u);

    }
}
