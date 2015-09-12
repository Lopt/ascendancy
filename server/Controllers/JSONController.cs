namespace Server.Controllers
{ 
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Core.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Handles all HTTP Accesses from the client.
    /// </summary>
    public class JSONController
    {
        /// <summary>
        /// Login method.
        /// </summary>
        /// <returns>Login Response serialized JSON</returns>
        /// <param name="json">Login Request as serialized JSON.</param>
        public static string Login(string json)
        {
            var loginRequest = JsonConvert.DeserializeObject<Core.Connections.LoginRequest>(json);

            var response = new Core.Connections.LoginResponse();
            var api = Controllers.APIController.Instance;

            var account = api.Login(loginRequest.Username, loginRequest.Password);
            if (account != null)
            {
                var accountC = (Server.Controllers.AccountController)account.Control;
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

        /// <summary>
        /// Loads the regions.
        /// </summary>
        /// <returns>The regions as LoadRegions Response serialized JSON.</returns>
        /// <param name="json">LoadRegions Request as serialized JSON.</param>
        public static string LoadRegions(string json)
        {
            var loadRegionRequest = JsonConvert.DeserializeObject<Core.Connections.LoadRegionsRequest>(json);

            var response = new Core.Connections.Response();
            var api = Controllers.APIController.Instance;
            var controller = Core.Controllers.Controller.Instance;

            var accountManagerC = (Server.Controllers.AccountManagerController)Core.Models.World.Instance.AccountManager;
            var account = accountManagerC.GetAccountBySession(loadRegionRequest.SessionID);

            if (account != null &&
                loadRegionRequest.RegionPositions.Length <= Core.Models.Constants.MAX_ENTRIES_PER_CONNECTION)
            {
                var regionActions = api.LoadRegions(account, loadRegionRequest.RegionPositions);
                response.Entities = regionActions.EntityDict;
                response.Actions = regionActions.ActionDict;
                response.Status = Core.Connections.Response.ReponseStatus.OK;
            }

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Executes the actions.
        /// </summary>
        /// <returns>DoAction Response as serialized JSON.</returns>
        /// <param name="json">DoAction Request as serialized JSON.</param>
        public static string DoActions(string json)
        {
            var doActionRequest = JsonConvert.DeserializeObject<Core.Connections.DoActionsRequest>(json);

            var response = new Core.Connections.Response();
            var api = Controllers.APIController.Instance;
            var controller = Core.Controllers.Controller.Instance;

            var accountManagerC = (Server.Controllers.AccountManagerController)Core.Models.World.Instance.AccountManager;
            var account = accountManagerC.GetAccountBySession(doActionRequest.SessionID);

            if (account != null &&
                doActionRequest.Actions.Length <= Core.Models.Constants.MAX_ENTRIES_PER_CONNECTION)
            {
                api.DoAction(account, doActionRequest.Actions);
                response.Status = Core.Connections.Response.ReponseStatus.OK;
            }

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Default accessor for testing purpose
        /// </summary>
        /// <returns>A string that the connection is working.</returns>
        /// <param name="json">Doesn't matter.</param>
        public static string Default(string json)
        {
            return "Yep. The Connection is working.";
        }
    }
}
