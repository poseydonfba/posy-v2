using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.MVC.App_Start;
using System;
using System.Web.Mvc;

namespace Posy.V2.MVC.Validacao
{
    public class PerfilValidacao
    {
        private int IdUsuarioLogado { get; set; }
        private int IdUsuarioView { get; set; }

        public PerfilValidacao(int idUsuarioLogado, int idUsuarioView)
        {
            IdUsuarioLogado = idUsuarioLogado;
            IdUsuarioView = idUsuarioView;
        }

        public bool IsMeuPerfil()
        {
            return IdUsuarioLogado == IdUsuarioView;
        }

        public bool IsAmigo()
        {
            if (IsMeuPerfil())
                return false;

            var _amizadeService = DependencyResolver.Current.GetService<IAmizadeService>();
            //var _amizadeService = SimpleInjectorInitializer.container.GetInstance<IAmizadeService>();

            var amizade = _amizadeService.Obter(IdUsuarioLogado, IdUsuarioView);

            return amizade != null && amizade.StatusSolicitacao == SolicitacaoAmizade.Aprovado;
        }
    }
}