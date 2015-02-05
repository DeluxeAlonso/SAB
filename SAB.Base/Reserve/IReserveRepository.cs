using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Base;
using SAB.Domain.Reserves; 
using SAB.Domain.Publication;
using SAB.Domain.Assets;

namespace SAB.Base.Reserves
{
    public interface IReserveRepository :IRepository<Reserve>
    {

        void InsertPublication(Reserve reserve);
        void InsertCubicle(Reserve reserve);
        int Delete(int reserve);

        IEnumerable<Reserve> PublicationQueryAll(int id);
        IEnumerable<Reserve> CubiclesQueryAll(int id);

        int getReserveCantDay(int user_id);
        IEnumerable<PublicationItem> getItemsOfPublication(int publication_id,int id_biblioteca);

        IEnumerable<Reserve> SearchPublication(int id, DateTime start, DateTime end);

        int getCantReservesCanDo(int user_id);

        IEnumerable<Reserve> SearchCubiclesReserve(int id, DateTime start, DateTime end);

        bool userCanReserveAtThisTime(int p, DateTime start, DateTime end);

        List<string> getTags(int id_publicacion);
        List<Asset> searchCubicles(DateTime? start, DateTime? end, int quantity);
    }
}
