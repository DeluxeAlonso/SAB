using SAB.Base.Assets;
using SAB.Domain.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Assets
{
    public class AssetsApplication
    {
        private readonly IAssetsRepository assetsRepository;

        public AssetsApplication(IAssetsRepository assetsRepository)
        {
            this.assetsRepository = assetsRepository;
        }

        public IEnumerable<Asset> QueryAll()
        {
            IEnumerable<Asset> _assetList = null;
            try
            {
                _assetList = assetsRepository.QueryAll(); 
            }
            catch (Exception)
            {

            }

            return _assetList;
        }

        public void Insert(Asset activo)
        {
            try
            {
                assetsRepository.Insert(activo);
            }
            catch (Exception)
            {

            }

        }


        public void Delete(int local_id)
        {
            try
            {

                assetsRepository.Delete(local_id);
            }
            catch (Exception)
            {

            }
        }

        public Asset QueryById(int id)
        {
            try
            {
               return assetsRepository.QueryById(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Update(Asset a)
        {
            try
            {
                assetsRepository.Update(a);
            }
            catch (Exception)
            {

            }
        }

        public IEnumerable<Asset> Search(int codigo, DateTime fechaD, DateTime fechaH, int tipoActivo)
        {
            IEnumerable<Asset> _assetList = null;
            try
            {
                _assetList = assetsRepository.Search(codigo, fechaD, fechaH, tipoActivo);
            }
            catch (Exception)
            {

            }

            return _assetList;
        }



        public void Delete(Asset asset)
        {
            try
            {
                 assetsRepository.Delete(asset);
            }
            catch (Exception)
            {

            }
        }
    }
}
