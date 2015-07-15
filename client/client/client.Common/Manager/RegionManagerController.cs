using Core.Models;
using Core.Models.Definitions;
using client.Common.Helper;
using client.Common.Controllers;
using client.Common.Models;
using System.Threading.Tasks;


namespace client.Common.Manager
{
    public class RegionManagerController : Core.Controllers.RegionManagerController
    {
        public RegionManagerController()
        {
        }

        #region Regions

        public Region GetRegionByGeolocator()
        {
            var geolocationPosition = Geolocation.Instance.CurrentGamePosition;
            return GetRegionByGamePosition(geolocationPosition);
        }

        public Region GetRegionByGamePosition(Position gameWorldPosition)
        {
            RegionPosition regionPosition = new RegionPosition(gameWorldPosition);
            return GetRegion(regionPosition);
        }

        public override Region GetRegion(RegionPosition regionPosition)
        {
            var region = World.Instance.RegionManager.GetRegion(regionPosition);

            if (!region.Exist)
            {
                LoadRegionAsync(region);
            }
				
            return region;
        }

        private async Task LoadRegionAsync(Region region)
        {
            TerrainDefinition[,] terrain = await NetworkController.Instance.LoadTerrainsAsync(region.RegionPosition);

            if (terrain != null)
                region.AddTerrain(terrain);

            World.Instance.RegionManager.AddRegion(region);
        }

        public async Task LoadRegionsAsync()
        {
            await LoadRegionsAsync(Geolocation.Instance.CurrentRegionPosition);
        }

        public async Task LoadRegionsAsync(RegionPosition regionPosition)
        {
            var WorldRegions = GetWorldNearRegionPositions(regionPosition);

            foreach (var RegionPosition in WorldRegions)
            {
                var region = World.Instance.RegionManager.GetRegion(RegionPosition);

                if (!region.Exist)
                {
                    await LoadRegionAsync(region);
                }
            }
        }

        public async Task<bool> DoActionAsync(Core.Models.Position currentGamePosition, Core.Models.Action[] actions)
        {
            await NetworkController.Instance.DoActionsAsync(currentGamePosition, actions);
            await EntityManagerController.Instance.LoadEntitiesAsync(currentGamePosition, currentGamePosition.RegionPosition);
            return true;
        }


        #endregion

        #region RegionPositions

        public RegionPosition[,] GetWorldNearRegionPositions(RegionPosition regionPosition)
        {
            int offsetX = -2;
            int offsetY = -2;

            RegionPosition[,] worldRegion = new RegionPosition[5, 5];
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    worldRegion[x, y] = new RegionPosition(regionPosition.RegionX + offsetX, regionPosition.RegionY + offsetY);
                    offsetY += 1;
                }
									 
                offsetX += 1;
                offsetY = -2;
            }

            return worldRegion;
        }

        #endregion

        #region private Fields


        #endregion
    }
}

