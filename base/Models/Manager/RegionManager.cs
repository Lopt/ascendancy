using System;
using System.Collections.Concurrent;

namespace Core.Models
{
    public class RegionManager
    {
        public RegionManager()
        {
            Regions = new ConcurrentDictionary<RegionPosition, Region>();
        }

        public Region GetRegion(RegionPosition regionPosition)
        {
            if (Regions.ContainsKey(regionPosition))
            {
                return Regions[regionPosition];
            }
            var region = new Region(regionPosition);

            return region;
        }

        public void AddRegion(Region region)
        {
            Regions[region.RegionPosition] = region;
        }

        public ConcurrentDictionary<RegionPosition, Region> Regions;
    }
}

