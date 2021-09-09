<%@ Page Title="" Language="C#" MasterPageFile="~/SitePrincipal.Master" AutoEventWireup="true" CodeBehind="pesqcmm.aspx.cs" Inherits="Posy.V2.WF.c.pesqcmm" %>

<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common" %>
<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common.Resources" %>
<%@ Import Namespace="Posy.V2.WF.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .pf-foto-effect,
        .pf-foto-effect figcaption p,
        .lblSobrenome,
        .ms-cnt-blc-video,
        .ms-cnt-blc-vis-recente,
        .ms-cnt-blc-amigos {
            display: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="ms-cnt-blc ms-cnt-pesqcmm">

        <div class="ms-cnt-tit-blc">
            <h1 class="ms-tit-blc-txt icon-cmm"><%= UIConfig.Comunidades %></h1>
            <div class="ms-tit-blc-line"></div>
        </div>

        <!---- BOTOES INI---------------------------------------------------------------------------------------------------------------------->

        <div class="ed-cnt-cmt-btn">
            <a href="<%= Funcao.ResolveServerUrl("~/" + FuncaoSite.ROUTE_URL_CADCMM, false) %>" class="button big icon remove btnCriarCmm">Criar comunidade</a>
        </div>

        <!---- BOTOES INI---------------------------------------------------------------------------------------------------------------------->

        <div id="containerPesqCmmView"></div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntJs" runat="server">

    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/c/pesqcmm.js", false) %>" type="text/javascript"></script>

</asp:Content>
