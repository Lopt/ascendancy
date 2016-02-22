namespace Core.Models.Definitions
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
        /// <param name="defenseModifier">Defense modifier.</param>
        /// <param name="attackModifier">Attack modifier.</param>
        public TerrainDefinition(
            EntityType terrainType,
            int[] resources,
            bool buildable,
            bool walkable,
            int travelCost,
            int defenseModifier,
            int attackModifier)
            : base((int)terrainType)
        {
            Resources = resources;
            Buildable = buildable;
            Walkable = walkable;
            TravelCost = travelCost;
            DefenseModifier = defenseModifier;
            AttackModifier = attackModifier;
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

        /// <summary>
        /// Gets the attack modifier.
        /// </summary>
        /// <value>The attack modifier.</value>
        public int AttackModifier
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the defense modifier.
        /// </summary>
        /// <value>The defense modifier.</value>
        public int DefenseModifier
        {
            get;
            private set;
        }
    }
}