using System;
using System.Collections.Generic;
using server.model;
using Newtonsoft.Json;

namespace server.control
{
	/// <summary>
	/// Loads the Definitions and contains them.
	/// </summary>
    public class DefinitionManagerController : Core.Controllers.DefinitionManagerController
    {
        public DefinitionManagerController()
        {
            {
                string json = System.IO.File.ReadAllText(ServerConstants.TERRAIN_FILE);
				var terrainDefintions = JsonConvert.DeserializeObject<List<Core.Models.Definitions.TerrainDefinition>>(json);

                foreach (var terrain in terrainDefintions)
                {
                    DefinitionManager.AddDefinition(terrain);
                }
            }

            {
                string json = System.IO.File.ReadAllText(ServerConstants.UNIT_FILE);
				var unitDefintions = JsonConvert.DeserializeObject<List<Core.Models.Definitions.UnitDefinition>>(json);

                foreach (var unit in unitDefintions)
                {
                    DefinitionManager.AddDefinition(unit);
                }
            }
		

        }

    }
}

            