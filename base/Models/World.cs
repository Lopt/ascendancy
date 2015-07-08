using System;
using System.Collections.Concurrent;

namespace @base.model
{
	public sealed class World
	{
        private static readonly Lazy<World> lazy =
            new Lazy<World>(() => new World());

        public static World Instance { get { return lazy.Value; } }

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

