﻿namespace Core.Helper
{
    using System;
    using Core.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Helper, used to load a region.
    /// </summary>
    public class LoadHelper
    {
        /// <summary>
        /// Replaces parts of the path with MajorRegion and MinorRegion of the given Region Position
        /// </summary>
        /// <returns>Path with replaced $MajorRegion and $MinorRegion </returns>
        /// <param name="path">Template Path</param>
        /// <param name="regionPosition">Region Position.</param>
        public static string ReplacePath(string path, Core.Models.RegionPosition regionPosition)
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
        public static Core.Models.Definitions.TerrainDefinition[,] JsonToTerrain(string json)
        {
            var definitionManager = Core.Models.World.Instance.DefinitionManager;

            int[,] terrainsTypes = JsonConvert.DeserializeObject<int[,]>(json);
            var terrains = new Core.Models.Definitions.TerrainDefinition[Constants.REGION_SIZE_X, Constants.REGION_SIZE_Y];

            for (int cellX = 0; cellX < Constants.REGION_SIZE_X; ++cellX)
            {
                for (int cellY = 0; cellY < Constants.REGION_SIZE_Y; ++cellY)
                {
                    var terrainId = terrainsTypes[cellX, cellY];
                    terrains[cellX, cellY] = (Core.Models.Definitions.TerrainDefinition)definitionManager.GetDefinition((Core.Models.Definitions.EntityType)terrainId);
                }
            }
            return terrains;
        }

        /// <summary>
        /// Converts a JSON String to a Region.
        /// </summary>
        /// <returns>Region which was created by the JSON String.</returns>
        /// <param name="json">JSON loaded from the server</param>
        /// <param name="regionPosition">Region position.</param>
        public static Core.Models.Region JsonToRegion(string json, Core.Models.RegionPosition regionPosition)
        {   
            var terrains = JsonToTerrain(json);
            return new Region(regionPosition, terrains);
        }
    }
}