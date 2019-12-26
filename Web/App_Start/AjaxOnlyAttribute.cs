using System.Web.Mvc;

namespace Web.App_Start
{
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
	{
		public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
		{
			return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
		}
	}
}