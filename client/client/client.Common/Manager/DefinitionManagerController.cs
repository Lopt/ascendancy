namespace Client.Common.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using Client.Common.Controllers;
    using Client.Common.Helper;
    using Core.Controllers.Actions;
    using Core.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Definition manager controller loads definitions and fill the definition manager
    /// </summary>
    public class DefinitionManagerController : Core.Controllers.DefinitionManagerController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionManagerController"/> class.
        /// </summary>
        public DefinitionManagerController()
        {
        }

        /// <summary>
        /// Loads the terrain definitions async, serialize the definitions and 
        /// add the definition in to the definition manager.
        /// </summary>
        /// <returns>task for this function.</returns>
        public async Task LoadTerrainDefinitionsAsync()
        {
            var terrainDefintions = await NetworkController.Instance.LoadTerrainTypesAsync();
            foreach (var terrain in terrainDefintions)
            {
                DefinitionManager.AddDefinition(terrain);
            }
        }

        /// <summary>
        /// Loads the Unit definitions async, serialize the definitions and 
        /// add the definition in to the definition manager.
        /// </summary>
        /// <returns>task for this function.</returns>
        public async Task LoadUnitDefinitionsAsync()
        {
            var unitDefinitions = await NetworkController.Instance.LoadUnitTypesAsync();
            foreach (var unitType in unitDefinitions)
            {
                DefinitionManager.AddDefinition(unitType);
            }
        }

        #region private Fields

        #endregion
    }
}