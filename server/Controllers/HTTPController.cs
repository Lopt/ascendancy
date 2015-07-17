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

using Core.Models;

namespace server.Controllers
{
	/// <summary>
	/// Handles all HTTP Accesses from the client.
	/// </summary>
    public class HTTPController : Controller
    {
        public string Login()
        {
            var json = Request["json"];
            var loginRequest = JsonConvert.DeserializeObject<Core.Connections.LoginRequest>(json);

            var response = new Core.Connections.LoginResponse();
            var api = server.control.APIController.Instance;

            var account = api.Login(loginRequest.Username, loginRequest.Password);
            if (account != null)
            {
                var accountC = (@server.control.AccountController)account.Control;
                response.SessionID = accountC.SessionID;
                response.AccountId = account.ID;
                response.Status = Core.Connections.LoginResponse.ReponseStatus.OK;
            }
            else
            {
                response.Status = Core.Connections.LoginResponse.ReponseStatus.ERROR;
            }

            return JsonConvert.SerializeObject(response);

        }

        public string LoadRegions()
        {
            var json = Request["json"];
            var loadRegionRequest = JsonConvert.DeserializeObject<Core.Connections.LoadRegionsRequest>(json);

            var response = new Core.Connections.Response();
            var api = server.control.APIController.Instance;
            var controller = Core.Controllers.Controller.Instance;

            var accountManagerC = (control.AccountManagerController)Core.Models.World.Instance.AccountManager;
            var account = accountManagerC.GetAccountBySession(loadRegionRequest.SessionID);


            if (account != null &&
                loadRegionRequest.RegionPositions.Count() <= Core.Models.Constants.MAX_ENTRIES_PER_CONNECTION)
            {
                var regionActions = api.LoadRegions(account, loadRegionRequest.RegionPositions);
                response.Entities = regionActions.EntityDict;
                response.Actions = regionActions.ActionDict;
                response.Status = Core.Connections.Response.ReponseStatus.OK;
            }

            return JsonConvert.SerializeObject(response);
        }

        public string DoActions()
        {
            var json = Request["json"];
            var doActionRequest = JsonConvert.DeserializeObject<Core.Connections.DoActionsRequest>(json);

            var response = new Core.Connections.Response();
            var api = server.control.APIController.Instance;
            var controller = Core.Controllers.Controller.Instance;

            var accountManagerC = (control.AccountManagerController)Core.Models.World.Instance.AccountManager;
            var account = accountManagerC.GetAccountBySession(doActionRequest.SessionID);

            if (account != null &&
                doActionRequest.Actions.Count() <= Core.Models.Constants.MAX_ENTRIES_PER_CONNECTION)
            {
                api.DoAction(account, doActionRequest.Actions);
                response.Status = Core.Connections.Response.ReponseStatus.OK;
            }

            return JsonConvert.SerializeObject(response);
        }


        public string Error(string json)
        {
            return "404 - Nothing to see here.";
        }

    }
}

