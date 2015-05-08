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

		private static readonly NetworkController m_instance = new NetworkController ();

		private NetworkController ()
		{
			ExceptionMessage = "";
			JsonTerrainsString = "";
			m_client = new HttpClient (new NativeMessageHandler ());
		}

		public static NetworkController GetInstance{ get { return m_instance; } }

		#endregion

		#region Networking

		public string ExceptionMessage { get; private set; }

		public string JsonTerrainsString{ get; private set; }

		public string JsonTerrainTypeString{ get; private set; }

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

		#endregion

		#region private Fields

		private HttpClient m_client;

		#endregion
	}
}

