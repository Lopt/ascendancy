using System;
using System.Collections.Concurrent;

namespace @base.model
{
	public class RegionManager
	{
        public RegionManager ()
        {
            m_regions = new ConcurrentDictionary<RegionPosition, Region> ();
        }

        public Region GetRegion (RegionPosition regionPosition)
        {
            if (m_regions.ContainsKey(regionPosition))
            {
                return m_regions[regionPosition];
            }
            var region =  new Region(regionPosition);

            return region;
        }
            
        public void AddRegion (Region region)
        {
            m_regions[region.RegionPosition] = region;
        }

        private ConcurrentDictionary<RegionPosition, Region> m_regions;
	}
}

