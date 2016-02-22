namespace Client.Common.Manager
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Client.Common.Controllers;
    using Client.Common.Helper;
    using Client.Common.Models;
    using Core.Models;
    using Core.Models.Definitions;

    /// <summary>
    /// The Region manager controller to control the regions.
    /// </summary>
    public class RegionManagerController : Core.Controllers.RegionManagerController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Manager.RegionManagerController"/> class.
        /// </summary>
        public RegionManagerController()
        {
        }

        #region Regions

        /// <summary>
        /// Gets the region by current geo location.
        /// </summary>
        /// <returns>The region by current geo location.</returns>
        public Region GetRegionByGeolocator()
        {
            var geolocationPosition = Geolocation.Instance.CurrentGamePosition;
            return GetRegionByGamePosition(geolocationPosition);
        }

        /// <summary>
        /// Gets the region by game position.
        /// </summary>
        /// <returns>The region by game position.</returns>
        /// <param name="gameWorldPosition">Game world position.</param>
        public Region GetRegionByGamePosition(Position gameWorldPosition)
        {
            RegionPosition regionPosition = new RegionPosition(gameWorldPosition);
            return GetRegion(regionPosition);
        }

        /// <summary>
        /// Gets the region.If needed loads the region.
        /// </summary>
        /// <returns>The region.</returns>
        /// <param name="regionPosition">Region position.</param>
        public override Region GetRegion(RegionPosition regionPosition)
        {
            var region = World.Instance.RegionManager.GetRegion(regionPosition);

            if (!region.Exist)
            {
                LoadTerrainAsync(region);
            }
            return region;
        }
            
        /// <summary>
        /// Dos the action async.
        /// Send the actions to the server and load the response.
        /// </summary>
        /// <returns>True once the action is done.</returns>
        /// <param name="currentGamePosition">Current game position.</param>
        /// <param name="actions">Actions which should be executed.</param>
        public async Task<bool> DoActionAsync(Core.Models.Position currentGamePosition, Core.Models.Action[] actions)
        {
            await NetworkController.Instance.DoActionsAsync(currentGamePosition, actions);
            return true;
        }

        #endregion

        #region RegionPositions

        /// <summary>
        /// Gets the world near the region position. Around 5x5 regions at the region position.
        /// </summary>
        /// <returns>The regions around the positions.</returns>
        /// <param name="regionPosition">Region position.</param>
        public HashSet<RegionPosition> GetWorldNearRegionPositions(RegionPosition regionPosition)
        {
            var regions = new HashSet<RegionPosition>();

            int halfX = Common.Constants.ClientConstants.DRAW_REGIONS_X / 2;
            int halfY = Common.Constants.ClientConstants.DRAW_REGIONS_Y / 2;

            for (int x = -halfX; x <= halfX; x++)
            {
                for (int y = -halfY; y <= halfY; y++)
                {
                    var regPos = new RegionPosition(regionPosition.RegionX + x, regionPosition.RegionY + y);
                    regions.Add(regPos);
                }
            }

            return regions;
        }

        /// <summary>
        /// Loads the terrain async over the network controller and adds the region to the world.
        /// </summary>
        /// <returns>The function as task.</returns>
        /// <param name="region">Region which should be loaded.</param>
        public async Task LoadTerrainAsync(Region region)
        {
            TerrainDefinition[,] terrain = await NetworkController.Instance.LoadTerrainsAsync(region.RegionPosition);

            if (terrain != null)
            {
                region.AddTerrain(terrain);
            }
            try
            {
                World.Instance.RegionManager.AddRegion(region);
            }
            catch
            {
            }
        }

        #endregion

        #region private Fields

        #endregion
    }
}