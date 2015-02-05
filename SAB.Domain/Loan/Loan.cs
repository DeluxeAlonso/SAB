using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;
using SAB.Domain.Reserves;
using SAB.Domain.User;

namespace SAB.Domain.Loan
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime Register_Date { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string Status { get; set; }
        public Reserve Loan_Reserve { get; set; }
        public PublicationItem Loan_Item { get; set; }
        public UserAccount Loan_User { get; set; }
        public LoanType Loan_Type { get; set; }
        //public Reserves Loan_Reserve { get; set; }
        //public Loan_Type Loan_Type { get; set; }
        //public Item Loan_Item { get; set; }
        //public Active Loan_Active { get; set; }
    }
}
