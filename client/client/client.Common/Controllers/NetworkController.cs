using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ModernHttpClient;
using XLabs.Platform.Services.Geolocation;

using @base.model.definitions;
using @base.model;



namespace client.Common.Controllers
{
	public sealed class NetworkController
	{
		#region Singelton

		private static readonly NetworkController _instance = new NetworkController ();

		private NetworkController ()
		{
			ExceptionMessage = "";
			JsonTerrainsString = "";
			_client = new HttpClient (new NativeMessageHandler ());
		}

		public static NetworkController GetInstance{ get { return _instance; } }

		#endregion

		#region Networking

		public string ExceptionMessage { get; private set; }

		public string JsonTerrainsString{ get; private set; }

		public string JsonTerrainTypeString{ get; private set; }

		public async Task LoadTerrainsAsync (string _JsonRegionServerPath)
		{
			try {
				HttpResponseMessage response = await _client.GetAsync (new Uri (_JsonRegionServerPath));
				if (response != null) {
					response.EnsureSuccessStatusCode ();
					JsonTerrainsString = await response.Content.ReadAsStringAsync ();
				}
			} catch (Exception ex) {
				ExceptionMessage = ex.Message;
				throw ex;
			}
		
		}

		public async Task LoadTerrainTypesAsync (string _JsonTerrainTypeServerPath)
		{
			try {
				HttpResponseMessage response = await _client.GetAsync (new Uri (_JsonTerrainTypeServerPath));
				if (response != null) {
					response.EnsureSuccessStatusCode ();
					JsonTerrainTypeString = await response.Content.ReadAsStringAsync ();
				}
			} catch (Exception ex) {
				ExceptionMessage = ex.Message;
				throw ex;
			}

		}

		#endregion

		#region private Fields

		private HttpClient _client;

		#endregion
	}
}

