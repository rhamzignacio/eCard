using System.Web;
using System.Web.Optimization;

namespace eCard
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-growl.min.js",
                "~/Scripts/select2.full.min.js",

                "~/App/App.js",
                "~/App/Controller/Login.js",
                "~/App/Controller/User.js"
                ));
        }
    }
}
