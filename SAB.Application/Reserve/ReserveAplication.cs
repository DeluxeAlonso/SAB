using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAB.Domain.Reserves;
using SAB.Base.Reserves;
using SAB.Domain.Publication;
using SAB.Base.Sanctions;
using SAB.Domain.User;
using SAB.Domain.Assets;
namespace SAB.Application.Reserves
{    
    public class ReserveAplication
    {
        private readonly IReserveRepository reserveRepository;
        private readonly ISanctionRepository sanctionRepository;

        public ReserveAplication(IReserveRepository reserveRepository, ISanctionRepository sanctionRepository)
        {
            this.reserveRepository = reserveRepository;
            this.sanctionRepository = sanctionRepository;
        }

        public IEnumerable<Reserve> SearchPublications(int id, DateTime start, DateTime end)
        {
            IEnumerable<Reserve> _publicationRepository = null;

            try
            {
                _publicationRepository = reserveRepository.SearchPublication(id, start, end);
            }
            catch (Exception)
            {

            }

            return _publicationRepository;
        }

        public IEnumerable<Reserve> SearchReservesCubicles(int id, DateTime start, DateTime end)
        {
            IEnumerable<Reserve> _c = null;

            try
            {
                _c = reserveRepository.SearchCubiclesReserve(id, start, end);
            }
            catch (Exception)
            {

            }

            return _c;
        }

        public int cancel(int id)
        {
            int _publicationRepository = 0;

            try
            {
                _publicationRepository = reserveRepository.Delete(id);
            }
            catch (Exception)
            {

            }

            return _publicationRepository;
        }


        public int getReserveCantDay(UserAccount u)
        {
            int cantdays= 0;

            try
            {
                cantdays = reserveRepository.getReserveCantDay(u.Id);
            }
            catch (Exception)
            {

            }

            return cantdays;

        }

        public IEnumerable<PublicationItem> getItemsOfPublication(int publication_id)
        {
            IEnumerable<PublicationItem> _publicationRepository = null;

            try
            {
                _publicationRepository = reserveRepository.getItemsOfPublication(publication_id, 0);
            }
            catch (Exception)
            {

            }

            return _publicationRepository;
        }

        public IEnumerable<PublicationItem> getItemsOfPublication(int publication_id, int id_biblioteca)
        {
            IEnumerable<PublicationItem> _publicationRepository = null;

            try
            {
                _publicationRepository = reserveRepository.getItemsOfPublication(publication_id, id_biblioteca);
            }
            catch (Exception)
            {

            }

            return _publicationRepository;
        }
        public IEnumerable<Reserve> PublicationQueryAll(int id)
        {
            IEnumerable<Reserve>  _publicationRepository = null;

            try
            {
                _publicationRepository = reserveRepository.PublicationQueryAll(id);
            }
            catch (Exception)
            { 
            
            }

            return _publicationRepository;
        }

        public IEnumerable<Reserve> CubiclesQueryAll(int id)
        {
            IEnumerable<Reserve> _c = null;

            try
            {
                _c = reserveRepository.CubiclesQueryAll(id);
            }
            catch (Exception)
            {

            }

            return _c;
        }
        public bool isSanctionUser(UserAccount u)
        {
            bool isSanction = false;
        
            try
            {
                isSanction = sanctionRepository.isSanctioned(u.Id);
            }
            catch 
            {

            }
            return isSanction;
        }

        public void makeReserve(Reserve r)
        {

            try
            {
                reserveRepository.InsertPublication(r);
            }
            catch (Exception)
            {

            }
        
        }

        public void reserveCubicle(Reserve r)
        {

            try
            {
                reserveRepository.InsertCubicle(r);
            }
            catch (Exception)
            {

            }

        }

        public bool userCanReserveAtThisTime(UserAccount u, DateTime start, DateTime end)
        {
            bool canReserve = true;

            try
            {
                canReserve = reserveRepository.userCanReserveAtThisTime(u.Id, start, end);
            }
            catch (Exception)
            {

            }


            return canReserve;
        
        }


        public int getQuantityReservesCanDo(UserAccount u)
        {
            int quantityCanReserve = 0;


            try
            {
               quantityCanReserve =  reserveRepository.getCantReservesCanDo(u.Id);
            }
            catch (Exception)
            {

            }

            return quantityCanReserve;

        }

        public List<string> getTags(int id_publicacion)
        {
            var tags = new List<string>();

            try
            {
                tags = reserveRepository.getTags(id_publicacion);
            }
            catch (Exception)
            {

            }

            return tags;
        
        }

        public List<Asset> searchCubicles(DateTime? start, DateTime? end,int  quantity)
        {
            var _reserves = new List<Asset>();

            try
            {
                _reserves = reserveRepository.searchCubicles(start, end, quantity);
            }
            catch (Exception)
            {

            }

            return _reserves;


        }
    }
}
