using System;

namespace server.model
{
	public class ServerConstants
	{
		// Warning: Paths may be with an backshlash (\) instead of an slash (/) on windows
		// path to the terrain file
		public const string TERRAIN_FILE = @"data/terrain.json";

		// path to the terrain file
		public const string REGION_FILE = @"data/world/$MajorRegionX/$MajorRegionY/germany-$MinorRegionX-$MinorRegionY.json";
	}
}

