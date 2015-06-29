using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using @base.model;
using @base.model.definitions;

namespace @base.control
{
    public class RegionManagerController
    {
        public RegionManagerController()
        {
        }

        virtual public Region GetRegion(RegionPosition regionPosition)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Replaces parts of the path with MajorRegion and MinorRegion of the given Region Position
        /// </summary>
        /// <returns>Path with replaced $MajorRegion and $MinorRegion </returns>
        /// <param name="path">Template-Path</param>
        /// <param name="regionPosition">Region Position.</param>
        public string ReplacePath(string path, RegionPosition regionPosition)
        {
            path = path.Replace("$MajorRegionX", regionPosition.MajorX.ToString());
            path = path.Replace("$MajorRegionY", regionPosition.MajorY.ToString());
            path = path.Replace("$MinorRegionX", regionPosition.RegionX.ToString());
            path = path.Replace("$MinorRegionY", regionPosition.RegionY.ToString());

            return path;
        }

        /// <summary>
        /// Converts JSON to an TerrainDefinition[ , ]
        /// </summary>
        /// <returns>A two-dimensional array of TerrainDefinitions</returns>
        /// <param name="json">JSON loaded from the server</param>
        public  TerrainDefinition[ , ] JsonToTerrain(string json)
        {
            var definitionManager = World.Instance.DefinitionManager;

            int[,] terrainsTypes = JsonConvert.DeserializeObject<int[,]>(json);
            var terrains = new TerrainDefinition[Constants.REGION_SIZE_X, Constants.REGION_SIZE_Y];

            for (int cellX = 0; cellX < Constants.REGION_SIZE_X; ++cellX)
            {
                for (int cellY = 0; cellY < Constants.REGION_SIZE_Y; ++cellY)
                {
                    var terrainId = terrainsTypes[cellX, cellY];
                    terrains[cellX, cellY] = (TerrainDefinition) definitionManager.GetDefinition(terrainId);
                }
            }
            return terrains;
        }

        /// <summary>
        /// Converts a JSON String to a Region.
        /// </summary>
        /// <returns>Region which was created by the JSON String.</returns>
        /// <param name="json">JSON</param>
        /// <param name="regionPosition">Region position.</param>
        public Region JsonToRegion(string json, RegionPosition regionPosition)
        {   
            var terrains = JsonToTerrain(json);
            return new Region(regionPosition, terrains);
        }
    }
}

