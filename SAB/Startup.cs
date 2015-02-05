using Microsoft.Owin;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Owin;
using SAB.Base.Publication;
using SAB.Base.LoanInterface;
using SAB.Base.Acquisition;
using SAB.Infraestructure.Publication;
using SAB.Infraestructure.LoanInfraestructure;
using SAB.Infraestructure.Acquisition;
using SAB.Shared;

using SAB.Base.User;
using SAB.Infraestructure;

using SAB.Infraestructure.Reserves;
using SAB.Base.Reserves;
using SAB.Infraestructure.User;

using SAB.Base.Library;
using SAB.Infraestructure.Library;
using SAB.Base.Politica;
using SAB.Infraestructure.Politica;

using SAB.Base.Sanctions;
using SAB.Infraestructure.Sanctions;
using SAB.Base.Assets;
using SAB.Infraestructure.Assets;

[assembly: OwinStartupAttribute(typeof(SAB.Startup))]
namespace SAB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);

            /**********Login********/

            InstanceFactory.Instance.Register(typeof(IUserAccountRepository), typeof(UserAccountRepository));
            InstanceFactory.Instance.Register(typeof(IDocumentTypeRepository), typeof(DocumentTypeRepository));
            InstanceFactory.Instance.Register(typeof(IActionRepository), typeof(ActionRepository));
            InstanceFactory.Instance.Register(typeof(IUserProfileRepository), typeof(UserProfileRepository));
            InstanceFactory.Instance.Register(typeof(IDevolutionProfileRepository), typeof(DevolutionProfileRepository));


            /****** PUBLICATION ****/

            InstanceFactory.Instance.Register(typeof(ITopicRepository), typeof(TopicRepository));
            InstanceFactory.Instance.Register(typeof(IAuthorRepository), typeof(AuthorRepository));
            InstanceFactory.Instance.Register(typeof(IEditorialRepository), typeof(EditorialRepository));
            InstanceFactory.Instance.Register(typeof(IPublicationTypeRepository), typeof(PublicationTypeRepository));
            InstanceFactory.Instance.Register(typeof(IPublicationTitleRepository), typeof(PublicationTitleRepository));
            InstanceFactory.Instance.Register(typeof(IPublicationTopicRepository), typeof(PublicationTopicRepository)); 
            InstanceFactory.Instance.Register(typeof(IPublicationItemRepository), typeof(PublicationItemRepository));

            /***** ASSETS ****/
            InstanceFactory.Instance.Register(typeof(IAssetsRepository), typeof(AssetsRepository));
            InstanceFactory.Instance.Register(typeof(ITypeAssetRepository), typeof(TypeAssetRepository));


            /***** ACQUISITION ****/

            InstanceFactory.Instance.Register(typeof(IPurchaseOrderRepository), typeof(PurchaseOrderRepository));
            InstanceFactory.Instance.Register(typeof(IPurchaseRequestRepository), typeof(PurchaseRequestRepository));
            InstanceFactory.Instance.Register(typeof(IPurchaseOrderDetailRepository), typeof(PurchaseOrderDetailRepository));
            InstanceFactory.Instance.Register(typeof(IPurchaseRequestDetailRepository), typeof(PurchaseRequestDetailRepository));
            InstanceFactory.Instance.Register(typeof(ISupplierRepository), typeof(SupplierRepository));
            InstanceFactory.Instance.Register(typeof(ISuscriptionRepository), typeof(SuscriptionRepository));
            InstanceFactory.Instance.Register(typeof(IRenewalRepository), typeof(RenewalRepository));

            /****** SERVICES ******/

            InstanceFactory.Instance.Register(typeof(ILoanRepository),typeof(LoanRepository));


            /****** RESERVES *****/

            InstanceFactory.Instance.Register(typeof(IReserveRepository), typeof(ReserveRepository));

            /****** LIBRARY ******/

            InstanceFactory.Instance.Register(typeof(ILocalRepository), typeof(LocalRepository));


            /***** SANCTIONS *****/

            InstanceFactory.Instance.Register(typeof(ISanctionRepository), typeof(SanctionRepository));

        }
    }
}
