using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Service;
using System;

namespace Posy.V2.WF.Validacao
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

            //IUnityContainer _container = HttpContext.Current.Application.GetContainer();
            //var _amizadeServico = _container.Resolve<IAmizadeServico>();

            var _amizadeService = Global.container.GetInstance<IAmizadeService>();
            var amizade = _amizadeService.Obter(IdUsuarioLogado, IdUsuarioView);

            return amizade != null && amizade.StatusSolicitacao == SolicitacaoAmizade.Aprovado;
        }

        //public static int ValidaLoadPagePerfil(string nomeId)
        //{
        //    int codperfil = -1;

        //    var container = HttpContext.Current.Application.GetContainer();
        //    IPerfilServico _perfilServico = container.Resolve<IPerfilServico>();

        //    //if (!string.IsNullOrWhiteSpace(nomeId))
        //    //{
        //    //    PerfilDao objDao = new PerfilDao();
        //    //    Perfil perfil;

        //    //    if (Funcao.IsNumeric(nomeId))
        //    //    {
        //    //        perfil = objDao.getObjeto(int.Parse(nomeId));
        //    //    }
        //    //    else
        //    //    {
        //    //        perfil = objDao.getObjeto(nomeId);
        //    //    }

        //    //    codperfil = (perfil == null) ? -1 : perfil.CodPerfil;
        //    //}

        //    return codperfil;
        //}

        //private PerfilValidacao ValidaViewPagePerfil(Guid p_codperfil_view)
        //{
        //    PerfilAmigoDao amigoDao = new PerfilAmigoDao();

        //    PerfilValidacao validacao = new PerfilValidacao();
        //    validacao.IsMeuPerfil = (CODPERFIL_LOGADO == p_codperfil_view) ? true : false;
        //    validacao.IsAmigo = (amigoDao.getAmigo(CODPERFIL_LOGADO, p_codperfil_view) == null) ? false : true;

        //    return validacao;
        //}
    }
}