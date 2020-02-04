using System.Web;
using System.Web.Optimization;

namespace AwesomeDesk
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js").
                        Include("~/Scripts/ROScripts.js").
                        Include("~/Scripts/datatables.min.js").
                          Include("~/Scripts/Moment.js").
                        Include("~/Scripts/tempusdominus-bootstrap-4.min.js").
                        Include("~/Scripts/moment.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/popper").Include("~/Scripts/umd/popper.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(

                 "~/Scripts/umd/popper.js",
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/jquery.dataTables.min.css",
                      "~/Content/tempusdominus-bootstrap-4.min.css",
                       "~/Content/fontawesome.css"

                      ));
        }
    }
}