namespace Core.Controllers
{
    using System;
    using Core.Models;

    /// <summary>
    /// Loads and adds regions to the region manager
    /// </summary>
    public class RegionManagerController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.RegionManagerController"/> class.
        /// </summary>
        public RegionManagerController()
        {
            m_regionManager = World.Instance.RegionManager;
        }

        /// <summary>
        /// Gets the region manager.
        /// </summary>
        /// <value>The region manager.</value>
        public RegionManager RegionManager
        {
            get
            {
                return m_regionManager;
            }
        }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <returns>The region.</returns>
        /// <param name="regionPosition">Region position.</param>
        public virtual Region GetRegion(RegionPosition regionPosition)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The m region manager.
        /// </summary>
        private RegionManager m_regionManager;
    }
}
