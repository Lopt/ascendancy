using Core.Controllers.Actions;
using Core.Models;
using client.Common.Controllers;
using client.Common.Helper;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;


namespace client.Common.Manager
{
	/// <summary>
	/// Definition manager controller laod definitions and fill the definition manager
	/// </summary>
	public class DefinitionManagerController : Core.Controllers.DefinitionManagerController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="client.Common.Manager.DefinitionManagerController"/> class.
		/// </summary>
		public DefinitionManagerController()
		{
			m_Network = NetworkController.Instance;
		}

		/// <summary>
		/// Loads the terrain definitions async, serialize the definitions and 
		/// add the definition in to the definition manager.
		/// </summary>
		public async Task LoadTerrainDefinitionsAsync()
		{
			await m_Network.LoadTerrainTypesAsync(ClientConstants.TERRAIN_TYPES_SERVER_PATH);

			var json = m_Network.JsonTerrainTypeString;
			var terrainDefintions = JsonConvert.DeserializeObject<ObservableCollection<Core.Models.Definitions.TerrainDefinition>>(json);

			foreach (var terrain in terrainDefintions)
			{
				DefinitionManager.AddDefinition(terrain);

			}
		}

		/// <summary>
		/// Loads the Entity definitions async, serialize the definitions and 
		/// add the definition in to the definition manager.
		/// </summary>
		public async Task LoadEntityDefinitionsAsync()
		{
			await m_Network.LoadTerrainTypesAsync(ClientConstants.ENTITY_TYPES_SERVER_PATH);

			var json = m_Network.JsonTerrainTypeString;
			var unitDefintions = JsonConvert.DeserializeObject<ObservableCollection<Core.Models.Definitions.UnitDefinition>>(json);

			foreach (var unitType in unitDefintions)
			{
				DefinitionManager.AddDefinition(unitType);

			}
		}




		#region private Fields

		/// <summary>
		/// The network controller.
		/// </summary>
		private NetworkController m_Network;

		#endregion
	}
}

