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
            var last = new RegionManager();
            var curr = new RegionManager(last);
            var next = new RegionManager(curr);

            RegionStates = new RegionStates (last, curr, next);
            DefinitionManager = new DefinitionManager ();
            Accounts = new ConcurrentDictionary<int, Account> ();
        }

        public RegionStates RegionStates;
        public DefinitionManager DefinitionManager;
        public ConcurrentDictionary<int, Account> Accounts;

	}
}

