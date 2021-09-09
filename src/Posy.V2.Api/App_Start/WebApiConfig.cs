//using Microsoft.AspNet.WebApi.MessageHandlers.Compression;
//using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Compressors;
//using Microsoft.Owin.Security.OAuth;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using System.Web.Http;

using System.Web.Http;
using System.Web.Http.Cors; // Nuget Microsoft.AspNet.WebApi.Cors
using Posy.V2.Api.Filters;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Compressors;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

// Usar esta versão para compressão
// Install-Package Microsoft.AspNet.WebApi.MessageHandlers.Compression -Version 1.3.0

// Configurar o cabeçalho adicionando "Accept-Encoding: gzip, deflate" antes de enviar a solicitação ao servidor. 
// Fiddler não adicioná-lo automaticamente, mas os navegadores sim. 

namespace Posy.V2.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configura compressão
            config.MessageHandlers.Insert(0,
                new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));

            // Remove o XML
            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            // Modifica a identação
            var jsonSettings = formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Formatting.None;// Indented;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Modifica a serialização
            formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            // CORS config
            var cors = new EnableCorsAttribute("*", "*", "get,post");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            SwaggerConfig.Register(config);

            // Enforce HTTPS
            //config.Filters.Add(new RequireHttpsAttribute());
        }
    }
}
