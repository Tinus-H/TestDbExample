using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using ExpressMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TestWebApp.Common.Dtos;
using TestWebApp.Models.Entities;

namespace TestWebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Expressmapper

            Mapper.Register<InheritedSharedEntity, InheritedSharedEntityDto>()
                .Include<InheritedStringEntity, InheritedStringEntityDto>()
                .Include<InheritedIntEntity, InheritedIntEntityDto>()
                .Include<InheritedBoolEntity, InheritedBoolEntityDto>();

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.JsonFormatter ?? new JsonMediaTypeFormatter()
            {
                SerializerSettings = new JsonSerializerSettings()
            };
            config.Formatters.Remove(jsonFormatter);

            jsonFormatter.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include; //Ignore
            jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            jsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            jsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            jsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            jsonFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.All;
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.Formatters.Insert(0, jsonFormatter);
        }
    }
}
