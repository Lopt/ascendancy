using System;
using System.Collections.ObjectModel;
using @base.model;
using Newtonsoft.Json;

namespace server.control
{
    public class TerrainManager
	{
        public TerrainManager ()
		{
            var world = World.Instance;
            string json = System.IO.File.ReadAllText(@base.model.Constants.terrainFile);
            var terrainDefintions = JsonConvert.DeserializeObject<ObservableCollection<@base.model.definitions.TerrainDefinition>>(json);

            foreach (var terrain in terrainDefintions)
            {
                world.TerrainManager.AddTerrainDefinition(terrain);
            }
        }

	}
}

            