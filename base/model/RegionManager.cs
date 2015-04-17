using System;
using System.Collections.ObjectModel;

namespace @base.model
{
	public class RegionManager
	{
		public RegionManager ()
		{
		}

        public extern Region GetRegion(RegionPosition regionPosition);

		KeyedCollection<RegionPosition, Region> _regions;
	}
}

