namespace Core.Models
{
    using System;

    /// <summary>
    /// An Class which provides access to all stuff, which isn't allowed normally. Can be used for debugging or simplify stuff.
    /// BUT: Cheating may cause other bugs! There is no need to fix them, unless it is essentially, nicer code or the bug could occur without the cheats
    /// </summary>
    public class Cheats
    {
        /// <summary>
        /// an parameter for the application so it won't load regions from the server btw load the regions from the local store
        /// should be false
        /// </summary>
        public const bool DONT_LOAD_REGIONS_MODE = false; // false

        /// <summary>
        /// the port of the debug server
        /// </summary>
        public static readonly int DEBUG_TCP_PORT = 13000;

        /// <summary>
        /// the name of the debug server
        /// </summary>
        public static readonly string DEBUG_TCP_SERVER = "192.168.2.6";

        /// <summary>
        /// activate or deactivates offline mode. Which enables other cheats, but won't send stuff to the server.
        /// </summary>
        public static readonly bool OFFLINE_MODE = false;
    }

    /// <summary>
    /// all constants which should be known by the server and client
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// size of a cell at the equator (in meters)
        /// </summary>
        public const double CELL_SIZE = 4;

        /// <summary>
        /// earth size at equator (in meters)
        /// </summary>
        public const double EARTH_CIRCUMFERENCE = 40075036.0;

        /// <summary>
        /// amount of cells per region x
        /// </summary>
        public const int REGION_SIZE_X = 32;

        /// <summary>
        /// amount of cells per region y
        /// </summary>
        public const int REGION_SIZE_Y = 32;

        /// <summary>
        /// amount of regions per folder x
        /// </summary>
        public const int MAJOR_REGION_SIZE_X = 16;

        /// <summary>
        /// amount of regions per folder y
        /// </summary>
        public const int MAJOR_REGION_SIZE_Y = 16;

        /// <summary>
        /// start x of playable world
        /// </summary>
        public const int START_X = 41504;

        /// <summary>
        /// end x of playable world
        /// </summary>
        public const int END_X = 41519;

        /// <summary>
        /// start y of playable world
        /// </summary>
        public const int START_Y = 26184;

        /// <summary>
        /// end y of playable world
        /// </summary>
        public const int END_Y = 26191;

        /// <summary>
        /// if there are more than entries in a LinkedList/array, the API will dismiss the request
        /// for safety reasons
        /// </summary>
        public const int MAX_ENTRIES_PER_CONNECTION = 25;

        /// <summary>
        /// wait time for region writer lock
        /// </summary>
        public const int REGION_LOCK_WAIT_TIME = 0;

        /// <summary>
        /// The minus point for range units in melee combat.
        /// </summary>
        public const int RANGE_IN_MEELE_MALUS = 10;

        /// <summary>
        /// Get the range from the ownership radius for the headquarter.
        /// </summary>
        public const int HEADQUARTER_TERRITORY_RANGE = 4;

        /// <summary>
        /// Get the range from the ownership radius for the guard tower.
        /// </summary>
        public const int GUARDTOWER_TERRITORY_RANGE = 2;

        /// <summary>
        /// The HQ storage value.
        /// </summary>
        public const int HEADQUARTER_STORAGE_VALUE = 100;

        /// <summary>
        /// The ground population storage.
        /// </summary>
        public const int POPULATION_STORAGE_VALUE = 25;

        /// <summary>
        /// The ground scrap storage
        /// </summary>
        public const int SCRAP_STORAGE_VALUE = 50;

        /// <summary>
        /// The energy value.
        /// </summary>
        public const int ENERGY_MAX_VALUE = 20;

        /// <summary>
        /// The technology maximum value.
        /// </summary>
        public const int TECHNOLOGY_MAX_VALUE = 7;

        /// <summary>
        /// The increment value for technology resource.
        /// </summary>
        public const float TECHNOLOGY_INCREMENT_VALUE = 0.1f;

        /// <summary>
        /// The increment value for scrap resource.
        /// </summary>
        public const int SCRAP_INCREMENT_VALUE = 2;

        /// <summary>
        /// The increment value for plutonium resource.
        /// </summary>
        public const float PLUTONIUM_INCREMENT_VALUE = 0.3f;
    }
}