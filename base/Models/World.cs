namespace Core.Models
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// World which contains all data.
    /// </summary>
    public sealed class World
    {
        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>The singleton instance.</value>
        public static World Instance
        {
            get
            {
                return Singleton.Value;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Core.Models.World"/> class from being created.
        /// </summary>
        private World()
        {
            RegionManager = new RegionManager();
            DefinitionManager = new DefinitionManager();
            AccountManager = new AccountManager();
        }

        /// <summary>
        /// The region manager which contains all available regions.
        /// </summary>
        public RegionManager RegionManager;

        /// <summary>
        /// The definition manager which contains all available definitions.
        /// </summary>
        public DefinitionManager DefinitionManager;

        /// <summary>
        /// The account manager which contains all known accounts.
        /// </summary>
        public AccountManager AccountManager;

        /// <summary>
        /// singleton instance of world.
        /// </summary>
        private static readonly Lazy<World> Singleton =
            new Lazy<World>(() => new World());        
    }
}
