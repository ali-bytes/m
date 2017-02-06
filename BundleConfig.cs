using System.Web;
using System.Web.Optimization;

namespace WoodStore.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/mainjs").Include(
                     //"~/plugins/jQuery/jquery-2.2.3.min.js",
                     //"~/Scripts/jquery-1.9.1.min.js",

                     "~/Scripts/bootstrap.min.js",
                     //"~/plugins/fastclick/fastclick.js",
                     "~/Scripts/app.min.js",
                     //"~/plugins/sparkline/jquery.sparkline.min.js",
                     //"~/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                     //"~/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                     //"~/plugins/slimScroll/jquery.slimscroll.min.js",
                     //"~/plugins/chartjs/Chart.min.js",
                     "~/Scripts/respond.js",
                     "~/Scripts/alertify.min.js",
                     "~/Scripts/main.js",
                     "~/Scripts/jquery.unobtrusive-ajax.min.js"
                     //"~/Scripts/perfect-scrollbar.jquery.min.js"
                     ));
            bundles.Add(new ScriptBundle("~/bundles/armainjs").Include(
                     "~/Scripts/bootstrap-rtl.min.js",
                     "~/Scripts/app.min.js",
                     "~/Scripts/respond.js",
                     "~/Scripts/alertify.min.js",
                     "~/Scripts/main.js",
                     "~/Scripts/jquery.unobtrusive-ajax.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/DataTables").Include(
                     "~/Content/DataTables/css/jquery.dataTables.min.css",
                     "~/Content/DataTables/css/buttons.dataTables.min.css",
                     "~/Content/DataTables/css/responsive.dataTables.min.css"));

            bundles.Add(new StyleBundle("~/Content/maincss").Include(
                   "~/Content/bootstrap.min.css",
                   //"~/plugins/jvectormap/jquery-jvectormap-1.2.2.css",
                   "~/Content/AdminLTE.min.css",
                   //"~/Content/skins/_all-skins.min.css",
                   //"~/Content/skins/skin-blue.min.css",
                   "~/Content/skins/skin-yellow-light.min.css",

                   "~/Content/alertifyjs/alertify.min.css",
                   "~/Content/font-awesome.min.css"
                   //"~/Content/perfect-scrollbar.min.css"
                   ));
            bundles.Add(new StyleBundle("~/Content/armaincss").Include(
                     "~/Content/bootstrap-rtl.min.css",
                     //"~/Content/ar/AdminLTE.min.css",
                     "~/Content/AdminLTE.min.css",
                     //"~/Content/ar/skin-blue.min.css",
                     "~/Content/skins/skin-yellow-light.min.css",
                    "~/Content/alertifyjs/alertify.min.css",
                    "~/Content/font-awesome.min.css"
                    ));

        }
    }
}
