using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.MVC.Controllers.Base
{
    public interface IGlobalBaseController : IDisposable
    {
        string GetTemplateMenuVerticalPerfilHtml(Perfil PerfilLogged, Perfil PerfilView);
        string GetTemplateVisitantesHtml(Perfil PerfilLogged, Perfil PerfilView);
        string GetTemplateAmigosHtml(Perfil PerfilLogged, Perfil PerfilView);
        string GetTemplateCmmHtml(Perfil PerfilLogged, Perfil PerfilView);

        string GetTemplateMenuVerticalComunidadeHtml(Comunidade ComunidadeView, Perfil PerfilLogged);
        string GetTemplateMembrosHtml(Comunidade ComunidadeView, Perfil PerfilLogged);
    }
}