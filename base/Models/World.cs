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
                    instance = new World ();
                }
                return instance;
            }
        }

        private World()
        {
            RegionManager = new RegionManager ();
            DefinitionManager = new DefinitionManager ();
            AccountManager = new AccountManager ();
        }

        public RegionManager RegionManager;
        public DefinitionManager DefinitionManager;
        public AccountManager AccountManager;

	}
}

