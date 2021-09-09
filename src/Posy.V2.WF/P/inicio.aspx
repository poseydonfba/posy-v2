<%@ Page Title="" Language="C#" MasterPageFile="~/SitePrincipal.Master" AutoEventWireup="true" CodeBehind="inicio.aspx.cs" Inherits="Posy.V2.WF.P.inicio" %>

<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common" %>
<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common.Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="ms-cnt-blc ms-cnt-inicio">
        <div class="view-editor"></div>
        <div class="ms-cnt-tit-blc">
            <h1 class="ms-tit-blc-txt icon-inicio"><%= UIConfig.TodasAsAtualizacoes %></h1>
            <div class="ms-tit-blc-line"></div>
        </div>
        <div class="view-btntop"></div>
        <div class="view-page"></div>
    </div>

    <div class="view-btnvermais"></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntJs" runat="server">
<%--    <script src="<%= Funcao.ResolveServerUrl("~/Scripts/app-connect-signalr.js", false) %>" type="text/javascript"></script>--%>
    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/p/inicio.js", false) %>" type="text/javascript"></script>
</asp:Content>
