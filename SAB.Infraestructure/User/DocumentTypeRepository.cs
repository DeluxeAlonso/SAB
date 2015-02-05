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
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        public void Insert(Domain.User.DocumentType entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Domain.User.DocumentType entity)
        {
            throw new NotImplementedException();
        }

        public Domain.User.DocumentType QueryById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            DocumentType a = new DocumentType();
            using (IDataReader reader = database.ExecuteReader("dbo.DocumentType_QueryById", id))
            {

                if (reader.Read())
                {


                    a.Id = Convert.ToInt32(reader["ID"]);
                    a.Name = Convert.ToString(reader["NOMBRE"]);
                    a.DigitQuantity = Convert.ToInt32(reader["CANT_DIGITOS"]);
                    a.IsNumerico = Convert.ToInt32(reader["IS_NUMERICO"]);


                }

            }
            return a;
        }

        public void Update(Domain.User.DocumentType entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Domain.User.DocumentType> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.DocumentType_QueryAll"))
            {
                List<DocumentType> listAct = new List<DocumentType>();
                while (reader.Read())
                {

                    DocumentType a = new DocumentType();
                    a.Id = Convert.ToInt32(reader["ID"]);
                    a.Name = Convert.ToString(reader["NOMBRE"]);
                    a.DigitQuantity = Convert.ToInt32(reader["CANT_DIGITOS"]);
                    a.IsNumerico = Convert.ToInt32(reader["IS_NUMERICO"]);
                    listAct.Add(a);

                }
                return listAct;
            }
        }




      
    }
}
