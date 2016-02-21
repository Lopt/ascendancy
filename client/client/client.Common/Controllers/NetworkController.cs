namespace Client.Common.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using Client.Common.Helper;
    using CocosSharp;
    using Core.Models;
    using Core.Models.Definitions;
    using ModernHttpClient;
    using Newtonsoft.Json;

    /// <summary>
    /// the Network controller is a singleton to control the network up and download to the server.
    /// </summary>
    public sealed class NetworkController
    {
        #region Singleton

        /// <summary>
        /// The lazy singleton.
        /// </summary>
        private static readonly Lazy<NetworkController> Singleton =
            new Lazy<NetworkController>(() => new NetworkController());

        /// <summary>
        /// Prevents a default instance of the <see cref="NetworkController" /> class from being created.
        /// </summary>
        private NetworkController()
        {
            ExceptionMessage = string.Empty;
            m_client = new HttpClient(new NativeMessageHandler());
            m_sessionID = Guid.Empty;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static NetworkController Instance
        {
            get
            {
                return Singleton.Value;
            }
        }

        #endregion

        #region Networking

        /// <summary>
        /// Gets the exception message.
        /// </summary>
        /// <value>The exception message.</value>
        public string ExceptionMessage
        {
            get;
            private set; 
        }

        /// <summary>
        /// Gets a value indicating whether this instance is logged in .
        /// </summary>
        /// <value><c>true</c> if this instance is logged in; otherwise, <c>false</c>.</value>
        public bool IsLoggedIn
        {
            get
            {
                if (m_sessionID != Guid.Empty)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Loads the terrains async and returns it.
        /// </summary>
        /// <returns>The terrains async.</returns>
        /// <param name="regionPosition">Region position.</param>
        public async Task<TerrainDefinition[,]> LoadTerrainsAsync(RegionPosition regionPosition)
        {            
            var path = Core.Helper.LoadHelper.ReplacePath(Common.Constants.ClientConstants.REGION_SERVER_PATH, regionPosition);
            var json = await RequestAsync(path);
            return Core.Helper.LoadHelper.JsonToTerrain(json);
        }

        /// <summary>
        /// Loads the terrain types async and returns an array of TerrainDefinition
        /// </summary>
        /// <returns>The terrain types async.</returns>
        public async Task<Core.Models.Definitions.TerrainDefinition[]> LoadTerrainTypesAsync()
        {
            string path = Common.Constants.ClientConstants.TERRAIN_TYPES_SERVER_PATH;
            var json = await RequestAsync(path);
            return JsonConvert.DeserializeObject<Core.Models.Definitions.TerrainDefinition[]>(json);
        }

        /// <summary>
        /// Loads the terrain types async and returns an array of TerrainDefinition
        /// </summary>
        /// <returns>The unit types async.</returns>
        public async Task<Core.Models.Definitions.UnitDefinition[]> LoadUnitTypesAsync()
        {
            string path = Common.Constants.ClientConstants.UNIT_TYPES_SERVER_PATH;
            var json = await RequestAsync(path);
            return JsonConvert.DeserializeObject<Core.Models.Definitions.UnitDefinition[]>(json);
        }

        /// <summary>
        /// Login async to the server and save the sessionID.
        /// </summary>
        /// <returns>The Account.</returns>
        /// <param name="currentGamePosition">Current game position.</param>
        /// <param name="user">User which wanted to log in.</param>
        /// <param name="password">Password of the user.</param>
        public async Task<Account> LoginAsync(Core.Models.Position currentGamePosition, string user, string password)
        {
            var request = new Core.Connections.LoginRequest(currentGamePosition, user, password);
            var json = JsonConvert.SerializeObject(request);

            var jsonFromServer = await TcpConnection.Connector.SendAsync(Core.Connection.MethodType.Login, json);

            var loginResponse = JsonConvert.DeserializeObject<Core.Connections.LoginResponse>(jsonFromServer);
            if (loginResponse.Status == Core.Connections.LoginResponse.ReponseStatus.OK)
            {
                m_sessionID = loginResponse.SessionID;
                GameAppDelegate.ServerTime = loginResponse.ServerTime; // TODO: this shouldn't be set here, change it
                return new Account(loginResponse.AccountId, user);
            }

            ExceptionMessage = "Login failure";

            return null;
        }

        /// <summary>
        /// Loads the entities and actions async from the server.
        /// </summary>
        /// <returns>The entities response from the server.</returns>
        /// <param name="currentGamePosition">Current game position.</param>
        /// <param name="regionPositions">Region positions.</param>
        public async Task<Core.Connections.Response> LoadEntitiesAsync(Core.Models.Position currentGamePosition, RegionPosition[] regionPositions)
        {            
            try
            {
                var request = new Core.Connections.LoadRegionsRequest(m_sessionID, currentGamePosition, regionPositions);
                var json = JsonConvert.SerializeObject(request);

                var jsonFromServer = await TcpConnection.Connector.SendAsync(Core.Connection.MethodType.LoadEntities, json);
                var entitiesResponse = JsonConvert.DeserializeObject<Core.Connections.Response>(jsonFromServer);

                if (entitiesResponse.Status == Core.Connections.Response.ReponseStatus.OK)
                {
                    return entitiesResponse;
                }
            }
            catch (Exception error)
            {
                Logging.Error(error.StackTrace);
                Logging.Error(error.Message);
            }

            return new Core.Connections.Response();
        }

        /// <summary>
        /// Sends the actions to the server.
        /// </summary>
        /// <returns>True if the response is ok, otherwise false.</returns>
        /// <param name="currentGamePosition">Current game position.</param>
        /// <param name="actions">Actions which should be executed.</param>
        public async Task<bool> DoActionsAsync(Core.Models.Position currentGamePosition, Core.Models.Action[] actions)
        {
            var request = new Core.Connections.DoActionsRequest(m_sessionID, currentGamePosition, actions);
            var json = JsonConvert.SerializeObject(request);

            var jsonFromServer = await TcpConnection.Connector.SendAsync(Core.Connection.MethodType.DoActions, json);

            var entitiesResponse = JsonConvert.DeserializeObject<Core.Connections.Response>(jsonFromServer);
            if (entitiesResponse.Status == Core.Connections.Response.ReponseStatus.OK)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to call the given url. Throws an exception when the website returns an error message (e.g. 404)
        /// </summary>
        /// <returns>JSON string from server</returns>
        /// <param name="url">Which URL should be called</param>
        private async Task<string> RequestAsync(string url)
        {
            // TODO: doesn't work in IOS 9
            try
            {
                Helper.Logging.Info("URL load: " + url);
                var response = await m_client.GetAsync(new Uri(url));

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception exception)
            {
                Helper.Logging.Error("URL load failed: " + exception.Message);
                throw exception;
            }
        }

        #endregion

        #region private Fields

        /// <summary>
        /// The client.
        /// </summary>
        private HttpClient m_client;

        /// <summary>
        /// The sessionID.
        /// </summary>
        private Guid m_sessionID;

        #endregion
    }
}