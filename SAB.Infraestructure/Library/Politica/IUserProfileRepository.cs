using SAB.Domain.Politica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base.Politica
{
    public interface IUserProfileRepository :IRepository<UserProfile>
    {
        void InsertPrivilegios(string nameUseProfile, int idAccion);
        void InsertPublicaciones(string nameUseProfile, int idTipoPublicacion);

        IEnumerable<UserProfile> Search( string ActionSelected,string PublicactionSelected, string searchCode, string searchName);
        void InsertDevolucionPerfil(string description, DateTime fechaDesde, DateTime fechaHasta, int cantDias, string searchName);
        void Actualiza(int id, string name, int maxMaterial, int day, string description);

        IEnumerable<UserProfile> QueryAll(int p);
    }
}
