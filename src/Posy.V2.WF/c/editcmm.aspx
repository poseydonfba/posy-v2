<%@ Page Title="" Language="C#" MasterPageFile="~/SitePrincipal.Master" AutoEventWireup="true" CodeBehind="editcmm.aspx.cs" Inherits="Posy.V2.WF.c.editcmm" %>

<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common" %>
<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common.Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .grid figcaption p,
        .lblSobrenome,
        .ms-cnt-blc-video,
        .ms-cnt-blc-vis-recente,
        .ms-cnt-blc-amigos,
        .ms-cnt-blc-cmm {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="ms-cnt-blc ms-cnt-cadcmm">

        <div class="ms-cnt-tit-blc">
            <h1 class="ms-tit-blc-txt icon-cmm">Criar Comunidade</h1>
            <div class="ms-tit-blc-line"></div>
        </div>

        <!-- TABS INI ----------------------------------------------------------------------------------------------------------------------------->

        <div class="tabs pnlCadCmm" style="display: block;">
            <table class="tabs-table-label">
                <tr>
                    <td><label class="tabs-tab-label tabs-tab-label-selected" idcontent="content-1">Dados da Comunidade</label></td>
                    <td style="display: none;"><label class="tabs-tab-label" idcontent="content-2">Foto da Comunidade</label></td>
                    <td style="display: none;"><label class="tabs-tab-label" idcontent="content-3">Privacidade</label></td>
                </tr>
            </table>

            <div class="clear-shadow"></div>

            <div class="tabs-content" style="min-height: 300px;">
                <div class="tabs-tab-content content-1" style="display: block;">

                    <!-- FORM INI ----------------------------------------------------------------------------------------------------------------------------->

                    <div id="formDados" class="fm-cnt-default">

                        <div class="box-form">
                            <fieldset>

                                <div class="half-width">
                                    <label for="txtNome">Nome da comunidade</label>
                                    <input type="text" id="txtNome" name="txtNome" maxlength="100" class="obrigatorio" />
                                </div>

                                <div class="half-width">
                                    <div>
                                        <p class="half-width" style="width: 100%;">
                                            <label>Categoria</label>
                                            <b>
                                                <span class="cd-select" style="width: 100%;">
                                                    <select name="dropCategoria" id="dropCategoria" style="width: 100%;">
                                                        <option value="1">Amizades</option>
                                                        <option value="2">Relacionamentos</option>
                                                    </select>
                                                </span>
                                            </b>
                                        </p>
                                    </div>
                                </div>

                                <div class="half-width" style="width: 100%;">
                                    <label for="userPerfilCmm">Perfil da Comunidade</label>

                                    <div class="ed-cnt-cmt" style="margin-top: 5px;">
                                    </div>
                                    <!-- end of comments container "ed-cnt-cmt" -->

                                </div>

                            </fieldset>

                            <div class="ed-cnt-cmt-btn">
                                <div class="button-group">
                                    <a href="javascript:void(0);" class="button big icon approve primary btnSalvarDadosCmm">Salvar Dados da Comunidade</a>
                                    <a href="javascript:void(0);" class="button big icon trash danger btnExcluirCmm">Excluir Comunidade</a>
                                </div>
                            </div>

                        </div>

                    </div>
                    <!-- .fm-cnt-default -->

                    <!-- FORM FIM ----------------------------------------------------------------------------------------------------------------------------->

                </div>
                <div class="tabs-tab-content content-2">

                    <!-- FORM INI ----------------------------------------------------------------------------------------------------------------------------->

                    <div class="fm-cnt-default">

                        <div class="box-form">
                            <fieldset>

                                <div class="half-width">
                                    <label for="txtNome">&nbsp;</label>

                                    <div id="filename"></div>
                                    <div id="progress"></div>
                                    <div id="progressBar"></div>

                                    <input type="file" id="fileUploadFoto" name="file">
                                </div>

                            </fieldset>
                        </div>

                    </div>

                    <!-- FORM FIM ----------------------------------------------------------------------------------------------------------------------------->


                    <!-- CONTAINER CROP IMAGEM INI ----------------------------------------------------------------------------------------------------------------------------->

                    <div id="containerCropImg"></div>

                    <!-- CONTAINER CROP IMAGEM FIM ----------------------------------------------------------------------------------------------------------------------------->

                </div>
                <div class="tabs-tab-content content-3">

                    <div id="formPrivacidade" class="fm-cnt-default">

                        <div class="box-form">

                            <fieldset>

                                <div class="half-width">
                                    <div>
                                        <p class="half-width" style="width: 100%;">
                                            <label>Tipo</label>
                                            <b>
                                                <span class="cd-select" style="width: 100%;">
                                                    <select name="dropTipo" id="dropTipo" style="width: 100%;">
                                                        <option value="Pública">Pública</option>
                                                        <option value="Privada">Privada</option>
                                                    </select>
                                                </span>
                                            </b>
                                        </p>
                                    </div>
                                </div>

                                <!--div class="half-width">
					                <div>
						                <p class="half-width" style="width:100%;">
							                <label>Quem pode criar tópicos</label>
							                <b>
								                <span class="cd-select" style="width: 100%;">
									                <select name="dropEscreverRecados" id="dropEscreverRecados" style="width: 100%;">
										                <option value="1">Apenas membros</option>
                                                        <option value="0">Todos</option>
									                </select>
								                </span>
							                </b>
						                </p>
					                </div>
				                </div-->

                            </fieldset>

                            <fieldset>
                                <div class="ed-cnt-cmt-btn">
                                    <a href="javascript:void(0);" class="button big icon approve primary btnSalvarDadosPrivacidade">Salvar dados de privacidade</a>
                                </div>
                            </fieldset>

                        </div>

                    </div>

                </div>
            </div>

            <div class="ed-cnt-cmt-btn">
                <a href="javascript:void(0);" class="button big icon approve primary btnCriarComunidade">Criar Comunidade</a>
            </div>

        </div>

        <!-- TABS FIM ----------------------------------------------------------------------------------------------------------------------------->

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntJs" runat="server">

    <script src="<%= Funcao.ResolveServerUrl("~/js/plugin/foto.crop.js", false) %>" type="text/javascript"></script>
    <script src="<%= Funcao.ResolveServerUrl("~/js/plugin/simpleUpload.min.js", false) %>" type="text/javascript"></script>
    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/c/foto.js", false) %>" type="text/javascript"></script>
    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/c/editcmm.js", false) %>" type="text/javascript"></script>

</asp:Content>

