using System;

namespace client.Common.controller
{
	public sealed class TerrainController
	{
		#region Singelton

		private static readonly TerrainController _instance = new TerrainController ();

		private TerrainController ()
		{

		}

		public static TerrainController GetInstance{ get { return _instance; } }

		#endregion
	}
}

