using Posy.V2.Api;
using Swashbuckle.Application;
using System;
using System.Web;
using System.Web.Http;

// https://equipealumni.wordpress.com/2018/08/14/tutorial-de-configuracao-do-swagger-em-projetos-asp-net-web-api/
// Outras alterações devem ser feitas dentro da classe SwaggerConfig. 
// A princípio, retirar a diretiva no topo do arquivo que registra o Swagger 
// na aplicação. Essa remoção é feita para evitar um conflito futuro com a 
// classe de configuração do projeto Web
//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]
namespace Posy.V2.Api
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            //GlobalConfiguration.Configuration 
            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Posy.V2.Api")
                    .Description("Documentação ativa da API do sistema Posy.V2");
                    c.IncludeXmlComments(GetXmlCommentsPath());
                    //c.Schemes(new[] { "http" });
                    //c.BasicAuth("Basic");
                    c.RootUrl(req => new Uri(req.RequestUri, HttpContext.Current.Request.ApplicationPath ?? string.Empty).ToString());
                })
                .EnableSwaggerUi(c =>
                {
                });
        }

        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\Swagger.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
