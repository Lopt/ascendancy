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
        public static readonly string REGION_FILE = Path.Combine("data", "world", "$MajorRegionX", "$MajorRegionY", "germany-$MinorRegionX-$MinorRegionY.json");
	}
}

