using System.Web;
using System.Web.Optimization;

namespace EarlySite.Web
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Themes/assembly/jquery").Include(
                        "~/Themes/expandAssembly/jquery-{version}.js",
                        "~/Themes/expandAssembly/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/Themes/assembly/bootstrap").Include(
                      "~/Themes/expandAssembly/bootstrap.js",
                      "~/Themes/expandAssembly/respond.js",
                      "~/Themes/expandAssembly/bootstrap-select/bootstrap-select.min.js",
                      "~/Themes/expandAssembly/bootstrap-select/defaults-zh_CN.min.js"));

            bundles.Add(new ScriptBundle("~/Themes/Scripts").Include(
                      "~/Themes/Scripts/earlysiteCommon.js",
                      "~/Themes/Scripts/amapScript.js"));


            bundles.Add(new StyleBundle("~/Themes/Style").Include(
                      "~/Themes/Style/bootstrap.css",
                      "~/Themes/Style/bootstrap-select.min.css",
                      "~/Themes/Style/site.css",
                      "~/Themes/Style/common.css"));
        }
    }
}
