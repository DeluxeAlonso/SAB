using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using SAB.Base.Sanctions;


namespace SAB.Infraestructure.Sanctions
{
    public class SanctionRepository : ISanctionRepository
    {
        public bool isSanctioned(int user_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            bool isSanctioned = false;

            using (IDataReader reader = database.ExecuteReader("dbo.isSanctionedUser", user_id, DateTime.Now))
            {
                while (reader.Read())
                {
                    isSanctioned = true;
                }
            }
            return isSanctioned;
        }
    }
}
