using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vidly
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Enable attribute routing (In order to use that instead of legacy type custom routing.
            routes.MapMvcAttributeRoutes();

            // ****************************************
            // Legacy way to create custom route.   
            //    In ASP.net MVC5, Microsoft introduced a cleaner and better way to create a custom route.
            //    Instead of creating the route here, we can create a custom route by using an attribute to the corresponding action.  
            //    To do this, you must enable attribute routing as done above with "routes.MapMvcAttributeRoutes();
            //    Next, go to the Controller, and apply the "Route" attribute... you can see this in the controller like this:   [Route("movies/released/{year}/{month:regex(\\d{2}):range(1, 12)}")]
            // ****************************************
            //routes.MapRoute(
            //    "MoviesByReleaseDate",
            //    "movies/released/{year}/{month}",
            //    new { controller = "Movies", action = "ByReleaseDate"},
            //    new { year = @"2015|2016", month = @"\d{2}" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
