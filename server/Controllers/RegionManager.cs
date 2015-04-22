using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using @base.model;
using @base.model.definitions;

namespace server.control
{
    public class RegionManager
	{
        public RegionManager()
        {
            for (int regionX = Constants.startX; regionX < Constants.endX; ++ regionX)
            {
                for (int regionY = Constants.startY; regionY < Constants.endY; ++ regionY)
                {
                    var terrainManager = World.Instance.TerrainManager;
                    var regionPosition = new RegionPosition(regionX, regionY);
                    var path = Constants.regionFile;

                    path = path.Replace("$MajorRegionX", regionPosition.MajorX.ToString());
                    path = path.Replace("$MajorRegionY", regionPosition.MajorY.ToString());
                    path = path.Replace("$MinorRegionX", regionPosition.RegionX.ToString());
                    path = path.Replace("$MinorRegionY", regionPosition.RegionY.ToString());
                    string json = System.IO.File.ReadAllText(path);

                    int[,] terrainsTypes = JsonConvert.DeserializeObject<int[,]>(json);
                    var terrains = new TerrainDefinition[Constants.regionSizeX, Constants.regionSizeY];

                    for (int cellX = 0; cellX < Constants.regionSizeX; ++cellX)
                    {
                        for (int cellY = 0; cellY < Constants.regionSizeY; ++cellY)
                        {
                            var terrainType = terrainsTypes[cellX, cellY];
                            terrains[cellX, cellY] = terrainManager.GetTerrainDefinition((TerrainDefinition.TerrainDefinitionType) terrainType);
                        }
                    }

                    var region = new Region(regionPosition, terrains);
                    World.Instance.RegionManager.AddRegion(region);

                }
            }
        }
    }
}

