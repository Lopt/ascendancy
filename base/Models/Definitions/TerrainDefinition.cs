﻿namespace Core.Models.Definitions
{
    using System;

    /// <summary>
    /// Terrain definition which contains information about a terrain type.
    /// </summary>
    public class TerrainDefinition : Definition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Definitions.TerrainDefinition"/> class.
        /// </summary>
        /// <param name="terrainType">Terrain type.</param>
        /// <param name="resources">Resources which the user will receive.</param>
        /// <param name="buildable">If set to <c>true</c>, it is buildable.</param>
        /// <param name="walkable">If set to <c>true</c> it is walkable.</param>
        /// <param name="travelCost">Travel cost.</param>
        public TerrainDefinition(
            EntityType terrainType,
            int[] resources,
            bool buildable = true,
            bool walkable = true,
            int travelCost = 1)
            : base((int)terrainType)
        {
            Resources = resources;
            Buildable = buildable;
            Walkable = walkable;
            TravelCost = travelCost;
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <value>The resources.</value>
        public int[] Resources
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Core.Models.Definitions.TerrainDefinition"/> is buildable.
        /// </summary>
        /// <value><c>true</c> if buildable; otherwise, <c>false</c>.</value>
        public bool Buildable
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Core.Models.Definitions.TerrainDefinition"/> is walkable.
        /// </summary>
        /// <value><c>true</c> if walkable; otherwise, <c>false</c>.</value>
        public bool Walkable
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the travel cost.
        /// </summary>
        /// <value>The travel cost.</value>
        public int TravelCost
        {
            get;
            private set;
        }
    }
}