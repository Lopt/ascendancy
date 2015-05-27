using System;
using System.IO;

namespace server.model
{
	public class ServerConstants
	{
		// Warning: Paths may be with an backshlash (\) instead of an slash (/) on windows
		// path to the terrain file
        public static readonly string TERRAIN_FILE = Path.Combine("data", "terrain.json");

		// path to the terrain file
		public static readonly string REGION_FILE = Path.Combine("data", Path.Combine("ascendancy-world", "world", "$MajorRegionX", "$MajorRegionY", "germany-$MinorRegionX-$MinorRegionY.json"));

		// time when all game datas should cleaned in milliseconds
		public static readonly int CLEANING_INTERVALL = 30 * 60 * 1000; // minutes * seconds * milliseconds

		// threads which execute actions
		public static readonly int ACTION_THREADS = 8;

		// 
		public static readonly int ACTION_THREAD_SLEEP = 1;

	}
}

