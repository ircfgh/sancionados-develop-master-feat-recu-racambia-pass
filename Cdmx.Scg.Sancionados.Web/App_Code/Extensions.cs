using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cdmx.Scg.Sancionados.Web
{
    public class Extensions
    {
    }

    /// <summary>
    /// Decorador para validar peticiones ajax
    /// </summary>
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
        }
    }

    //[AttributeUsage(AttributeTargets.Method)]
    //public class AjaxOnlyAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        if (!filterContext.HttpContext.Request.IsAjaxRequest())
    //        {
    //            filterContext.HttpContext.Response.StatusCode = 404;
    //            filterContext.Result = new HttpNotFoundResult();
    //        }
    //        else
    //        {
    //            base.OnActionExecuting(filterContext);
    //        }
    //    }
    //}

    //public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    //{
    //    public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
    //    {
    //        return controllerContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
    //    }
    //}

}