using System.Web;
using System.Web.Optimization;

namespace PanaceyaUser
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
          

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/styles.css"));
            bundles.Add(new ScriptBundle("~/bundles/jquerymaskedinput").Include("~/Scripts/jquery.maskedinput-{version}.js"));

        }
    }
}
