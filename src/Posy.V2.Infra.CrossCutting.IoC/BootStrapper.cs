using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using Posy.V2.Infra.CrossCutting.Identity.Context;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using Posy.V2.Infra.Data.Context;
using Posy.V2.Infra.Data.Repository;
using Posy.V2.Infra.Data.UnitOfWork;
using Posy.V2.Service;
using SimpleInjector;
using System;

namespace Posy.V2.Infra.CrossCutting.IoC
{
    public class BootStrapper
    {
        public static void RegisterServices(Container container, Lifestyle hybrid)
        {
            container.Register<DatabaseContext, DatabaseContext>(hybrid);
            container.Register<IUnitOfWork, UnitOfWork>(hybrid);
            //container.Register<ICurrentUser<ApplicationUser, Guid>, CurrentUser>(hybrid);
            container.Register<ICurrentUser, CurrentUser>(hybrid);
            container.Register<ICacheService, RedisCacheProvider>(hybrid);

            container.Register<IUsuarioRepository, UsuarioRepository>(hybrid);
            container.Register<IUsuarioService, UsuarioService>(hybrid);
            container.Register<IAmizadeRepository, AmizadeRepository>(hybrid);
            container.Register<IAmizadeService, AmizadeService>(hybrid);
            container.Register<IPerfilRepository, PerfilRepository>(hybrid);
            container.Register<IPerfilService, PerfilService>(hybrid);
            container.Register<IVisitantePerfilRepository, VisitantePerfilRepository>(hybrid);
            container.Register<IVisitantePerfilService, VisitantePerfilService>(hybrid);
            container.Register<IPostPerfilRepository, PostPerfilRepository>(hybrid);
            container.Register<IPostPerfilService, PostPerfilService>(hybrid);
            container.Register<IPostPerfilBloqueadoRepository, PostPerfilBloqueadoRepository>(hybrid);
            container.Register<IPostPerfilBloqueadoService, PostPerfilBloqueadoService>(hybrid);
            container.Register<IPostOcultoRepository, PostOcultoRepository>(hybrid);
            container.Register<IPostOcultoService, PostOcultoService>(hybrid);
            container.Register<IPostPerfilComentarioRepository, PostPerfilComentarioRepository>(hybrid);
            container.Register<IPostPerfilComentarioService, PostPerfilComentarioService>(hybrid);
            container.Register<IPrivacidadeRepository, PrivacidadeRepository>(hybrid);
            container.Register<IPrivacidadeService, PrivacidadeService>(hybrid);
            container.Register<IRecadoRepository, RecadoRepository>(hybrid);
            container.Register<IRecadoService, RecadoService>(hybrid);
            container.Register<IRecadoComentarioRepository, RecadoComentarioRepository>(hybrid);
            container.Register<IRecadoComentarioService, RecadoComentarioService>(hybrid);
            container.Register<IVideoRepository, VideoRepository>(hybrid);
            container.Register<IVideoComentarioRepository, VideoComentarioRepository>(hybrid);
            container.Register<IVideoService, VideoService>(hybrid);
            container.Register<IVideoComentarioService, VideoComentarioService>(hybrid);
            container.Register<IDepoimentoRepository, DepoimentoRepository>(hybrid);
            container.Register<IDepoimentoService, DepoimentoService>(hybrid);
            container.Register<IComunidadeRepository, ComunidadeRepository>(hybrid);
            container.Register<IComunidadeService, ComunidadeService>(hybrid);
            container.Register<IMembroRepository, MembroRepository>(hybrid);
            container.Register<IConnectionService, ConnectionService>(hybrid);
            container.Register<IConnectionRepository, ConnectionRepository>(hybrid);
            container.Register<IStorieService, StorieService>(hybrid);
            container.Register<IStorieRepository, StorieRepository>(hybrid);

            container.Register<IMembroService, MembroService>(hybrid);
            //container.RegisterDecorator<IMembroService, ComunidadeService>(hybrid);

            container.Register<IModeradorRepository, ModeradorRepository>(hybrid);
            container.Register<IModeradorService, ModeradorService>(hybrid);
            container.Register<ITopicoRepository, TopicoRepository>(hybrid);
            container.Register<ITopicoService, TopicoService>(hybrid);
            container.Register<ITopicoPostRepository, TopicoPostRepository>(hybrid);
            container.Register<ITopicoPostService, TopicoPostService>(hybrid);

            container.Register<IUserStore<ApplicationUser, Guid>>(
                () => new UserStore<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(new ApplicationDbContext()),
                hybrid);
            container.Register<IRoleStore<ApplicationRole, Guid>>(
                () => new RoleStore<ApplicationRole, Guid, ApplicationUserRole>(new ApplicationDbContext()),
                hybrid);
            container.Register<ApplicationRoleManager>(hybrid);
            container.Register<ApplicationUserManager>(hybrid);
            container.Register<ApplicationSignInManager>(hybrid);


            //container.Register<IAuthenticationManager, AuthenticationManager>(hybrid);
        }
    }
}
