using System;

namespace @base.model
{
    public class Constants
    {
        // size of a cell at the equator (in meters)
        public const double CELL_SIZE = 4;

        // earth size at equator (in meters)
        public const double EARTH_CIRCUMFERENCE = 40075036.0;

        // amount of cells per region
        public const int REGION_SIZE_X = 32;
        public const int REGION_SIZE_Y = 32;

        // amount of regions per folder
        public const int MAJOR_REGION_SIZE_X = 16;
        public const int MAJOR_REGION_SIZE_Y = 16;

        // start and end of playable world
        public const int START_X = 41504;
        public const int END_X = 41519;

        public const int START_Y = 26184;
        public const int END_Y = 26191;

        // if there are more than entries in a list/array, the API will dismiss the request
        // for safety reasons
        public const int MAX_ENTRIES_PER_CONNECTION = 25;
    }
}

	