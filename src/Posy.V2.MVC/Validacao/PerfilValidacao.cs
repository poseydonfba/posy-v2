using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Service;
using System;
using System.Web.Mvc;

namespace Posy.V2.MVC.Validacao
{
    public class PerfilValidacao
    {
        private Guid IdUsuarioLogado { get; set; }
        private Guid IdUsuarioView { get; set; }

        public PerfilValidacao(Guid idUsuarioLogado, Guid idUsuarioView)
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