using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts
            bundles.Add(new ScriptBundle("~/bundles/basejs").Include(
                        "~/Scripts/basejs/jquery-3.4.1.min.js",
                        "~/Scripts/basejs/popper.js",
                        "~/Scripts/basejs/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bubblejs").Include(
                        "~/Scripts/basejs/select2.min.js",
                        "~/Scripts/basejs/jquery.nicescroll.min.js",
                        "~/Scripts/basejs/bubble.min.js",
                        "~/Scripts/basejs/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                        "~/Scripts/pages/login.js"));

            bundles.Add(new ScriptBundle("~/bundles/bubblehub").Include(
                        "~/Scripts/pages/bubblehub.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                        "~/Scripts/basejs/jquery.signalR-2.4.1.min.js"));
            #endregion

            #region Styles
            bundles.Add(new StyleBundle("~/Content/basecss").Include(
                        "~/Content/css/basecss/bootstrap.min.css",
                        "~/Content/css/basecss/font-awesome.min.css",
                        "~/Content/css/basecss/basecss.css"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                       "~/Content/css/pages/iofrm-style.css",
                       "~/Content/css/pages/iofrm-theme.css",
                       "~/Content/css/pages/login.css"));

            bundles.Add(new StyleBundle("~/Content/bubblecss").Include(
                        "~/Content/css/basecss/animate.css",
                        "~/Content/css/basecss/select2.min.css",
                        "~/Content/css/basecss/perfect-scrollbar.css",
                        "~/Content/css/basecss/util.css",
                        "~/Content/css/basecss/bubble.min.css",
                        "~/Content/css/basecss/main.css"));
            #endregion
        }
    }
}