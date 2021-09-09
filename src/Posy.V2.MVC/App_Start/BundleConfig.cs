using System.Web;
using System.Web.Optimization;

namespace Posy.V2.MVC
{
    public class BundleConfig
    {
        /**
        * For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        **/
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            /**
            * Use the development version of Modernizr to develop with and learn from. Then, when you're
            * ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            **/
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/css/account").Include(
                        "~/Content/animate.css",
                        "~/Content/principal/font-awesome.css",
                        "~/Content/cssgram.min.css",
                        "~/Content/principal/nt.notificacao.css",
                        "~/Content/principal/ac.account.css"));

            bundles.Add(new StyleBundle("~/Content/principal/css").Include(
                        "~/Content/animate.css",
                        "~/Content/principal/reset.css",
                        "~/Content/principal/ms.master.css",
                        "~/Content/principal/mn.menu.css",
                        "~/Content/principal/gl.galeria.css",
                        "~/Content/principal/pf.perfil.css",
                        "~/Content/principal/fm.form.css",
                        "~/Content/principal/bt.buttons.css",
                        "~/Content/principal/tt.tooltip.css",
                        "~/Content/principal/tl.timeline.css",
                        "~/Content/principal/ed.editor.css",
                        "~/Content/principal/ft.fotocrop.css",
                        "~/Content/principal/ls.lista.css",
                        "~/Content/principal/ld.loader.css",
                        "~/Content/principal/pg.paginacao.css",
                        "~/Content/principal/vd.video.css",
                        "~/Content/principal/nt.notificacao.css",
                        "~/Content/principal/sc.social.css",
                        "~/Content/principal/font-awesome.css",
                        "~/Content/principal/ch.chat.css",
                        "~/Content/principal/st.sticky.css"));
            //"~/Scripts/plugin/stories/zuck.css",
            //"~/Scripts/plugin/stories/skins/snapgram.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/account").Include(
                        "~/Scripts/principal/lib/classie.js",
                        "~/Scripts/principal/lib/notification.js",
                        "~/Scripts/principal/config.js"));

            bundles.Add(new ScriptBundle("~/bundles/principal").Include(
                        "~/Scripts/principal/lib/tipsy.js",
                        "~/Scripts/principal/lib/classie.js",
                        "~/Scripts/principal/lib/notification.js",
                        "~/Scripts/principal/lib/buttonloader.js",
                        "~/Scripts/principal/lib/editorhtml.js",
                        "~/Scripts/jquery.signalR-2.2.1.min.js",
                        "~/Scripts/jquery.sticky-kit.js",
                        "~/Scripts/principal/chat.js",
                        "~/Scripts/principal/config.js"));

            bundles.Add(new ScriptBundle("~/bundles/zuck").Include(
                        "~/Scripts/plugin/stories/zuck.js",
                        "~/Scripts/principal/stories.js"));

            bundles.Add(new ScriptBundle("~/bundles/perfil/edit").Include(
                        "~/Scripts/plugin/foto.crop.js",
                        "~/Scripts/plugin/simpleUpload.min.js",
                        "~/Scripts/principal/p/foto.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
