using System.Web.Http;
using WebActivatorEx;
using Generic.API;
using Swashbuckle.Application;

namespace Generic.API
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "Generic API");
            }).EnableSwaggerUi(c =>
            {

            });
        }
    }
}
