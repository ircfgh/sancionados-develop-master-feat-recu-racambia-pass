using System.Web.Mvc;

namespace Cdmx.Scg.Sancionados.Web.Areas.Catalogos
{
    public class CatalogosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Catalogos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Catalogos_default",
                "Catalogos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}