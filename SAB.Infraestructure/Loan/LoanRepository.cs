using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Loan;
using SAB.Domain.Publication;
using SAB.Domain.User;
using SAB.Base.LoanInterface;
using SAB.Domain.Reserves;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace SAB.Infraestructure.LoanInfraestructure
{
    public class LoanRepository : ILoanRepository
    {
        public IEnumerable<Loan> QueryAllP(UserAccount u)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var loans = new List<Loan>();
            IDataReader reader = database.ExecuteReader("dbo.Loan_QueryAll", u.IdBiblioteca);
            if (u.IdPerfil == 2)
            {
                reader = database.ExecuteReader("dbo.Loan_admm");

            }
            using (reader)
            {
                while (reader.Read())
                {
                    PublicationItem item = new PublicationItem();
                    item.Id = Convert.ToInt32(reader["ID_ITEM"]);

                    item.Publication = new PublicationTitle();
                    item.Publication.Id = Convert.ToInt32(reader["ID_PUBLICACION"]);
                    item.Publication.Title = Convert.ToString(reader["TITULO"]);

                    UserAccount user = new UserAccount();
                    user.Id = Convert.ToInt32(reader["ID_USUARIO"]);

                    Reserve reserve = new Reserve();
                    if (!reader.IsDBNull(5))
                        reserve.Id = Convert.ToInt32(reader["ID_RESERVA"]);
                    else
                        reserve.Id = 0;

                    Loan l = new Loan();
                    l.Id = Convert.ToInt32(reader["ID"]);
                    l.Register_Date = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    l.End_Date = Convert.ToDateTime(reader["FECHA_ENTREGA"]);
                    l.Start_Date= Convert.ToDateTime(reader["FECHA_DEVOLUCION"]);
                    l.Status = Convert.ToString(reader["ESTADO"]);
                    l.Loan_User = user;
                    l.Loan_Item = item;
                    l.Loan_Reserve = reserve;

                    loans.Add(l);

                }
            }
            return loans;
        }

        public IEnumerable<Loan> QueryAllMyLoan(UserAccount u)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var loans = new List<Loan>();
            DateTime start = Convert.ToDateTime("1920-01-01"), end = DateTime.Now;
            IDataReader reader = database.ExecuteReader("dbo.Loan_Search", u.Id, null, start, end, null, null);

            using (reader)
            {
                while (reader.Read())
                {
                    PublicationItem item = new PublicationItem();
                    item.Id = Convert.ToInt32(reader["ID_ITEM"]);

                    item.Publication = new PublicationTitle();
                    item.Publication.Id = Convert.ToInt32(reader["ID_PUBLICACION"]);
                    item.Publication.Title = Convert.ToString(reader["TITULO"]);

                    UserAccount user = new UserAccount();
                    user.Id = Convert.ToInt32(reader["ID_USUARIO"]);
                    Reserve reserve = new Reserve();

                    if (!reader.IsDBNull(5))
                        reserve.Id = Convert.ToInt32(reader["ID_RESERVA"]);
                    else
                        reserve.Id = 0;

                    Loan l = new Loan();
                    l.Id = Convert.ToInt32(reader["ID"]);
                    l.Register_Date = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    l.End_Date = Convert.ToDateTime(reader["FECHA_ENTREGA"]);
                    l.Start_Date = Convert.ToDateTime(reader["FECHA_DEVOLUCION"]);
                    l.Status = Convert.ToString(reader["ESTADO"]);
                    l.Loan_User = user;
                    l.Loan_Item = item;
                    l.Loan_Reserve = reserve;

                    loans.Add(l);
                }
            }
            return loans;
        }

        public IEnumerable<LoanType> QueryAllLoanTypes()
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var loans = new List<LoanType>();
            IDataReader reader = database.ExecuteReader("dbo.LoanType_QueryAll");
            using (reader)
            {
                while (reader.Read())
                {

                    LoanType l = new LoanType();
                    l.Id = Convert.ToInt32(reader["ID"]);
                    l.Name = Convert.ToString(reader["NOMBRE"]);
                    l.Description = Convert.ToString(reader["DESCRIPCION"]);

                    loans.Add(l);

                }
            }
            return loans;
        }
        


        public IEnumerable<Loan> QueryAll()
        {
            
            return null;
        }

        public void Insert(Loan l)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            if (l.Loan_Reserve != null)
            {
                database.ExecuteNonQuery("dbo.Loan_Insert", l.Register_Date, l.End_Date, l.Start_Date, l.Status, l.Loan_Reserve.Id, l.Loan_Item.Id, l.Loan_User.Id, l.Loan_Type.Id);
                database.ExecuteNonQuery("dbo.Reserve_Update",l.Loan_Reserve.Id ,"FINALIZADO");
            }
            else
                database.ExecuteNonQuery("dbo.Loan_Insert", l.Register_Date, l.End_Date, l.Start_Date, l.Status, null, l.Loan_Item.Id, l.Loan_User.Id, l.Loan_Type.Id);
            database.ExecuteNonQuery("dbo.Loan_UpdateItem", l.Loan_Item.Id);
        }

        public int Delete(Loan entity)
        {
            throw new NotImplementedException();
        }

        public Loan QueryById(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            IDataReader reader = database.ExecuteReader("dbo.Loan_QueryById",id);
            Loan l = new Loan();
            using (reader)
            {
                    reader.Read();
                    PublicationItem item = new PublicationItem();
                    item.Id = Convert.ToInt32(reader["ID_ITEM"]);

                    item.Publication = new PublicationTitle();
                    item.Publication.Id = Convert.ToInt32(reader["ID_PUBLICACION"]);
                    item.Publication.Title = Convert.ToString(reader["TITULO"]);

                    UserAccount user = new UserAccount();
                    user.Id = Convert.ToInt32(reader["ID_USUARIO"]);
                    Reserve reserve = new Reserve();

                    if (!reader.IsDBNull(5))
                        reserve.Id = Convert.ToInt32(reader["ID_RESERVA"]);
                    else
                        reserve.Id = 0;

                    l.Id = Convert.ToInt32(reader["ID"]);
                    l.Register_Date = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    l.End_Date = Convert.ToDateTime(reader["FECHA_ENTREGA"]);
                    l.Start_Date = Convert.ToDateTime(reader["FECHA_DEVOLUCION"]);
                    l.Status = Convert.ToString(reader["ESTADO"]);
                    l.Loan_User = user;
                    l.Loan_Item = item;
                    l.Loan_Reserve = reserve;
            }
            return l;
        }

        public void Update(Loan entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Loan> SearchLoan(int user_id,int publication_id,DateTime start, DateTime end , string status,UserAccount u)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var loans = new List<Loan>();
            IDataReader reader = database.ExecuteReader("dbo.Loan_Search", user_id, publication_id, start, end, status,u.IdBiblioteca);
            if (u.IdPerfil != 2) { 
                if (user_id == 0 && publication_id == 0)
                    reader = database.ExecuteReader("dbo.Loan_Search", null, null, start, end, status,u.IdBiblioteca);
                else if (user_id == 0)
                    reader = database.ExecuteReader("dbo.Loan_Search", null, publication_id, start, end, status,u.IdBiblioteca);
                else if (publication_id == 0)
                    reader = database.ExecuteReader("dbo.Loan_Search", user_id, null, start, end, status,u.IdBiblioteca);
            }
            else
            {
                reader = database.ExecuteReader("dbo.Loan_Search", user_id, publication_id, start, end, status,null);
                if (user_id == 0 && publication_id == 0)
                    reader = database.ExecuteReader("dbo.Loan_Search", null, null, start, end, status, null);
                else if (user_id == 0)
                    reader = database.ExecuteReader("dbo.Loan_Search", null, publication_id, start, end, status, null);
                else if (publication_id == 0)
                    reader = database.ExecuteReader("dbo.Loan_Search", user_id, null, start, end, status, null);
            }

            using (reader)
            {
                while (reader.Read())
                {
                    PublicationItem item = new PublicationItem();
                    item.Id = Convert.ToInt32(reader["ID_ITEM"]);

                    item.Publication = new PublicationTitle();
                    item.Publication.Id = Convert.ToInt32(reader["ID_PUBLICACION"]);
                    item.Publication.Title = Convert.ToString(reader["TITULO"]);

                    UserAccount user = new UserAccount();
                    user.Id = Convert.ToInt32(reader["ID_USUARIO"]);

                    Reserve reserve = new Reserve();

                    if (!reader.IsDBNull(5))
                        reserve.Id = Convert.ToInt32(reader["ID_RESERVA"]);
                    else
                        reserve.Id = 0;

                    Loan l = new Loan();
                    l.Id = Convert.ToInt32(reader["ID"]);
                    l.Register_Date = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    l.End_Date = Convert.ToDateTime(reader["FECHA_ENTREGA"]);
                    l.Start_Date = Convert.ToDateTime(reader["FECHA_DEVOLUCION"]);
                    l.Status = Convert.ToString(reader["ESTADO"]);
                    l.Loan_User = user;
                    l.Loan_Item = item;
                    l.Loan_Reserve = reserve;

                    loans.Add(l);

                }

            }
            return loans;
        }

        

        public IEnumerable<Loan> ReportLoan(int user_id, DateTime start, DateTime end, UserAccount u)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var loans = new List<Loan>();
            IDataReader reader = database.ExecuteReader("dbo.Loan_UserReport", user_id, start, end, u.IdBiblioteca);
            if (u.IdPerfil != 2)
            {
                reader = database.ExecuteReader("dbo.Loan_UserReport", user_id, start, end, u.IdBiblioteca);
            }
            else
            {
                reader = database.ExecuteReader("dbo.Loan_UserReport", user_id, start, end, null);
            }

            using (reader)
            {
                while (reader.Read())
                {
                    PublicationItem item = new PublicationItem();
                    item.Id = Convert.ToInt32(reader["ID_ITEM"]);

                    item.Publication = new PublicationTitle();
                    item.Publication.Id = Convert.ToInt32(reader["ID_PUBLICACION"]);
                    item.Publication.Title = Convert.ToString(reader["TITULO"]);

                    UserAccount user = new UserAccount();
                    user.Id = Convert.ToInt32(reader["ID_USUARIO"]);
                    user.Name = Convert.ToString(reader["NAME_USUARIO"]);
                    Reserve reserve = new Reserve();

                    if (!reader.IsDBNull(5))
                        reserve.Id = Convert.ToInt32(reader["ID_RESERVA"]);
                    else
                        reserve.Id = 0;

                    LoanType lt = new LoanType();
                    lt.Name = Convert.ToString(reader["TIPO"]);

                    Loan l = new Loan();
                    l.Id = Convert.ToInt32(reader["ID"]);
                    l.Register_Date = Convert.ToDateTime(reader["FECHA_REGISTRO"]);
                    l.End_Date = Convert.ToDateTime(reader["FECHA_ENTREGA"]);
                    l.Start_Date = Convert.ToDateTime(reader["FECHA_DEVOLUCION"]);
                    l.Status = Convert.ToString(reader["ESTADO"]);
                    l.Loan_Type = lt;
                    l.Loan_User = user;
                    l.Loan_Item = item;
                    l.Loan_Reserve = reserve;

                    loans.Add(l);

                }

            }
            return loans;
        }

        public UserAccount GetLoanUser(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            UserAccount u = new UserAccount();
            IDataReader reader = database.ExecuteReader("dbo.Usuario_QueryById", id);
            using (reader)
            {
                reader.Read();
                u.Id = Convert.ToInt32(reader["ID"]);
                u.Name = Convert.ToString(reader["NOMBRE"]);
            }
            return u;
        }



        public int ValidateCantPerUser(int user_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int quantity = 0;
            IDataReader reader = database.ExecuteReader("dbo.Loan_CantPerUser", user_id);
            using (reader)
            {
                reader.Read();
                quantity = Convert.ToInt32(reader["DIFFERENCE"]);

            }
            return quantity;
        }

        public IEnumerable<PublicationItem> GetItemsWithoutReserve(int publication_id,UserAccount u)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var items = new List<PublicationItem>();
            IDataReader reader = database.ExecuteReader("dbo.Loan_GetItemWithoutReserve",publication_id, u.IdBiblioteca);
            using (reader)
            {
                while (reader.Read())
                {
                    PublicationItem item = new PublicationItem();
                    item.Id = Convert.ToInt32(reader["ITEM_ID"]);

                    item.Publication = new PublicationTitle();
                    item.Publication.Id = Convert.ToInt32(reader["PUBLICACION_ID"]);
                    item.Publication.Title = Convert.ToString(reader["TITULO"]);
      
                    items.Add(item);
                }

            }
            return items;
        }

        public IEnumerable<PublicationItem> GetItemsWithReserve(int user_id, int publication_id,ref Reserve r,UserAccount u)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var items = new List<PublicationItem>();
            IDataReader reader = database.ExecuteReader("dbo.Loan_GetItemWithReserve",user_id, publication_id, DateTime.Now,u.IdBiblioteca);
            using (reader)
            {
                while (reader.Read())
                {
                    PublicationItem item = new PublicationItem();
                    item.Id = Convert.ToInt32(reader["ITEM_RESERVADO"]);

                    item.Publication = new PublicationTitle();
                    item.Publication.Id = Convert.ToInt32(reader["PUBLICACION_ID"]);
                    item.Publication.Title = Convert.ToString(reader["TITULO"]);

                    r.Id = Convert.ToInt32(reader["ID"]);
                    items.Add(item);
                }

            }
            return items;
        }

        public int ValidatePublication(int publication_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int quantity = 0;
            IDataReader reader = database.ExecuteReader("dbo.Loan_ValidPublication", publication_id);
            using (reader)
            {
                reader.Read();
                quantity = Convert.ToInt32(reader["PUBLICATION_CANT"]);

            }
            return quantity;
        }

        public int ValidateUser(int user_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int quantity = 0;
            IDataReader reader = database.ExecuteReader("dbo.Loan_ValidUser", user_id);
            using (reader)
            {
                reader.Read();
                quantity = Convert.ToInt32(reader["USER_CANT"]);

            }
            return quantity;
        }

        public int ValidatePublicationTypePerUser(int user_id, int publication_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int quantity = 0;
            IDataReader reader = database.ExecuteReader("dbo.Loan_PerfilPublication", user_id, publication_id);
            using (reader)
            {
                reader.Read();
                quantity = Convert.ToInt32(reader["CANTIDAD"]);

            }
            return quantity;
        }

        public int GetLoanDays(int user_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int quantity = 0;
            IDataReader reader = database.ExecuteReader("dbo.DiasReserva_Get", user_id);
            using (reader)
            {
                reader.Read();
                quantity = Convert.ToInt32(reader["CANTIDAD_DIAS"]);

            }
            return quantity;
        }

        public void returnLoan(Loan l, int n, int days_diff,int user_id,int l_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int quantity;
            int id_tiposancion;
            database.ExecuteNonQuery("dbo.Loan_Update", l.Id,DateTime.Now);
            if (n != 2)
                database.ExecuteNonQuery("dbo.Loan_ReturnItem", l.Loan_Item.Id);
            else
                database.ExecuteNonQuery("ItemPublicacion_Delete", l.Loan_Item.Id);
            if (n == 1)
            {
                IDataReader reader = database.ExecuteReader("dbo.Loan_GetSanctionDays", DateTime.Now);
                using (reader)
                {
                    reader.Read();
                    quantity = Convert.ToInt32(reader["CANTIDAD"]);
                    id_tiposancion = Convert.ToInt32(reader["ID"]);
                }
                int number_of_days = quantity * (days_diff)*(-1);
                int profile_id;
                reader = database.ExecuteReader("dbo.Loan_get_profile", user_id);
                using (reader)
                {
                    reader.Read();
                    profile_id= Convert.ToInt32(reader["ID"]);

                }
                DateTime days= (DateTime.Now).AddDays(quantity*days_diff*(-1));
                database.ExecuteNonQuery("dbo.Loan_SanctionInsert", days,l_id,id_tiposancion,user_id);
            }
            else if (n == 2)
            {
                IDataReader reader = database.ExecuteReader("dbo.GetSanctionWhenLose");
                using (reader)
                {
                    reader.Read();
                    quantity = Convert.ToInt32(reader["CANTIDAD"]);
                    id_tiposancion = Convert.ToInt32(reader["ID"]);
                }
                int number_of_days = quantity * (days_diff) * (-1);
                int profile_id;
                reader = database.ExecuteReader("dbo.Loan_get_profile", user_id);
                using (reader)
                {
                    reader.Read();
                    profile_id = Convert.ToInt32(reader["ID"]);

                }
                DateTime days = (DateTime.Now).AddDays(quantity * days_diff * (-1));
                database.ExecuteNonQuery("dbo.Loan_SanctionInsert", days, l_id, id_tiposancion, user_id);
            }
        }
    }
}
