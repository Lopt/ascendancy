using System;

namespace @base.model.definitions
{
    public class TerrainDefinition : Definition
    {
        enum TerrainDefintionType
        {
            Water = 0,
            Street = 1,
            Building = 2,
            Wood = 3,
        }
      
        public TerrainDefinition(Guid guid, DefinitionType type,
            UnitType terrainType, int[] ressources)
        {
            base.TerrainDefinition(guid, type);

            m_terrainType = terrainType;
            m_ressources = ressources;
        }

        public int Ressources
        {
            get { return m_ressources; }
        }

        public TerrainDefintionType TerrainType
        {
            get { return m_terrainType; }
        }

        private TerrainDefintionType m_terrainType;
        private int m_ressources;

    }
}

