using System;

namespace @base.model
{
	public sealed class World
	{
        public static World Instance { get { return lazy.Value; } } 


        private static readonly Lazy<World> lazy =
            new Lazy<World>(() => new World());

        private World()
        {
            m_regionManagers = new RegionManager ();
        }
            
		private RegionManager m_regionManagers;
	}
}

