using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;
using SAB.Domain.User;
using SAB.Domain.Assets;


namespace SAB.Domain.Reserves
{
    public class Reserve
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string State { get; set; }
        public PublicationItem Item { get; set; }
        public UserAccount User { get; set; }
        public Asset Asset { get; set; }
    }
}
