
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace server
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

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
				"Default",
				"{controller}",
				new { controller = "HTTP", action = "Error" }
			);

		}
		
		public static void RegisterGlobalFilters (GlobalFilterCollection filters)
		{
			filters.Add (new HandleErrorAttribute ());
		}

		protected void Application_Start ()
		{
			var world = @base.model.World.Instance;
			var controller = @base.control.Controller.Instance;

			var api = server.control.APIController.Instance;

			controller.RegionManagerController = new server.control.RegionManagerController ();
			controller.TerrainManagerController = new server.control.TerrainManagerController ();
			controller.AccountManagerController = new server.control.AccountManagerController ();

			var testAccount = new @base.model.Account (Guid.NewGuid(), "Test");
			var testAccountC = new server.control.AccountController (testAccount, "Test");

			controller.AccountManagerController.Registrate (testAccount);

			AreaRegistration.RegisterAllAreas ();
			RegisterGlobalFilters (GlobalFilters.Filters);
			RegisterRoutes (RouteTable.Routes);
		}
	}
}
