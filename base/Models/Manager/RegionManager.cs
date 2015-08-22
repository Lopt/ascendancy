namespace Core.Models
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Contains all loaded Regions.
    /// </summary>
    public class RegionManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.RegionManager"/> class.
        /// </summary>
        public RegionManager()
        {
            Regions = new ConcurrentDictionary<RegionPosition, Region>();
        }

        /// <summary>
        /// Gets the region depending on the region position.
        /// </summary>
        /// <returns>The region.</returns>
        /// <param name="regionPosition">Region position.</param>
        public Region GetRegion(RegionPosition regionPosition)
        {
            if (Regions.ContainsKey(regionPosition))
            {
                return Regions[regionPosition];
            }
            var region = new Region(regionPosition);

            return region;
        }

        /// <summary>
        /// Adds an region.
        /// </summary>
        /// <param name="region">Region which should be added.</param>
        public void AddRegion(Region region)
        {
            Regions[region.RegionPosition] = region;
        }

        /// <summary>
        /// The regions.
        /// </summary>
        public ConcurrentDictionary<RegionPosition, Region> Regions;
    }
}