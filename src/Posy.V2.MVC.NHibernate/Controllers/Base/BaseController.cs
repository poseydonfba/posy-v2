using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Identity.Extensions;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace Posy.V2.MVC.Controllers.Base
{
    public class BaseController : Controller
    {
        protected readonly IGlobalBaseController _globalBaseController;
        protected readonly IUnitOfWork _uow;
        protected readonly ICacheService _cacheService;
        protected readonly IPerfilService _perfilService;

        protected string BTN_VER_MAIS_COMENTS = UIConfig.VerMaisComentarios;
        protected string BTN_VER_MENOS_COMENTS = UIConfig.VerMenosComentarios;
        protected static int PAGE_NUMBER { get; set; }
        protected static int TOPICO_NUMBER { get; set; }

        protected string urlEncryptadaPerfil;
        protected string urlEncryptadaCmm;
        protected string urlResolve;

        public static Perfil PerfilLogged { get; set; }
        public static Perfil PerfilView { get; set; }
        public static Comunidade ComunidadeView { get; set; }

        public BaseController(IGlobalBaseController globalBaseController,
                              IUnitOfWork uow,
                              ICacheService cacheService,
                              IPerfilService perfilService)
        {
            _globalBaseController = globalBaseController;
            _uow = uow;
            _cacheService = cacheService;
            _perfilService = perfilService;
        }

        protected void VerifyCultureInfo(RequestContext requestContext)
        {
            var user = requestContext.HttpContext.User.Identity as ClaimsIdentity;
            var culturePreferences = user.GetCulture();
            if (culturePreferences == null)
                return;

            var uiCulture = new CultureInfo(culturePreferences.Language);
            var culture = new CultureInfo(culturePreferences.Language);

            if (!string.IsNullOrEmpty(culturePreferences.ShortDateFormat))
                culture.DateTimeFormat.ShortDatePattern = culturePreferences.ShortDateFormat;
            if (!string.IsNullOrEmpty(culturePreferences.LongDateFormat))
                culture.DateTimeFormat.LongDatePattern = culturePreferences.LongDateFormat;
            if (!string.IsNullOrEmpty(culturePreferences.CurrencySymbol))
                culture.NumberFormat.CurrencySymbol = culturePreferences.CurrencySymbol;

            /// Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = uiCulture;
        }

        public Perfil GetByIsOrAliasPerfil(string idview)
        {
            if (int.TryParse(idview, out int id))
                return _perfilService.Obter(id);

            return _perfilService.Obter(idview);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_uow != null)
                    _uow.Dispose();

                if (_perfilService != null)
                    _perfilService.Dispose();

                //if (_cacheService != null)
                //    _cacheService.Dispose();

                if (_globalBaseController != null)
                    _globalBaseController.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}