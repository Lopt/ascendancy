using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ModernHttpClient;
using XLabs.Platform.Services.Geolocation;

using @base.model.definitions;
using @base.model;
using client.Common.Helper;
using System.Collections.Generic;



namespace client.Common.Controllers
{
    public sealed class NetworkController
    {
        #region Singelton

        private static readonly NetworkController m_instance = new NetworkController ();

        private NetworkController ()
        {
            ExceptionMessage = "";
            JsonTerrainsString = "";
            m_client = new HttpClient (new NativeMessageHandler ());
            m_sessionID = Guid.Empty;
        }

        public static NetworkController GetInstance {
            get {
                return m_instance; 
            }
        }

        #endregion

        #region Networking

        public string ExceptionMessage {
            get;
            private set; 
        }

        public string JsonTerrainsString {
            get; 
            private set;
        }

        public string JsonTerrainTypeString {
            get; 
            private set;
        }


        public bool IsLogedin {
            get {
                if (m_sessionID != Guid.Empty) {
                    return true;
                }
                return false;
            }
        }

        public async Task LoadTerrainsAsync (string jsonRegionServerPath)
        {
            try {
                HttpResponseMessage response = await m_client.GetAsync (new Uri (jsonRegionServerPath));
                if (response != null) {
                    response.EnsureSuccessStatusCode ();
                    JsonTerrainsString = await response.Content.ReadAsStringAsync ();
                }
            } catch (Exception ex) {
                ExceptionMessage = ex.Message;
                throw ex;
            }
		
        }

        public async Task LoadTerrainTypesAsync (string jsonTerrainTypeServerPath)
        {
            try {
                HttpResponseMessage response = await m_client.GetAsync (new Uri (jsonTerrainTypeServerPath));
                if (response != null) {
                    response.EnsureSuccessStatusCode ();
                    JsonTerrainTypeString = await response.Content.ReadAsStringAsync ();
                }
            } catch (Exception ex) {
                ExceptionMessage = ex.Message;
                throw ex;
            }

        }

        public async Task<int> LoginAsync (@base.model.Position currentGamePosition, string user, string password)
        {
            try {
                int id = -1;
                var request = new @base.connection.LoginRequest (currentGamePosition, user, password);
                var json = JsonConvert.SerializeObject (request);

                var path = ClientConstants.LOGIN_PATH;
                path = path.Replace (ClientConstants.LOGIC_SERVER_JSON, json);

                HttpResponseMessage response = await m_client.GetAsync (new Uri (ClientConstants.LOGIC_SERVER + path));
                if (response != null) {
                    response.EnsureSuccessStatusCode ();
                    var jsonFromServer = await response.Content.ReadAsStringAsync ();

                    var loginResponse = JsonConvert.DeserializeObject<@base.connection.LoginResponse> (jsonFromServer);
                    if (loginResponse.Status == @base.connection.LoginResponse.ReponseStatus.OK) {
                        m_sessionID = loginResponse.SessionID;
                        id = loginResponse.AccountId;
                    } else {
                        ExceptionMessage = "Login failure";
                    }

                }
                return id;
            } catch (Exception ex) {
                ExceptionMessage = ex.Message;
                throw ex;
            }
        }

        public async Task<@base.connection.Response> LoadEntitiesAsync (@base.model.Position currentGamePosition, RegionPosition[] regionPositions)
        {
            try {
                var request = new @base.connection.LoadRegionsRequest (m_sessionID, currentGamePosition, regionPositions);
                var json = JsonConvert.SerializeObject (request);

                var path = ClientConstants.LOAD_REGIONS_PATH;
                path = path.Replace (ClientConstants.LOGIC_SERVER_JSON, json);

                HttpResponseMessage response = await m_client.GetAsync (new Uri (ClientConstants.LOGIC_SERVER + path));
                if (response != null) {
                    response.EnsureSuccessStatusCode ();
                    var jsonFromServer = await response.Content.ReadAsStringAsync ();

                    var entitiesResponse = JsonConvert.DeserializeObject<@base.connection.Response> (jsonFromServer);

                    if (entitiesResponse.Status == @base.connection.Response.ReponseStatus.OK) {
                        return entitiesResponse;
                    }

                }
                return new @base.connection.Response ();

            } catch (Exception ex) {
                ExceptionMessage = ex.Message;
                throw ex;
            }
        }

        public async Task<bool> DoActionsAsync (@base.model.Position currentGamePosition, @base.model.Action[] actions)
        {
            try {
                var request = new @base.connection.DoActionsRequest (m_sessionID, currentGamePosition, actions);
                var json = JsonConvert.SerializeObject (request);

                var path = ClientConstants.DO_ACTIONS_PATH;
                path = path.Replace (ClientConstants.LOGIC_SERVER_JSON, json);

                HttpResponseMessage response = await m_client.GetAsync (new Uri (ClientConstants.LOGIC_SERVER + path));
                if (response != null) {
                    response.EnsureSuccessStatusCode ();
                    var jsonFromServer = await response.Content.ReadAsStringAsync ();

                    var entitiesResponse = JsonConvert.DeserializeObject<@base.connection.Response> (jsonFromServer);
                    if (entitiesResponse.Status == @base.connection.Response.ReponseStatus.OK) {
                        return true;
                    }

                }
                return false;

            } catch (Exception ex) {
                ExceptionMessage = ex.Message;
                throw ex;
            }
        }




        #endregion

        #region private Fields

        private HttpClient m_client;
        private Guid m_sessionID;

        #endregion
    }
}

