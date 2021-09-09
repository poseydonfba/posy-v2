using System.Web.Optimization; // Install-Package Microsoft.AspNet.Web.Optimization

namespace Posy.V2.WF
{
    public class BundleConfig
    {
        // For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                  "~/js-dev/lib/modernizr.min.js",
                  "~/js-dev/lib/jquery-2.1.1.min.js",
                  "~/js-dev/lib/tipsy.js",
                  "~/js-dev/lib/classie.js",
                  "~/js-dev/lib/notification.js",
                  "~/js-dev/lib/buttonloader.js",
                  "~/js-dev/config.js",
                  "~/js-dev/master.js",
                  "~/js-dev/lib/editorhtml.js"));

            //<script src="<%= Funcao.ResolveServerUrl("~/js-dev/lib/modernizr.min.js", false) %>" type="text/javascript"></!--script>
            //<script src="<%= Funcao.ResolveServerUrl("~/js-dev/lib/jquery-2.1.1.min.js", false) %>" type="text/javascript"></script>
            //<script src="<%= Funcao.ResolveServerUrl("~/js-dev/lib/tipsy.js", false) %>" type="text/javascript"></script>
            //<script src="<%= Funcao.ResolveServerUrl("~/js-dev/lib/classie.js", false) %>" type="text/javascript"></script>
            //<script src="<%= Funcao.ResolveServerUrl("~/js-dev/lib/notification.js", false) %>" type="text/javascript"></script>
            //<script src="<%= Funcao.ResolveServerUrl("~/js-dev/lib/buttonloader.js", false) %>" type="text/javascript"></script>
            //<script src="<%= Funcao.ResolveServerUrl("~/js-dev/config.js", false) %>" type="text/javascript"></script>
            //<script src="<%= Funcao.ResolveServerUrl("~/js-dev/master.js", false) %>" type="text/javascript"></script>
            //<script src="<%= Funcao.ResolveServerUrl("~/js-dev/lib/editorhtml.js", false) %>" type="text/javascript"></script>

            //<script src="<%= Funcao.ResolveServerUrl("~/Scripts/jquery.signalR-2.2.1.min.js", false) %>" type="text/javascript"></script>


            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/css/animate.css",
                "~/css-dev/reset.css",
                "~/css-dev/ms.master.css",
                "~/css-dev/mn.menu.css",
                "~/css-dev/gl.galeria.css",
                "~/css-dev/pf.perfil.css",
                "~/css-dev/fm.form.css",
                "~/css-dev/bt.buttons.css",
                "~/css-dev/tt.tooltip.css",
                "~/css-dev/tl.timeline.css",
                "~/css-dev/ed.editor.css",
                "~/css-dev/ft.fotocrop.css",
                "~/css-dev/ls.lista.css",
                "~/css-dev/ld.loader.css",
                "~/css-dev/pg.paginacao.css",
                "~/css-dev/vd.video.css",
                "~/css-dev/nt.notificacao.min.css",
                "~/css-dev/sc.social.css",
                "~/css-dev/font-awesome.css",
                "~/css-dev/ch.chat.css"));

            //<link href="<%= Funcao.ResolveServerUrl("~/css/animate.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/reset.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/ms.master.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/mn.menu.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/gl.galeria.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/pf.perfil.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/fm.form.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/bt.buttons.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/tt.tooltip.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/tl.timeline.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/ed.editor.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/ft.fotocrop.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/ls.lista.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/ld.loader.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/pg.paginacao.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/vd.video.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/nt.notificacao.min.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/sc.social.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/font-awesome.css", false) %>" rel="stylesheet" type="text/css" />
            //<link href="<%= Funcao.ResolveServerUrl("~/css-dev/ch.chat.css", false) %>" rel="stylesheet" type="text/css" />



            // O agrupamento e a minificação são ativados ou desativados ao definir o valor do atributo debug 
            // no Elemento de compilação no arquivo Web.config . 
            // No XML a seguir, debugé definido como true para que o agrupamento e a minificação sejam desativados.
            // <compilation debug="true" />

            // Para ativar o agrupamento e a minificação, defina o debugvalor como "falso".Você pode substituir 
            // a configuração Web.config com a propriedadeEnableOptimizations na BundleTableclasse. 
            // O código a seguir habilita o agrupamento e a minificação e substitui qualquer configuração no arquivo Web.config.

            BundleTable.EnableOptimizations = true;

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
            //      "~/Scripts/WebForms/WebForms.js",
            //      "~/Scripts/WebForms/WebUIValidation.js",
            //      "~/Scripts/WebForms/MenuStandards.js",
            //      "~/Scripts/WebForms/Focus.js",
            //      "~/Scripts/WebForms/GridView.js",
            //      "~/Scripts/WebForms/DetailsView.js",
            //      "~/Scripts/WebForms/TreeView.js",
            //      "~/Scripts/WebForms/WebParts.js"));

            //bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
            //    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
            //    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
            //    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
            //    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //    "~/Scripts/modernizr-*"));
        }
    }
}