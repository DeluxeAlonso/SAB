using Microsoft.Practices.EnterpriseLibrary.Data;
using SAB.Base.User;
using SAB.Domain.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SAB.Infraestructure.User
{
    public class ActionRepository:IActionRepository
    {
        public List<ActionType> QueryAllByIdPerfil(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Action_QueryAllByIdPerfil",id))
            {
                 List<ActionType> listAct= new List<ActionType>();
                while (reader.Read())
                {
                  
                     ActionType a = new ActionType();
                        a.Id = Convert.ToInt32(reader["ID_ACCION"]);
                        a.Name = Convert.ToString(reader["NOMBRE"]);
                        listAct.Add(a);
                    
                }
                return listAct;
            }
        }

        public void Insert(ActionType entity)
        {
            throw new NotImplementedException();
        }



        public int Delete(ActionType entity)
        {
            throw new NotImplementedException();
        }

        public ActionType QueryById(int id)
        {
            throw new NotImplementedException();
        }


 

        public void DeleteById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.UserProfile_DeletePrivilegio", id);
        
        }


        public IEnumerable<ActionType> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Action_QueryAll"))
            {
                List<ActionType> listAct = new List<ActionType>();
                while (reader.Read())
                {

                    ActionType a = new ActionType();
                    a.Id = Convert.ToInt32(reader["ID"]);
                    a.Name = Convert.ToString(reader["NOMBRE"]);
                    listAct.Add(a);

                }
                return listAct;
            }
        }

        public void Update(ActionType entity)
        {
            throw new NotImplementedException();
        }

    }
}
