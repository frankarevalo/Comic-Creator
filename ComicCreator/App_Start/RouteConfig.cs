using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ComicCreator
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Index", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SelectPanel",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "CreatorController", action = "SelectPanel", id = UrlParameter.Optional }
            );

        }
    }
}