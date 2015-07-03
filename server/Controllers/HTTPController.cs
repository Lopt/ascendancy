using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using System.Threading;

using @base.model;
using server.Models;

namespace server.Controllers
{
	public class HTTPController : Controller
	{
		public string Login()
		{
			var json = Request ["json"];
			var loginRequest = JsonConvert.DeserializeObject<@base.connection.LoginRequest>(json);

			var response = new @base.connection.LoginResponse ();
			var api = server.control.APIController.Instance;

			var account = api.Login (loginRequest.Username, loginRequest.Password);
			if (account != null)
			{
				var accountC = (@server.control.AccountController)account.Control;
				response.SessionID = accountC.SessionID;
                response.AccountId = account.ID;
                response.Status = @base.connection.LoginResponse.ReponseStatus.OK;
			}
			else
			{
				response.Status = @base.connection.LoginResponse.ReponseStatus.ERROR;
			}

			return JsonConvert.SerializeObject (response);

        }

		public string LoadRegions()
		{
			var json = Request ["json"];
			var loadRegionRequest = JsonConvert.DeserializeObject<@base.connection.LoadRegionsRequest>(json);

			var response = new @base.connection.Response ();
			var api = server.control.APIController.Instance;
			var controller = @base.control.Controller.Instance;

			var accountManagerC = (control.AccountManagerController)controller.AccountManagerController;
			var account = accountManagerC.GetAccountBySession (loadRegionRequest.SessionID);


			if (account != null &&
				loadRegionRequest.RegionPositions.Count() <= @base.model.Constants.MAX_ENTRIES_PER_CONNECTION)
			{
				var regionActions = api.LoadRegions (account, loadRegionRequest.RegionPositions);
				response.Entities = regionActions.EntityDict;
				response.Actions = regionActions.ActionDict;
				response.Status = @base.connection.Response.ReponseStatus.OK;
			}

			return JsonConvert.SerializeObject (response);
		}

		public string DoActions()
		{
			var json = Request ["json"];
			var doActionRequest = JsonConvert.DeserializeObject<@base.connection.DoActionsRequest>(json);

			var response = new @base.connection.Response ();
			var api = server.control.APIController.Instance;
			var controller = @base.control.Controller.Instance;

			var accountManagerC = (control.AccountManagerController)controller.AccountManagerController;
			var account = accountManagerC.GetAccountBySession (doActionRequest.SessionID);

			if (account != null &&
				doActionRequest.Actions.Count() <= @base.model.Constants.MAX_ENTRIES_PER_CONNECTION)
			{
				api.DoAction (account, doActionRequest.Actions);
				response.Status = @base.connection.Response.ReponseStatus.OK;
			}

			return JsonConvert.SerializeObject (response);
		}
           

		public string Error(string json)
		{
			return "404 - Nothing to see here.";
		}

	}
}

