using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Cdmx.Scg.Sancionados.Web
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            
            BundleTable.EnableOptimizations = true;

#if DEBUG
           BundleTable.EnableOptimizations = false;
#endif

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/theme.main.css",
                    "~/Content/bootstrap-table.css",
                    "~/Content/nprogress.css",
                    //"~/Content/bootstrap-table-materialize.css",
                    "~/Content/site.css")
                );

            bundles.Add(new ScriptBundle("~/bundles/jshead")
                    .Include("~/Scripts/nprogress.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/jsmain").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.unobtrusive.js",
                    "~/Scripts/jquery.unobtrusive-ajax.js",
                    "~/Scripts/bootstrap.bundle.js",
                    "~/Scripts/bootstrap-table.js",
                    "~/Scripts/bootstrap-table-es-MX.js",
                    "~/Scripts/axios.js",
                    //"~/Scripts/themes/materialize/bootstrap-table-materialize.js",                    
                    "~/Scripts/modernizr-{version}.js",                    
                    "~/Scripts/metisMenu.js",
                    "~/Scripts/perfect-scrollbar.js",
                    "~/Scripts/wNumb.js",
                    "~/Scripts/toastr.min.js",
                    "~/Scripts/theme.main.js",
                    "~/Scripts/js-base.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/jsaccordion")
                    .Include("~/Scripts/theme.main.accordion.js")
                );
        }

    }
}

