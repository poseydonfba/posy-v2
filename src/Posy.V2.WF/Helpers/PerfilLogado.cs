using Microsoft.AspNet.Identity;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using System;
using System.Web;

namespace Posy.V2.WF.Helpers
{
    public class PerfilLogado
    {
        //[Import] public IPerfilService _perfilService { get; set; }
        //[Import] public ICacheService _cacheService { get; set; }

        public static Perfil Perfil()
        {
            //var _container = HttpContext.Current.Application.GetContainer();
            //var _perfilServico = _container.Resolve<IPerfilServico>();

            //var usuarioId = HttpContext.Current.User.Identity.Name;
            var usuarioId = HttpContext.Current.User.Identity.GetUserId();

            Perfil _perfilLogado;

            var _cacheService = Global.container.GetInstance<ICacheService>();
            if (_cacheService.Get<Perfil>(usuarioId) == null)
            {
                var _perfilService = Global.container.GetInstance<IPerfilService>();
                _perfilLogado = FuncaoJson.CopyObj(_perfilService.Obter(Guid.Parse(usuarioId))) as Perfil;

                _cacheService.Set(usuarioId, _perfilLogado);
            }
            else
            {
                _perfilLogado = _cacheService.Get<Perfil>(usuarioId);
            }

            _perfilLogado.Foto = Funcao.ResolveServerUrl("~/img/perfil/" + _perfilLogado.UsuarioId.ToString() + "/1.jpg", false);

            return _perfilLogado;
        }
    }
}