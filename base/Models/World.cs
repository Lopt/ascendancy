using System;
using System.Collections.Concurrent;

namespace Core.Models
{
    /// <summary>
    /// World which contains all datas.
    /// </summary>
    public sealed class World
    {
        private static readonly Lazy<World> m_singleton =
            new Lazy<World>(() => new World());

        public static World Instance
        {
            get
            {
                return m_singleton.Value;
            }
        }

        private World()
        {
            RegionManager = new RegionManager();
            DefinitionManager = new DefinitionManager();
            AccountManager = new AccountManager();
        }

        public RegionManager RegionManager;
        public DefinitionManager DefinitionManager;
        public AccountManager AccountManager;

    }
}

