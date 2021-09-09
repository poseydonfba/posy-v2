<%@ Page Title="" Language="C#" MasterPageFile="~/SitePrincipal.Master" AutoEventWireup="true" CodeBehind="topico.aspx.cs" Inherits="Posy.V2.WF.c.topico" %>

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
        .view-topico  {
            background:#fff;
            padding:30px;
        }
        .view-editor-post {
            background:#fff;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="ms-cnt-blc ms-cnt-topico">
        <div class="view-titulo">
            <div class="ms-cnt-tit-blc">
                <h1 class="ms-tit-blc-txt icon-cmm"></h1>
                <div class="ms-tit-blc-line"></div>
            </div>
        </div>
        <div class="view-topico"></div>        
        <div class="view-pager vp1"></div>
        <div class="view-page"></div>
        <div class="view-pager vp2"></div>
    </div>
    <div class="view-editor"></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntJs" Runat="Server">

    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/c/topicos.js", false) %>" type="text/javascript"></script>
    
</asp:Content>