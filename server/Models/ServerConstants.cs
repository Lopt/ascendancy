namespace Server.Models
{
    using System;
    using System.IO;

    /// <summary>
    /// Constants for the server
    /// </summary>
    public class ServerConstants
    {
        /// <summary>
        /// path to the terrain definition file
        /// </summary>
        public static readonly string TERRAIN_FILE = Path.Combine("data", "terrain.json");

        /// <summary>
        /// path to the unit definition file
        /// </summary>
        public static readonly string UNIT_FILE = Path.Combine("data", "unit.json");

        /// <summary>
        /// path to the region terrain file
        /// </summary>
        public static readonly string REGION_FILE = Path.Combine("data", Path.Combine("ascendancy-world", "world", "$MajorRegionX", "$MajorRegionY", "germany-$MinorRegionX-$MinorRegionY.json"));

        /// <summary>
        /// path to the DB
        /// </summary>
        public static readonly string DB_PATH = Path.Combine(Environment.CurrentDirectory, "DB_Ascendancy");

        /// <summary>
        /// size of the salt for password encryption
        /// </summary>
        public static readonly int SALT_SIZE = 32;

        /// <summary>
        /// cycles to compute the hash value
        /// </summary>
        public static readonly int HASH_CYCLES = 32;

        /// <summary>
        /// amount threads which execute actions
        /// </summary>
        public static readonly int ACTION_THREADS = 2;

        /// <summary>
        /// sleeping time of each thread when there is nothing to do
        /// </summary>
        public static readonly int ACTION_THREAD_SLEEP = 1;
    }
}