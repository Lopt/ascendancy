using System;
using System.Collections.Concurrent;

namespace @base.model
{
    public class RegionManager
	{
        public RegionManager (RegionManager parent = null)
        {
            Regions = new ConcurrentDictionary<RegionPosition, Region> ();
        }

        public Region GetRegion (RegionPosition regionPosition)
        {
            if (Regions.ContainsKey(regionPosition))
            {
                return Regions[regionPosition];
            }
            var region =  new Region(regionPosition);

            return region;
        }
            
        public void AddRegion (Region region)
        {
            Regions[region.RegionPosition] = region;
        }

        public ConcurrentDictionary<RegionPosition, Region> Regions;
	}
}

