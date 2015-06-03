using System;
using System.IO;

namespace server.model
{
	
	public class ServerConstants
	{
		// path to the terrain file
		public static readonly string TERRAIN_FILE = Path.Combine("data", "terrain.json");


		// path to the terrain file
		public static readonly string UNIT_FILE = Path.Combine("data", "unit.json");



		// path to the terrain file
		public static readonly string REGION_FILE = Path.Combine("data", Path.Combine("ascendancy-world", "world", "$MajorRegionX", "$MajorRegionY", "germany-$MinorRegionX-$MinorRegionY.json"));

        // path to the DB
        public static readonly string DB_PATH = Path.Combine(Environment.CurrentDirectory, "DB_Ascendancy");

        // size of the salt for password encryption 
        public static readonly int SALT_SIZE = 32;

        // cicle to compute the hash value
        public static readonly int HASH_CICLE = 32;

		// time when all game datas should cleaned in milliseconds
		public static readonly int CLEANING_INTERVALL = 30 * 60 * 1000; // minutes * seconds * milliseconds

		// threads which execute actions
		public static readonly int ACTION_THREADS = 8;

		// 
		public static readonly int ACTION_THREAD_SLEEP = 1;

	}
}

