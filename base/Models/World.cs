using System;
using System.Collections.Concurrent;

namespace @base.model
{
	public class World
	{
        // TODO: find better singleton implementation
        // http://csharpindepth.com/articles/general/singleton.aspx
        // NOT lazy-singletons: throws useless exceptions when initialisation failed
        private static World instance=null;

        public static World Instance
        {
            get
            {
                if (instance==null)
                {
                    instance = new World();
                }
                return instance;
            }
        }

        private World()
        {
            m_regionManager = new RegionManager ();
            m_terrainManager = new TerrainManager ();
            m_accounts = new ConcurrentDictionary<Guid, Account> ();
        }

        public RegionManager RegionManager
        {
            get { return m_regionManager; }
//            set { m_regionManager = value; }
        }

        public TerrainManager TerrainManager
        {
            get { return m_terrainManager; }
//            set { m_terrainManager = value; }
        }

        public ConcurrentDictionary<Guid, Account> Accounts
        {
            get { return m_accounts; }
        }

		private RegionManager m_regionManager;
        private TerrainManager m_terrainManager;
        private ConcurrentDictionary<Guid, Account> m_accounts;
	}
}

