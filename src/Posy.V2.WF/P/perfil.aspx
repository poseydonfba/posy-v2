<%@ Page Title="" Language="C#" MasterPageFile="~/SitePrincipal.Master" AutoEventWireup="true" CodeBehind="perfil.aspx.cs" Inherits="Posy.V2.WF.P.perfil" %>

<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common" %>
<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common.Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="ms-cnt-blc ms-cnt-perfil">
        <div class="ms-cnt-tit-blc">
            <h1 class="ms-tit-blc-txt icon-perfil"><%= UIConfig.Perfil %></h1>
            <div class="ms-tit-blc-line"></div>
        </div>
        <div class="view-page"></div>
    </div>
    <br />
    <div class="ms-cnt-blc ms-cnt-depoimentos" style="display: none;">
        <div class="view-titulo">
            <div class="ms-cnt-tit-blc">
                <h1 class="ms-tit-blc-txt icon-depo">Depoimentos</h1>
                <div class="ms-tit-blc-line"></div>
            </div>
        </div>
        <div class="view-page-dep"></div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntJs" runat="server">

    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/p/perfil.js", false) %>" type="text/javascript"></script>

</asp:Content>