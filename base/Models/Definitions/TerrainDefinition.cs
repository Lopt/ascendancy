using System;

namespace Core.Models.Definitions
{
    /// <summary>
    /// Terrain Definiton which contains informations about a terraintype.
    /// </summary>
    public class TerrainDefinition : Definition
    {

        public TerrainDefinition(EntityType terrainType,
                                 int[] ressources, bool buildable = true,
                                 bool walkable = true, int travelCost = 1)
            : base((int)terrainType)
        {
            Ressources = ressources;
            Buildable = buildable;
            Walkable = walkable;
            TravelCost = travelCost;
        }

        public int[] Ressources
        {
            get;
            private set;
        }


        public bool Buildable
        {
            get;
            private set;
        }

        public bool Walkable
        {
            get;
            private set;
        }

        public int TravelCost
        {
            get;
            private set;
        }
    }
}

