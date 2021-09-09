using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Service;

namespace Posy.V2.WF.Validacao
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

            //IUnityContainer _container = HttpContext.Current.Application.GetContainer();
            //var _membroServico = _container.Resolve<IMembroServico>();

            var _membroService = Global.container.GetInstance<IMembroService>();
            var membro = _membroService.Obter(ComunidadeView.ComunidadeId, PerfilLogado.UsuarioId);

            IsDono = (ComunidadeView != null) ? ((ComunidadeView.UsuarioId == PerfilLogado.UsuarioId) ? true : false) : false;
            IsModerador = false;
            IsMembro = (IsDono) ? false : (membro != null && membro.StatusSolicitacao == StatusSolicitacaoMembroComunidade.Aprovado) ? true : false;
            IsPublica = (ComunidadeView.TipoComunidade == TipoComunidade.Publica) ? true : false;
        }
    }
}