using System;

namespace @base.model
{
	public class Constants
	{
        // size of a cell at the equator (in meters * 4)
        public const double cellSize = 4 * 4; 

        // earth size at equator (in meters)
        public const double earthCircumference = 40075036.0;

        // amount of cells per region
		public const int regionSizeX = 32;
        public const int regionSizeY = 32;

        // amount of regions per folder
        public const int majorRegionSizeX = 16;
        public const int majorRegionSizeY = 16;

        // start and end of playable world
        public const int startX = 41504;
        public const int endX = 41519;

        public const int startY = 26184;
        public const int endY = 26191;

        // Warning: Paths may be with an backshlash (\) instead of an slash (/) on windows
        // path to the terrain file 
        public const string terrainFile = @"data/terrain.json";

        // path to the terrain file 
        public const string regionFile = @"data/world/$MajorRegionX/$MajorRegionY/germany-$MinorRegionX-$MinorRegionY.json";
    }
}

	