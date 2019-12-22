using System.Web.Mvc;
using Web.App_Start;

namespace Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            // Add sesion check filter for all action
            filters.Add(new SessionCheckActionFilterAttribute());
        }
    }
}