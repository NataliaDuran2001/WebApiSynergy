using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiV2.Controllers;

namespace WebApiV2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración de rutas y servicios de API web

            // Rutas de API web
            config.EnableCors();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //handler de validación de tokens
            config.MessageHandlers.Add(new TokenValidationHandler());


        }
    }
}
