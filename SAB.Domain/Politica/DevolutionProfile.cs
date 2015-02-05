using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Domain.Politica
{
    public class DevolutionProfile

    {
        public int IdPefil { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }

        public int Days { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }
}
