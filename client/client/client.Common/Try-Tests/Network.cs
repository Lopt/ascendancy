using System;
using System.Net.Http;
using ModernHttpClient;
using System.Linq.Expressions;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace client.Common
{
	public class Network
	{
		HttpClient client;

		public string antwort{ get; private set; }

		public Network ()
		{
			antwort = "";
			client = new HttpClient (new NativeMessageHandler ());
		}

		public async Task Test ()
		{
           
			try {
				HttpResponseMessage response = await client.GetAsync (new Uri ("http://derfalke.no-ip.biz/world/2594/1636/germany-41505-26190.json"));
				if (response != null) {
					response.EnsureSuccessStatusCode ();
					var erg = await response.Content.ReadAsStringAsync ();
					antwort = JsonConvert.DeserializeObject<int[,]> (erg) [0, 1].ToString ();
				}
			} catch (Exception ex) {
				antwort = ex.Message;
				throw ex;
			}

       
		}


	}

   
}

