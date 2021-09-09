<%@ Page Title="" Language="C#" MasterPageFile="~/SitePrincipal.Master" AutoEventWireup="true" CodeBehind="editperfil.aspx.cs" Inherits="Posy.V2.WF.P.editperfil" %>

<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common" %>
<%@ Import Namespace="Posy.V2.Infra.CrossCutting.Common.Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div class="ms-cnt-blc ms-cnt-cadperfil">

        <div class="ms-cnt-tit-blc">
            <h1 class="ms-tit-blc-txt icon-edit">Dados do Perfil</h1>
            <div class="ms-tit-blc-line"></div>
        </div>

        <!-- TABS INI ----------------------------------------------------------------------------------------------------------------------------->

        <div class="tabs pnlEditPerfil" style="display: none;">
            <table class="tabs-table-label">
                <tr>
                    <td>
                        <label class="tabs-tab-label tabs-tab-label-selected" idcontent="content-1">Dados Pessoais</label></td>
                    <td>
                        <label class="tabs-tab-label" idcontent="content-2">Foto do Perfil</label></td>
                    <td>
                        <label class="tabs-tab-label" idcontent="content-3">Privacidade</label></td>
                </tr>
            </table>

            <div class="clear-shadow"></div>

            <div class="tabs-content" style="min-height: 300px;">
                <div class="tabs-tab-content content-1" style="display: block;">

                    <!-- FORM INI ----------------------------------------------------------------------------------------------------------------------------->

                    <div id="containerPerfilCad" class="fm-cnt-default">

                        <div class="box-form">
                            <fieldset>

                                <div class="half-width">
                                    <label for="txtNome">Nome</label>
                                    <input type="text" id="txtNome" name="txtNome" maxlength="15" class="obrigatorio" />
                                </div>

                                <div class="half-width">
                                    <label for="txtSobrenome">Sobrenome</label>
                                    <input type="text" id="txtSobrenome" name="txtSobrenome" maxlength="15" class="obrigatorio" />
                                </div>

                                <div class="half-width">
                                    <div>
                                        <p class="half-width" style="width: 100%;">
                                            <label>País</label>
                                            <b>
                                                <span class="cd-select" style="width: 100%;">
                                                    <select name="dropPais" id="dropPais" style="width: 100%;">
                                                        <option value="pt-BR">Brasil</option>
                                                        <option value="en-US">English</option>
                                                    </select>
                                                </span>
                                            </b>
                                        </p>
                                    </div>
                                </div>

                                <div class="half-width">
                                    <div>
                                        <p class="half-width" style="width: 100%;">
                                            <label>Data de nascimento</label>
                                            <b style="display: block;">
                                                <span class="cd-select">
                                                    <select name="dropDNDia" id="dropDNDia">
                                                    </select>
                                                </span>

                                                <span class="cd-select">
                                                    <select name="dropDNMes" id="dropDNMes">
                                                    </select>
                                                </span>

                                                <span class="cd-select">
                                                    <select name="dropDNAno" id="dropDNAno">
                                                    </select>
                                                </span>
                                            </b>
                                        </p>
                                    </div>
                                </div>

                                <div class="half-width">
                                    <div>
                                        <p class="half-width" style="width: 100%;">
                                            <label>Sexo</label>
                                            <b>
                                                <span class="cd-select" style="width: 100%;">
                                                    <select name="dropSexo" id="dropSexo" style="width: 100%;">
                                                        <option value="M">Masculino</option>
                                                        <option value="F">Feminino</option>
                                                    </select>
                                                </span>
                                            </b>
                                        </p>
                                    </div>
                                </div>

                                <div class="half-width">
                                    <div>
                                        <p class="half-width" style="width: 100%;">
                                            <label>Estado Civil</label>
                                            <b>
                                                <span class="cd-select" style="width: 100%;">
                                                    <select name="dropEstadoCivil" id="dropEstadoCivil" style="width: 100%;">
                                                        <option value="Solteiro">Solteiro</option>
                                                        <option value="Casado">Casado</option>
                                                        <option value="Divorciado">Divorciado</option>
                                                        <option value="Namorando">Namorando</option>
                                                        <option value="Noivo">Noivo</option>
                                                    </select>
                                                </span>
                                            </b>
                                        </p>
                                    </div>
                                </div>

                                <div class="half-width">
                                    <label for="txtNomeId">Nome para url amigável</label>
                                    <input type="text" id="txtNomeId" name="txtNomeId" maxlength="20" />
                                </div>

                                <div class="half-width" style="width: 100%;">
                                    <label for="txtFrase">Frase do perfil</label>
                                    <input type="text" id="txtFrase" name="txtFrase" maxlength="200" />
                                </div>

                                <div class="half-width" style="width: 100%;">
                                    <label for="userEmail">Perfil</label>

                                    <div class="ed-cnt-cmt" style="margin-top: 5px;">
                                    </div>
                                    <!-- end of comments container "ed-cnt-cmt" -->

                                </div>

                            </fieldset>

                            <div class="ed-cnt-cmt-btn">
                                <a href="javascript:void(0);" class="button big icon approve primary btnSalvarDadosPessoais">Salvar Dados Pessoais</a>
                            </div>
                        </div>

                    </div>

                    <!-- FORM FIM ----------------------------------------------------------------------------------------------------------------------------->

                </div>
                <div class="tabs-tab-content content-2">

                    <!-- FORM INI ----------------------------------------------------------------------------------------------------------------------------->

                    <div class="fm-cnt-default">
                        <fieldset>

                            <div class="half-width">
                                <label for="txtNome">&nbsp;</label>

                                <div id="filename"></div>
                                <div id="progress"></div>
                                <div id="progressBar"></div>

                                <input type="file" id="fileUploadFotoPerfil" name="file">
                            </div>

                        </fieldset>
                    </div>

                    <!-- FORM FIM ----------------------------------------------------------------------------------------------------------------------------->


                    <!-- CONTAINER CROP IMAGEM INI ----------------------------------------------------------------------------------------------------------------------------->

                    <div id="containerCropImgPerfil"></div>

                    <!-- CONTAINER CROP IMAGEM FIM ----------------------------------------------------------------------------------------------------------------------------->

                </div>
                <div class="tabs-tab-content content-3">

                    <div id="containerPerfilPrivacidade" class="fm-cnt-default">
                        <div class="box-form" action="">
                            <fieldset>

                                <div class="half-width">
                                    <div>
                                        <p class="half-width" style="width: 100%;">
                                            <label>Quem pode ver meus recados?</label>
                                            <b>
                                                <span class="cd-select" style="width: 100%;">
                                                    <select name="dropVerRecados" id="dropVerRecados" style="width: 100%;">
                                                        <option value="1">Apenas meus amigos</option>
                                                        <option value="0">Todos</option>
                                                    </select>
                                                </span>
                                            </b>
                                        </p>
                                    </div>
                                </div>

                                <div class="half-width">
                                    <div>
                                        <p class="half-width" style="width: 100%;">
                                            <label>Quem pode escrever recados para mim?</label>
                                            <b>
                                                <span class="cd-select" style="width: 100%;">
                                                    <select name="dropEscreverRecados" id="dropEscreverRecados" style="width: 100%;">
                                                        <option value="1">Apenas meus amigos</option>
                                                        <option value="0">Todos</option>
                                                    </select>
                                                </span>
                                            </b>
                                        </p>
                                    </div>
                                </div>

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

        </div>

        <!-- TABS FIM ----------------------------------------------------------------------------------------------------------------------------->

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntJs" runat="server">

    <script src="<%= Funcao.ResolveServerUrl("~/js/plugin/foto.crop.js", false) %>" type="text/javascript"></script>
    <script src="<%= Funcao.ResolveServerUrl("~/js/plugin/simpleUpload.min.js", false) %>" type="text/javascript"></script>
    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/p/foto.js", false) %>" type="text/javascript"></script>
    <script src="<%= Funcao.ResolveServerUrl("~/js-dev/p/cadperfil.js", false) %>" type="text/javascript"></script>

</asp:Content>
