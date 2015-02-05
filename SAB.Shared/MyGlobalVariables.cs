using SAB.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Shared
{
    public class MyGlobalVariables
    {
        public static List<ActionType> ListActions { get; set; }
        public static UserAccount UserActive { get; set; }

        public static List<int> convertIdAction() {
            List<int> list = new List<int>();
            foreach (ActionType aType in ListActions)
            {
                list.Add(aType.Id);
            }
            return list;
        }

       

       
    }
}
