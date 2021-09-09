<%@ Page Title="" Language="C#" MasterPageFile="~/SitePrincipal.Master" AutoEventWireup="true" CodeBehind="videos.aspx.cs" Inherits="Posy.V2.WF.P.videos" %>

<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common" %>
<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common.Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="ms-cnt-blc ms-cnt-videos">
    
        <div class="view-titulo">
            <div class="ms-cnt-tit-blc">
                <h1 class="ms-tit-blc-txt icon-video"><%= UIConfig.Videos %></h1>
                <div class="ms-tit-blc-line"></div>
            </div>
        </div>

        <div class="view-form"></div>

    <!---- VIEW VIDEO [INI] ----------------------------------------------------------------------------------------------------------->

        <div class="poseydon-video-view" codvideo="-1">
		    <div class="image-view-foto">
			    <iframe class="video-iframe" src="" frameborder="0" height="375" width="100%"></iframe>
                <div class="video-caption">
				    <h3></h3>
			    </div>
		    </div>
		    <div class="image-view-cmt">
                <div class="ed-cnt-cmt" style="padding:7px;margin:0px auto;">
                    <div class="ed-cnt-cmt-coments"></div>
                </div>
		    </div>
        </div>

    <!---- VIEW VIDEO [FIM] ----------------------------------------------------------------------------------------------------------->

        <div class="view-pager vp1"></div>
        <div class="view-page"></div>
        <div class="view-pager vp2"></div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntJs" runat="server">

    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/p/videos.js", false) %>" type="text/javascript"></script>

</asp:Content>

