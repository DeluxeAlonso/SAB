using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base.Reserves;
using SAB.Domain.Reserves;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using SAB.Domain.Publication;
using SAB.Domain.Library;
using SAB.Domain.User;
using SAB.Domain.Assets;

namespace SAB.Infraestructure.Reserves
{
    public class ReserveRepository : IReserveRepository
    {

        public void InsertCubicle(Reserve reserve)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            database.ExecuteNonQuery("dbo.ReserveCubicleInsert", reserve.StartDate, reserve.EndDate, reserve.Asset.Id, reserve.User.Id);
 
        }

        public void InsertPublication (Reserve reserve)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");

            database.ExecuteNonQuery("dbo.Reserve_PublicationInsert", reserve.StartDate, reserve.EndDate, reserve.Item.Id, reserve.User.Id);
        }

        public int Delete(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");

          
            
            database.ExecuteNonQuery("dbo.Reserve_PublicationDelete", id);

            return id;
        }

        public Reserve QueryById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Reserve reserve)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reserve> PublicationQueryAll( int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var reserves = new List<Reserve>();

            using (IDataReader reader = database.ExecuteReader("dbo.ReservePublications_QueryAll", "RESERVADO",id, DateTime.Now))
            {
                while (reader.Read())
                {    
                    var item = new PublicationItem();
                    item.Publication = new PublicationTitle() { Id = Convert.ToInt32(reader["ID_PUBLICACION"]), Title = Convert.ToString(reader["TITULO"])};

                    item.Publication.Type = new PublicationType(){Name = Convert.ToString(reader["NOMBRE_TIPO_PUBLICACION"])};
                    item.Biblioteca = new Local() { Name = Convert.ToString(reader["NombreBiblioteca"]) };
                   
                    var user  = new UserAccount();
                    user.Id = Convert.ToInt32(reader["ID_USER"]);


                    var r = new Reserve();
                    r.Id = Convert.ToInt32(reader["ID"]);
                    r.StartDate = Convert.ToDateTime(reader["FECHA_INICIO"]);
                    r.EndDate = Convert.ToDateTime(reader["FECHA_FIN"]);
                    r.State = Convert.ToString(reader["ESTADO"]);
                    r.User = user;
                    r.Item = item;

                    reserves.Add(r);
                
                }
            }
            return reserves;
        }

        public IEnumerable<Reserve> CubiclesQueryAll(int id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var reserves = new List<Reserve>();

            using (IDataReader reader = database.ExecuteReader("dbo.ReserveCubicles_QueryAll", "RESERVADO", id, DateTime.Now))
            {
                while (reader.Read())
                {
                    Asset activo = new Asset() { Id = Convert.ToInt32(reader["ID_ACTIVO"]),Name= Convert.ToString(reader["NOMBRE"]) };
                    activo.Biblioteca = new Local() { Name = Convert.ToString(reader["NombreBiblioteca"]) };

                    var user = new UserAccount();
                    user.Id = Convert.ToInt32(reader["ID_USER"]);


                    var r = new Reserve();
                    r.Id = Convert.ToInt32(reader["ID"]);
                    r.StartDate = Convert.ToDateTime(reader["FECHA_INICIO"]);
                    r.EndDate = Convert.ToDateTime(reader["FECHA_FIN"]);
                    r.State = Convert.ToString(reader["ESTADO"]);
                    r.User = user;
                    r.Asset = activo;

                    reserves.Add(r);

                }
            }
            return reserves;        
        }

        public IEnumerable<Reserve> SearchPublication(int id, DateTime start, DateTime end)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var reserves = new List<Reserve>();

            using (IDataReader reader = database.ExecuteReader("dbo.ReservePublications_Search", "RESERVADO",id, start,end))
            {
                while (reader.Read())
                {
                    var item = new PublicationItem();
                    item.Publication = new PublicationTitle() { Id = Convert.ToInt32(reader["ID_PUBLICACION"]), Title = Convert.ToString(reader["TITULO"]) };
                    item.Publication.Type = new PublicationType() { Name = Convert.ToString(reader["NOMBRE_TIPO_PUBLICACION"])};
                    item.Biblioteca = new Local() { Id = Convert.ToInt32(reader["ID_BIBLIOTECA"]), Name = Convert.ToString(reader["nombrBiblioteca"]) };

                    var user = new UserAccount() { Id = Convert.ToInt32(reader["ID_USER"]) };


                    var r = new Reserve();
                    r.Id = Convert.ToInt32(reader["ID"]);
                    r.StartDate = Convert.ToDateTime(reader["FECHA_INICIO"]);
                    r.EndDate = Convert.ToDateTime(reader["FECHA_FIN"]);
                    r.State = Convert.ToString(reader["ESTADO"]);
                    r.User = user;
                    r.Item = item;

                    reserves.Add(r);

                }
            }
            return reserves;
        }


        public int getReserveCantDay(int user_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");

            int diasreserva = 0;


            using (IDataReader reader = database.ExecuteReader("dbo.DiasReserva_Get", user_id))
            {
                while (reader.Read())
                {
                    diasreserva = Convert.ToInt32(reader["CANTIDAD_DIAS"]);
                }
            }
            return diasreserva;

            
            throw new NotImplementedException();
        }

        public IEnumerable<PublicationItem> getItemsOfPublication(int publication_id,int id_biblioteca)
        {
            
            var database = DatabaseFactory.CreateDatabase("SAB");
            var itemPublicacion = new List<PublicationItem>();

            if (id_biblioteca != 0)
            {
                using (IDataReader reader = database.ExecuteReader("dbo.itemPublicacion_QueryAllCanReserveByLibray", publication_id, id_biblioteca))
                {
                    while (reader.Read())
                    {
                        var item = new PublicationItem() { Id = Convert.ToInt32(reader["ID"]), Id_Biblioteca = Convert.ToInt32(reader["ID_BIBLIOTECA"]) };
                        item.Publication = new PublicationTitle() { Id = Convert.ToInt32(reader["ID_PUBLICACION"]), Title = Convert.ToString(reader["TITULO"]) };
                        item.Biblioteca = new Local() { Id = Convert.ToInt32(reader["ID_BIBLIOTECA"]), Name = Convert.ToString(reader["nombrBiblioteca"]) };

                        itemPublicacion.Add(item);
                    }
                }
            }
            else
            {
                using (IDataReader reader = database.ExecuteReader("dbo.itemPublicacion_QueryAllCanReserve", publication_id))
                {
                    while (reader.Read())
                    {
                        var item = new PublicationItem() { Id = Convert.ToInt32(reader["ID"]), Id_Biblioteca = Convert.ToInt32(reader["ID_BIBLIOTECA"]) };
                        item.Publication = new PublicationTitle() { Id = Convert.ToInt32(reader["ID_PUBLICACION"]), Title = Convert.ToString(reader["TITULO"]) };
                        item.Biblioteca = new Local() { Id = Convert.ToInt32(reader["ID_BIBLIOTECA"]), Name = Convert.ToString(reader["nombrBiblioteca"]) };

                        itemPublicacion.Add(item);
                    }
                }
            }



            return itemPublicacion;
        }




        public void Insert(Reserve entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Reserve entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reserve> QueryAll()
        {
            throw new NotImplementedException();
        }

        public int getCantReservesCanDo(int user_id)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            int quantity = 0;
            IDataReader reader = database.ExecuteReader("dbo.Reserve_CantPerUser", user_id, DateTime.Now);
            using (reader)
            {
                reader.Read();
                quantity = Convert.ToInt32(reader["DIFFERENCE"]);

            }
            return quantity;
        }


        public List<Asset> searchCubicles(DateTime? start, DateTime? end, int quantity)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var cubicles = new List<Asset>();

            using (IDataReader reader = database.ExecuteReader("dbo.ReserveCubiclesSearch", start, end, quantity))
            {
                while (reader.Read())
                {
                    Asset cubicle = new Asset() { Id = Convert.ToInt32(reader["ID"]), Name = Convert.ToString(reader["NOMBRE"]),Quantity = Convert.ToInt32(reader["CAPACIDAD"]) };
                    cubicle.Biblioteca = new Local() { Name = Convert.ToString(reader["NombreBiblioteca"])};

                    if (cubicles.Any(c => c.Id == cubicle.Id) == false)
                    {
                        cubicles.Add(cubicle);
                    }                   

                }
            }
            return cubicles;
            
        }




        public IEnumerable<Reserve> SearchCubiclesReserve(int id, DateTime start, DateTime end)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var reserves = new List<Reserve>();

            using (IDataReader reader = database.ExecuteReader("dbo.ReserveCubicles_Search", "RESERVADO", id,start,end))
            {
                while (reader.Read())
                {
                    Asset activo = new Asset() { Id = Convert.ToInt32(reader["ID_ACTIVO"]), Name = Convert.ToString(reader["NOMBRE"]) };
                    activo.Biblioteca = new Local() { Name = Convert.ToString(reader["NombreBiblioteca"]) };

                    var user = new UserAccount();
                    user.Id = Convert.ToInt32(reader["ID_USER"]);


                    var r = new Reserve();
                    r.Id = Convert.ToInt32(reader["ID"]);
                    r.StartDate = Convert.ToDateTime(reader["FECHA_INICIO"]);
                    r.EndDate = Convert.ToDateTime(reader["FECHA_FIN"]);
                    r.State = Convert.ToString(reader["ESTADO"]);
                    r.User = user;
                    r.Asset = activo;

                    reserves.Add(r);

                }
            }
            return reserves;        
        }


        public bool userCanReserveAtThisTime(int user_id, DateTime start, DateTime end)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            bool canReserve = true;

            using (IDataReader reader = database.ExecuteReader("dbo.UserHasReserveAtThisTime", user_id, start, end))
            {
                while (reader.Read())
                {
                    canReserve = false;
                }
            }
            return canReserve;
          
        }


        public List<string> getTags(int id_publicacion)
        {
            var database = DatabaseFactory.CreateDatabase("SAB");
            var tags = new List<string>();


            using (IDataReader reader = database.ExecuteReader("dbo.getTagsByPublication", id_publicacion))
            {
                while (reader.Read())
                {
                    string t = Convert.ToString(reader["NOMBRE"]);
                    tags.Add(t);
                }

                return tags;
            }
        }
    }
}
