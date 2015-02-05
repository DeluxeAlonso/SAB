using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using SAB.Domain.User;

namespace SAB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

    }

    public class BasicAuthAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user =  HttpContext.Current.Session["usuario"];
            if (user == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}
