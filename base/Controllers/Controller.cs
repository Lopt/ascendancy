using System;

namespace @base.control
{
	public class Controller
	{
		// TODO: find better singleton implementation
		// http://csharpindepth.com/articles/general/singleton.aspx
		// NOT lazy-singletons: throws useless exceptions when initialisation failed
		private static Controller instance=null;

		public static Controller Instance
		{
			get
			{
				if (instance==null)
				{
					instance = new Controller();
				}
				return instance;
			}
		}

		private Controller()
		{
		}

		public RegionManagerController RegionManagerController
		{
			get { return m_regionManagerController; }
            set { m_regionManagerController = value; }
		}

        public TerrainManagerController TerrainManagerController
        {
            get { return m_terrainManagerController; }
            set { m_terrainManagerController = value; }
        }

        public AccountManagerController AccountManagerController
        {
            get { return m_accountManagerController; }
            set { m_accountManagerController = value; }
        }

        RegionManagerController m_regionManagerController;
        TerrainManagerController m_terrainManagerController;
        AccountManagerController m_accountManagerController;
	}
}

