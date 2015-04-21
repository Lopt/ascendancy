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
                    var file = Constants.regionFile;

                    file = file.Replace("$MajorRegionX", regionPosition.MajorX.ToString());
                    file = file.Replace("$MajorRegionY", regionPosition.MajorY.ToString());
                    file = file.Replace("$MinorRegionX", regionPosition.RegionX.ToString());
                    file = file.Replace("$MinorRegionY", regionPosition.RegionY.ToString());
                    string json = System.IO.File.ReadAllText(file);

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

