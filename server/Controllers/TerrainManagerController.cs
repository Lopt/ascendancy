using System;
using System.Collections.ObjectModel;
using @base.model;
using Newtonsoft.Json;

namespace server.control
{
	public class TerrainManagerController : @base.control.TerrainManagerController
	{
		public TerrainManagerController ()
		{
            var world = World.Instance;
            string json = System.IO.File.ReadAllText(@base.model.Constants.TERRAIN_FILE);
            var terrainDefintions = JsonConvert.DeserializeObject<ObservableCollection<@base.model.definitions.TerrainDefinition>>(json);

            foreach (var terrain in terrainDefintions)
            {
                world.TerrainManager.AddTerrainDefinition(terrain);
            }
        }

	}
}

            