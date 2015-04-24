using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using @base.model;
using @base.model.definitions;

namespace @base.control
{
    public class TerrainManagerController 
    {
        public TerrainManagerController()
        {
            m_terrainManager = World.Instance.TerrainManager;
        }

        public TerrainManager TerrainManager
        {
            get { return m_terrainManager; }
        }

        TerrainManager m_terrainManager;
    }



}

