using System.Web.Optimization;
namespace CamajanSport.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            System.Web.Optimization.BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/js/HomeLayout").Include(
                       "~/js/bootstrap.js",              
                       "~/js/helpers/Utilities.js",                        
                       "~/js/plugins/jquery.validate/jquery.validate.js",          
                       "~/js/plugins/jquery.validate/messages_es.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/Home/Index").Include(
                      "~/js/Home/Index.js"));

            bundles.Add(new StyleBundle("~/Content/css/HomeLayout").Include(
                      "~/css/bootstrap.css",                  
                       "~/css/main.css",                         
                       "~/css/MembresiaBanner.css",            
                       "~/css/home.css",                             
                       "~/css/plugins/sweetalert/sweetalert.css",
                       "~/css/loading-gif.css"       
                      ));

            bundles.Add(new StyleBundle("~/Content/css/Slider").Include(
                      "~/css/slider.css"));

            bundles.Add(new StyleBundle("~/Content/css/Partials").Include(
                      "~/css/Partials/HomeExpertos.css"));

            bundles.Add(new StyleBundle("~/Content/css/Home/Picks").Include(
                     "~/css/Partials/HomeExpertos.css",
                     "~/css/Home/Picks.css"));
        }
    }
}