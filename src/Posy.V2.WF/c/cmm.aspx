<%@ Page Title="" Language="C#" MasterPageFile="~/SitePrincipal.Master" AutoEventWireup="true" CodeBehind="cmm.aspx.cs" Inherits="Posy.V2.WF.c.cmm" %>

<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common" %>
<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common.Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .grid figcaption p,
        .lblSobrenome,
        .ms-cnt-blc-video,
        .ms-cnt-blc-vis-recente {
            display: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="ms-cnt-blc ms-cnt-cmm">
        <div class="ms-cnt-tit-blc">
            <h1 class="ms-tit-blc-txt icon-cmm">Comunidade</h1>
            <div class="ms-tit-blc-line"></div>
        </div>
        <div class="view-page"></div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntJs" runat="server">

    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/c/cmm.js", false) %>" type="text/javascript"></script>

</asp:Content>
