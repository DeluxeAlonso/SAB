using SAB.Base.Assets;
using SAB.Domain.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Assets
{
    public class TypeAssetApplication
    {

        private readonly ITypeAssetRepository typeAssetRepository;

        public TypeAssetApplication(ITypeAssetRepository typeAssetRepository)
        {
            this.typeAssetRepository = typeAssetRepository;
        }


        public IEnumerable<TypeAsset> QueryAll()
        {
            IEnumerable<TypeAsset> _assetList = null;
            try
            {
                _assetList = typeAssetRepository.QueryAll();
            }
            catch (Exception)
            {

            }

            return _assetList;
        }

        public void Insert(TypeAsset activo)
        {
            try
            {
                typeAssetRepository.Insert(activo);
            }
            catch (Exception)
            {

            }

        }


        public void Update(TypeAsset activo)
        {
            try
            {
                typeAssetRepository.Update(activo);
            }
            catch (Exception)
            {

            }

        }


        public void Delete(TypeAsset activo)
        {
            try
            {
                typeAssetRepository.Delete(activo);
            }
            catch (Exception)
            {

            }

        }





        public TypeAsset QueryById(int id)
        {
            TypeAsset tipo = null;
            try
            {
                tipo=typeAssetRepository.QueryById(id);
            }
            catch (Exception)
            {



            }

            return tipo;
        }

        public IEnumerable<TypeAsset> Search(int id, string name)
        {
            IEnumerable<TypeAsset> _assetList = null;
            try
            {
                _assetList = typeAssetRepository.Search(id,name);
            }
            catch (Exception)
            {

            }

            return _assetList;
        }
    }
}
