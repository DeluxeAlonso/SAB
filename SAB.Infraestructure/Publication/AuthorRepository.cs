using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Publication;
using SAB.Base.Publication;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace SAB.Infraestructure.Publication
{
    public class AuthorRepository : IAuthorRepository
    {
        /***************************************************************************************/

        public IEnumerable<Author> QueryAll()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            using (IDataReader reader = database.ExecuteReader("dbo.Autor_QueryAll"))
            {
                while (reader.Read())
                {

                    yield return new Author
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Name = Convert.ToString(reader["NOMBRE"]),
                        First_last_Name = Convert.ToString(reader["AP_PATERNO"]),
                        Second_last_Name = Convert.ToString(reader["AP_MATERNO"]),
                        Country = Convert.ToString(reader["PAIS"]),
                        Hometown = Convert.ToString(reader["CIUDAD"]),
                    };

                }
            }
        }

        /***************************************************************************************/

        public IEnumerable<Author> Search(int id, string name, string country)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");

            if (name == "") name = null;
            if (country == "") country = null;

            using (IDataReader reader = database.ExecuteReader("dbo.Autor_Search", id, name, country))
            {
                while (reader.Read())
                {
                    yield return new Author
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Name = Convert.ToString(reader["NOMBRE"]),
                        First_last_Name = Convert.ToString(reader["AP_PATERNO"]),
                        Second_last_Name = Convert.ToString(reader["AP_MATERNO"]),
                        Country = Convert.ToString(reader["PAIS"])
                    };

                }
            }
        }

        /***************************************************************************************/

        public Author QueryById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            Author autor = new Author();

            using (IDataReader reader = database.ExecuteReader("dbo.Autor_QueryById", id))
            {
                if (reader.Read())
                {
                    autor.Id = Convert.ToInt32(reader["ID"]);
                    autor.Name = Convert.ToString(reader["NOMBRE"]);
                    autor.First_last_Name = Convert.ToString(reader["AP_PATERNO"]);
                    autor.Second_last_Name = Convert.ToString(reader["AP_MATERNO"]);
                    autor.Country = Convert.ToString(reader["PAIS"]);
                    autor.Hometown = Convert.ToString(reader["CIUDAD"]);
                }
            }
            return autor;
        }

        /***************************************************************************************/
        public void Insert(Author entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Autor_Insert", entity.Name, entity.First_last_Name,
                entity.Second_last_Name, entity.Country, entity.Hometown);
        }

        /***************************************************************************************/

        public void Update(Author entity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.Autor_Update", entity.Id, entity.Name, entity.First_last_Name,
                entity.Second_last_Name, entity.Country, entity.Hometown);
        }

        /***************************************************************************************/

        public int Delete(Author entity)
        {
            throw new NotImplementedException();
        }

        /***************************************************************************************/
    }
}
