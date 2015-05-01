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
			var response = new @base.connection.Response (@base.connection.Response.ReponseStatus.INTERNAL_ERROR,
				               new ConcurrentDictionary<RegionPosition, Region.DatedActions> (),
				               new ConcurrentDictionary<RegionPosition, Region.DatedEntities> ());

			var controller = @base.control.Controller.Instance;
			var accountManagerC = (control.AccountManagerController)controller.AccountManagerController;
			if (accountManagerC.Login (loginRequest.Username, loginRequest.Password))
			{
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

