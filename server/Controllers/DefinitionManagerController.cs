using System;
using System.Collections.ObjectModel;
using server.model;
using Newtonsoft.Json;

namespace server.control
{
	public class DefinitionManagerController : @base.control.DefinitionManagerController
	{
		public DefinitionManagerController ()
		{
			{
				string json = System.IO.File.ReadAllText (ServerConstants.TERRAIN_FILE);
				var terrainDefintions = JsonConvert.DeserializeObject<ObservableCollection<@base.model.definitions.TerrainDefinition>> (json);

				foreach (var terrain in terrainDefintions)
				{
					DefinitionManager.AddDefinition (terrain);
				}
			}

			{/*
				string json = System.IO.File.ReadAllText (ServerConstants.UNIT_FILE);
				var unitDefintions = JsonConvert.DeserializeObject<ObservableCollection<@base.model.definitions.UnitDefinition>>(json);

				foreach (var unit in unitDefintions)
				{
					DefinitionManager.AddDefinition(unit);
				}*/
			}
		

        }

	}
}

            