using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using @base.model;
using @base.model.definitions;

namespace @base.control
{
    public class RegionManager
    {
        /// <summary>
        /// Paths the replace.
        /// </summary>
        /// <returns>The replace.</returns>
        /// <param name="path">Path.</param>
        /// <param name="regionPosition">Region position.</param>
        public string ReplacePath(string path, RegionPosition regionPosition)
        {
            path = path.Replace("$MajorRegionX", regionPosition.MajorX.ToString());
            path = path.Replace("$MajorRegionY", regionPosition.MajorY.ToString());
            path = path.Replace("$MinorRegionX", regionPosition.RegionX.ToString());
            path = path.Replace("$MinorRegionY", regionPosition.RegionY.ToString());

            return path;
        }

        public Region JsonToRegion(string json, RegionPosition regionPosition)
        {           
            var terrainManager = World.Instance.TerrainManager;

            int[,] terrainsTypes = JsonConvert.DeserializeObject<int[,]>(json);
            var terrains = new TerrainDefinition[Constants.REGIONSIZE_X, Constants.REGIONSIZE_Y];

            for (int cellX = 0; cellX < Constants.REGIONSIZE_X; ++cellX)
            {
                for (int cellY = 0; cellY < Constants.REGIONSIZE_Y; ++cellY)
                {
                    var terrainType = terrainsTypes[cellX, cellY];
                    terrains[cellX, cellY] = terrainManager.GetTerrainDefinition(
                        (TerrainDefinition.TerrainDefinitionType)terrainType);
                }
            }

            return new Region(regionPosition, terrains);
        }
    }
}

