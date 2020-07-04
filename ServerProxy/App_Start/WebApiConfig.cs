using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ServerProxy
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Controller config
            config.MapHttpAttributeRoutes();

            //General Filters
            config.Filters.Add(new ValidateModelFilter());
        }
    }
}
