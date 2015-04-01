using System;
using System.Collections.ObjectModel;

namespace @base.model
{
	public class RegionManager
	{
		public RegionManager ()
		{
			for (int regionX = 0; regionX < Constants.numberOfRegionsX; ++regionX)
			{
				for (int regionY = 0; regionY < Constants.numberOfRegionsY; ++regionY)
				{
					_regions.Add(Region(RegionPosition(regionX, regionY)));
				}
			}
		}

		extern public Region GetRegion(RegionPosition regionPosition);

		KeyedCollection<RegionPosition, Region> _regions;
	}
}

