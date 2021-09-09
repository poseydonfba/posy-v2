using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Posy.V2.MVC.Validacao
{
    public class CmmValidacao
    {
        private Perfil PerfilLogado { get; set; }
        private Comunidade ComunidadeView { get; set; }

        public bool IsDono { get; set; }
        public bool IsModerador { get; set; }
        public bool IsMembro { get; set; }
        public bool IsPublica { get; set; }

        public CmmValidacao(Perfil perfilLogado, Comunidade comunidadeView)
        {
            PerfilLogado = perfilLogado;
            ComunidadeView = comunidadeView;

            if (ComunidadeView == null)
                return;

            var _membroService = DependencyResolver.Current.GetService<IMembroService>();
            var membro = _membroService.Obter(ComunidadeView.Id, PerfilLogado.Id);

            IsDono = (ComunidadeView != null) ? ((ComunidadeView.UsuarioId == PerfilLogado.Id) ? true : false) : false;
            IsModerador = false;
            IsMembro = (IsDono) ? false : (membro != null && membro.StatusSolicitacao == StatusSolicitacaoMembroComunidade.Aprovado) ? true : false;
            IsPublica = (ComunidadeView.TipoComunidade == TipoComunidade.Publica) ? true : false;
        }
    }
}