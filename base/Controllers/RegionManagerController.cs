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
    }
}
