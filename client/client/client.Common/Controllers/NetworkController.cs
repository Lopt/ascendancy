using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ModernHttpClient;
using XLabs.Platform.Services.Geolocation;

using Core.Models.Definitions;
using Core.Models;
using client.Common.Helper;
using System.Collections.Generic;



namespace client.Common.Controllers
{
    public sealed class NetworkController
    {
        #region Singleton

        /// <summary>
        /// The lazy singleton.
        /// </summary>
        private static readonly Lazy<NetworkController> m_singleton =
            new Lazy<NetworkController>(() => new NetworkController());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static NetworkController Instance { get { return m_singleton.Value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="client.Common.Controllers.NetworkController"/> class.
        /// </summary>
        private NetworkController()
        {
            ExceptionMessage = "";
            m_client = new HttpClient(new NativeMessageHandler());
            m_sessionID = Guid.Empty;
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
        /// Gets a value indicating whether this instance is logedin.
        /// </summary>
        /// <value><c>true</c> if this instance is logedin; otherwise, <c>false</c>.</value>
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

        private async Task<string> RequestAsync(string url)
        {
            string json = null;
            try
            {
                HttpResponseMessage response = await m_client.GetAsync(new Uri(url));
                if (response != null)
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.Message;
                throw ex;
            }
            return json;
        }

        /// <summary>
        /// Loads the terrains async and write it in property JsonTerrainString.
        /// </summary>
        /// <param name="jsonRegionServerPath">Json region server path.</param>
        public async Task<TerrainDefinition[,]> LoadTerrainsAsync(RegionPosition regionPosition)
        {
            var path = Core.Helper.NetworkHelper.ReplacePath(ClientConstants.REGION_SERVER_PATH, regionPosition);
            var json = await RequestAsync(path);
            return JsonConvert.DeserializeObject<TerrainDefinition[,]>(json);
        }

        /// <summary>
        /// Loads the terrain types async and returns an array of TerrainDefinition
        /// </summary>
        public async Task<Core.Models.Definitions.TerrainDefinition[]> LoadTerrainTypesAsync()
        {
            string path = ClientConstants.TERRAIN_TYPES_SERVER_PATH;
            var json = await RequestAsync(path);
            return JsonConvert.DeserializeObject<Core.Models.Definitions.TerrainDefinition[]>(json);
        }

        /// <summary>
        /// Loads the terrain types async and returns an array of TerrainDefinition
        /// </summary>
        public async Task<Core.Models.Definitions.UnitDefinition[]> LoadUnitTypesAsync()
        {
            string path = ClientConstants.UNIT_TYPES_SERVER_PATH;
            var json = await RequestAsync(path);
            return JsonConvert.DeserializeObject<Core.Models.Definitions.UnitDefinition[]>(json);
        }


        /// <summary>
        /// Login async to the server and save the sessionID.
        /// </summary>
        /// <returns>The AccoountId.</returns>
        /// <param name="currentGamePosition">Current game position.</param>
        /// <param name="user">User.</param>
        /// <param name="password">Password.</param>
        public async Task<Account> LoginAsync(Core.Models.Position currentGamePosition, string user, string password)
        {
            Account account = null;
            var request = new Core.Connections.LoginRequest(currentGamePosition, user, password);
            var json = JsonConvert.SerializeObject(request);

            var path = ClientConstants.LOGIN_PATH;
            path = path.Replace(ClientConstants.LOGIC_SERVER_JSON, json);

            var newPath = ClientConstants.LOGIC_SERVER + path;
            var jsonFromServer = await RequestAsync(newPath);

            var loginResponse = JsonConvert.DeserializeObject<Core.Connections.LoginResponse>(jsonFromServer);
            if (loginResponse.Status == Core.Connections.LoginResponse.ReponseStatus.OK)
            {
                m_sessionID = loginResponse.SessionID;
                account = new Account(loginResponse.AccountId, user);
            }
            else
            {
                ExceptionMessage = "Login failure";
            }

            return account;
        }

        /// <summary>
        /// Loads the entities async.
        /// </summary>
        /// <returns>The entities async.</returns>
        /// <param name="currentGamePosition">Current game position.</param>
        /// <param name="regionPositions">Region positions.</param>
        public async Task<Core.Connections.Response> LoadEntitiesAsync(Core.Models.Position currentGamePosition, RegionPosition[] regionPositions)
        {            
            var request = new Core.Connections.LoadRegionsRequest(m_sessionID, currentGamePosition, regionPositions);
            var json = JsonConvert.SerializeObject(request);

            var path = ClientConstants.LOAD_REGIONS_PATH;
            path = path.Replace(ClientConstants.LOGIC_SERVER_JSON, json);
            path = ClientConstants.LOGIC_SERVER + path;

            var jsonFromServer = await RequestAsync(path);
            var entitiesResponse = JsonConvert.DeserializeObject<Core.Connections.Response>(jsonFromServer);

            if (entitiesResponse.Status == Core.Connections.Response.ReponseStatus.OK)
            {
                return entitiesResponse;
            }
            return new Core.Connections.Response();
        }

        public async Task<bool> DoActionsAsync(Core.Models.Position currentGamePosition, Core.Models.Action[] actions)
        {
            var request = new Core.Connections.DoActionsRequest(m_sessionID, currentGamePosition, actions);
            var json = JsonConvert.SerializeObject(request);

            var path = ClientConstants.DO_ACTIONS_PATH;
            path = path.Replace(ClientConstants.LOGIC_SERVER_JSON, json);
            path = ClientConstants.LOGIC_SERVER + path;  
            var jsonFromServer = await RequestAsync(path);


            var entitiesResponse = JsonConvert.DeserializeObject<Core.Connections.Response>(jsonFromServer);
            if (entitiesResponse.Status == Core.Connections.Response.ReponseStatus.OK)
            {
                return true;
            }
            return false;
        }




        #endregion

        #region private Fields

        private HttpClient m_client;
        private Guid m_sessionID;

        #endregion
    }
}

