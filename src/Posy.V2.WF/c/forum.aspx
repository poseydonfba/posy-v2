<%@ Page Title="" Language="C#" MasterPageFile="~/SitePrincipal.Master" AutoEventWireup="true" CodeBehind="forum.aspx.cs" Inherits="Posy.V2.WF.c.forum" %>

<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common" %>
<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common.Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .grid figcaption p, 
        .lblSobrenome,
        .ms-cnt-blc-video,
        .ms-cnt-blc-vis-recente,
        .ms-cnt-blc-cmm  {
            display: none;
        }   
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="ms-cnt-blc ms-cnt-forum">
        <div class="view-titulo">
            <div class="ms-cnt-tit-blc">
                <h1 class="ms-tit-blc-txt icon-cmm">Forum</h1>
                <div class="ms-tit-blc-line"></div>
            </div>
        </div>
        <div class="view-form"></div>
        <div class="view-pager vp1"></div>
        <div class="view-page"></div>
        <div class="view-pager vp2"></div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntJs" Runat="Server">

    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/c/forum.js", false) %>" type="text/javascript"></script>

</asp:Content>