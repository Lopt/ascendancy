using System;

namespace @base.model
{
    public class Constants
    {
        // size of a cell at the equator (in meters)
        public const double CELLSIZE = 4;

        // earth size at equator (in meters)
        public const double EARTHCIRCUMFERENCE = 40075036.0;

        // amount of cells per region
        public const int REGIONSIZE_X = 32;
        public const int REGIONSIZE_Y = 32;

        // amount of regions per folder
        public const int MAJORREGIONSIZE_X = 16;
        public const int MAJORREGIONSIZE_Y = 16;

        // start and end of playable world
        public const int START_X = 41504;
        public const int END_X = 41519;

        public const int START_Y = 26184;
        public const int END_Y = 26191;

        // Warning: Paths may be with an backshlash (\) instead of an slash (/) on windows
        // path to the terrain file
        public const string TERRAINFILE = @"data/terrain.json";

        // path to the terrain file
        public const string REGIONFILE = @"data/world/$MajorRegionX/$MajorRegionY/germany-$MinorRegionX-$MinorRegionY.json";
    }
}

	