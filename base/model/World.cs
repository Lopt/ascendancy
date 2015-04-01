using System;

namespace @base.model
{
	public class World : Singleton
	{
		public World ()
		{
			m_regionManagers = new RegionManager ();
		}


		private RegionManager m_regionManagers;
	}
}

