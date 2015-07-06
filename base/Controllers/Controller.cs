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

        public RegionManagerController RegionManagerController;
        public DefinitionManagerController DefinitionManagerController;
	}
}

