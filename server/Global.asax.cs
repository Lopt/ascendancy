
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Threading;

namespace server
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public enum Phases
        {
            Started,
            Init,
            Running,
            Pause,
            Exit,
        }

        public static Phases Phase = Phases.Started;


        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.config/{*pathInfo}");
            routes.IgnoreRoute("{resource}.xml/{*pathInfo}");

            routes.MapRoute(
                "Login",                                           
                "Login",
                new { controller = "HTTP", action = "Login" }  
            );

            routes.MapRoute(
                "LoadRegions",                                           
                "LoadRegions",
                new { controller = "HTTP", action = "LoadRegions" }  
            );

            routes.MapRoute(
                "DoActions",                                           
                "DoActions",
                new { controller = "HTTP", action = "DoActions" }  
            );



            routes.MapRoute(
                "Default",
                "{controller}",
                new { controller = "HTTP", action = "Error" }
            );

        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Start()
        {
            Phase = Phases.Init;
            var world = Core.Models.World.Instance;
            var controller = Core.Controllers.Controller.Instance;

            var api = server.control.APIController.Instance;

            controller.DefinitionManagerController = new server.control.DefinitionManagerController();
            world.AccountManager = new server.control.AccountManagerController();
            controller.RegionManagerController = new server.control.RegionManagerController();


				
            for (int threadNr = 0; threadNr < server.model.ServerConstants.ACTION_THREADS; ++threadNr)
            {
                Thread t = new Thread(new ParameterizedThreadStart(server.control.APIController.Instance.Worker));
                t.Start(threadNr);
            }

            Phase = Phases.Running;


            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

        }
    }
}
