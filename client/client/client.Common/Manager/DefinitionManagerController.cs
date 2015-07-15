using Core.Controllers.Actions;
using Core.Models;
using client.Common.Controllers;
using client.Common.Helper;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;


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
            var terrainDefintions = await m_Network.LoadTerrainTypesAsync();
			foreach (var terrain in terrainDefintions)
			{
				DefinitionManager.AddDefinition(terrain);
			}
		}

		/// <summary>
		/// Loads the Unit definitions async, serialize the definitions and 
		/// add the definition in to the definition manager.
		/// </summary>
		public async Task LoadUnitDefinitionsAsync()
		{
			var unitDefinitions = await m_Network.LoadUnitTypesAsync();
            foreach (var unitType in unitDefinitions)
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

