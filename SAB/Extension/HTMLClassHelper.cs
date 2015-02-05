using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAB.Extension
{
    public static class HTMLClassHelper
    {
        public static string MakeActiveClass(this UrlHelper urlHelper, string controller)
        {
            string result = "active";

            string controllerName = urlHelper.RequestContext.RouteData.Values["controller"].ToString();
            if (!controllerName.Equals(controller, StringComparison.OrdinalIgnoreCase))
            {
                result = null;
            }

            return result;
        }
    }
}