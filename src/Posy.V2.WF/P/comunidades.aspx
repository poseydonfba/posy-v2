<%@ Page Title="" Language="C#" MasterPageFile="~/SitePrincipal.Master" AutoEventWireup="true" CodeBehind="comunidades.aspx.cs" Inherits="Posy.V2.WF.P.comunidades" %>

<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common" %>
<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common.Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="ms-cnt-blc ms-cnt-comunidades">
        <div class="ms-cnt-tit-blc">
            <h1 class="ms-tit-blc-txt icon-cmm"><%= UIConfig.Comunidades %></h1>
            <div class="ms-tit-blc-line"></div>
        </div>
        <div class="view-pager vp1"></div>
        <div class="view-page"></div>
        <div class="view-pager vp2"></div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntJs" runat="server">

    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/p/comunidades.js", false) %>" type="text/javascript"></script>

</asp:Content>

