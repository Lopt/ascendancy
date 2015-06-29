
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
//using System.Web.Routing;
using System.Threading;

namespace server
{
	public class MvcApplication //: System.Web.HttpApplication
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

        /*
		public static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.config/{*pathInfo}");
            routes.IgnoreRoute("{resource}.xml/{*pathInfo}");

			routes.MapRoute (
				"Login",                                           
				"Login",
				new { controller = "HTTP", action = "Login" }  
			);

			routes.MapRoute (
				"LoadRegions",                                           
				"LoadRegions",
				new { controller = "HTTP", action = "LoadRegions" }  
			);

            routes.MapRoute (
                "DoActions",                                           
                "DoActions",
                new { controller = "HTTP", action = "DoActions" }  
            );

            routes.MapRoute (
                "Wait",                                           
                "Wait",
                new { controller = "HTTP", action = "Wait" }  
            );


			routes.MapRoute (
				"Default",
				"{controller}",
				new { controller = "HTTP", action = "Error" }
			);

		}
		
		public static void RegisterGlobalFilters (GlobalFilterCollection filters)
		{
			filters.Add (new HandleErrorAttribute ());
		}*/

		public void Application_Start ()
		{
			Phase = Phases.Init;
			var world = @base.model.World.Instance;
			var controller = @base.control.Controller.Instance;

			var api = server.control.APIController.Instance;

			var regionManagerLastC = new control.RegionManagerController (null, world.RegionStates.Last);
			var regionManagerCurrC = new control.RegionManagerController (regionManagerLastC, world.RegionStates.Curr);
			var regionManagerNextC = new control.RegionManagerController (regionManagerCurrC, world.RegionStates.Next);

			controller.RegionStatesController = new @base.control.RegionStatesController (regionManagerLastC,
																						  regionManagerCurrC,
																						  regionManagerNextC);
			controller.DefinitionManagerController = new server.control.DefinitionManagerController ();
			controller.AccountManagerController = new server.control.AccountManagerController ();


				
			var cleanC = new @server.control.CleaningController ();
			//ThreadPool.QueueUserWorkItem (new WaitCallback (cleanC.Run));
            for (int threadNr = 0; threadNr < server.model.ServerConstants.ACTION_THREADS; ++ threadNr)
            {
                Thread t = new Thread (new ParameterizedThreadStart(server.control.APIController.Instance.Worker));
                t.Start (threadNr);
                //new Thread (server.control.APIController.Instance.Worker2).Start ();
                //ThreadPool.QueueUserWorkItem (new WaitCallback (server.control.APIController.Instance.Worker), threadNr);
            }

			Phase = Phases.Running;

            /*
			AreaRegistration.RegisterAllAreas ();
			RegisterGlobalFilters (GlobalFilters.Filters);
			RegisterRoutes (RouteTable.Routes);
            */
		}
	}
}
