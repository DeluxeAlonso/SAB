using SAB.Base.Politica;
using SAB.Domain.Politica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Politica
{
    public class UserProfileApplication
    {
        private readonly IUserProfileRepository userProfileRepository;
               
        public UserProfileApplication(IUserProfileRepository userRepository)
        {
            this.userProfileRepository = userRepository;
        }
               
        public IEnumerable<UserProfile> QueryAll()
        {
            IEnumerable<UserProfile> _userProfileList = null;
            try
            {
                _userProfileList = userProfileRepository.QueryAll();
            }
            catch (Exception)
            {

            }

            return _userProfileList;
        }
        
        public UserProfile QueryById(int id)
        {
            UserProfile _userProfile = null;
            try
            {
                _userProfile = userProfileRepository.QueryById(id);
            }
            catch (Exception)
            {

            }

            return _userProfile;
        }

        public IEnumerable<UserProfile> Search( string ActionSelected, string PublicactionSelected, string searchCode, string searchName)
        {
            IEnumerable<UserProfile> _userProfileList = null;
            try
            {
                _userProfileList = userProfileRepository.Search(ActionSelected,PublicactionSelected,searchCode,searchName);
            }
            catch (Exception)
            {

            }

            return _userProfileList;
        }
                      
        public void Insert(UserProfile userProfile)
        {
            try
            {
                userProfileRepository.Insert(userProfile);
            }
            catch (Exception)
            {

            }
        }


        public void InsertPrivilegios(string nameUseProfile, int[] listAcciones)
        {
            try
            {
                for (int i = 0; i < listAcciones.Length; i++) {

                    userProfileRepository.InsertPrivilegios(nameUseProfile, listAcciones[i]);
                }
              
            }
            catch (Exception)
            {

            }
        }

        public void Actualiza(int id, string name, int maxMaterial, int day, string description){
             try
            {
                userProfileRepository.Actualiza(id, name, maxMaterial, day, description);
              
            }
            catch (Exception)
            {

            }
        
        
        
        }

        public void InsertPublication(string nameUseProfile, int[] list)
        {
            try
            {
                for (int i = 0; i < list.Length; i++)
                {

                    userProfileRepository.InsertPublicaciones(nameUseProfile, list[i]);
                }

            }
            catch (Exception)
            {

            }
        }

        
        public void InsertDevolucion(string nameUseProfile, string[] descriptions, int[] days, string[] fechaHasta, string[] fechaDesde)
        {
            try
            {
                for (int i = 0; i < descriptions.Length && descriptions[i]!=""; i++)
                {
                    DateTime desde = DateTime.ParseExact(fechaDesde[i],"yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime hasta = DateTime.ParseExact(fechaHasta[i],"yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    userProfileRepository.InsertDevolucionPerfil(descriptions[i],  desde, hasta, days[i], nameUseProfile);
                }

            }
            catch (Exception)
            {

            }
        }

        public void Delete(int id)
        {
            try
            {     UserProfile u= this.QueryById(id);
                  userProfileRepository.Delete(u);

            }
            catch (Exception)
            {

            }
        }

        
        public object QueryAll(int p)
        {
            IEnumerable<UserProfile> _userProfileList = null;
            try
            {
                _userProfileList = userProfileRepository.QueryAll(p);
            }
            catch (Exception)
            {

            }

            return _userProfileList;
        }
    }
}
