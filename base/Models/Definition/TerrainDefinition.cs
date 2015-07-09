using System;

namespace @base.model.definitions
{
    public class TerrainDefinition : Definition
    {
        public enum TerrainDefinitionType
        {
            // Terrain Range 0-99
            Water = 0,
            Buildings = 1,
            Woods = 2,
            Grassland = 3,
            Fields = 4,
            Streets = 5,
            NotDefined = 6,
            Forbidden = 7,
            Town = 8,
            Glacier = 9,
            Beach = 10,
            Park = 11,
            Invalid = 12
        }

        public TerrainDefinition(TerrainDefinitionType terrainType,
                                 int[] ressources, bool buildable = true,
                                 bool walkable = true, int travelCost = 1)
            : base((int)terrainType)
        {
            Ressources = ressources;
            Buildable = buildable;
            Walkable = walkable;
            TerrainType = terrainType;
            TravelCost = travelCost;
        }

        public int[] Ressources
        {
            get;
            private set;
        }

        public TerrainDefinitionType TerrainType
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

