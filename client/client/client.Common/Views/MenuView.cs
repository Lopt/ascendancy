using Core.Models;

namespace Client.Common.Views
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Client.Common.Models;
    using CocosSharp;
    using Core.Models;
    using Core.Models.Definitions;

    /// <summary>
    /// Menu view.
    /// </summary>
    public class MenuView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.MenuView"/> class.
        /// </summary>
        /// <param name="worldlayer">The WorldLayer.</param>
        /// <param name="center">PositionI where the menu should be drawn.</param>
        /// <param name="types">Which menu entries should be shown.</param>
        /// regionmanager übergeben
        public MenuView(WorldLayerHex worldLayerHex)
        {
            m_worldLayerHex = worldLayerHex; 
            m_definitions = new Dictionary<PositionI, Definition>();
        }
            
        /// <summary>
        /// Draws the menu
        /// </summary>
        public void DrawMenu(PositionI positionI, Definition[] types)
        {
            var surroundedFields = Core.Models.LogicRules.GetSurroundedFields(positionI);
            for (var index = 0; index < surroundedFields.Length; ++index)
            {
                var field = surroundedFields[index];
                var gid = ViewDefinitions.Instance.DefinitionToTileGid(types[index], ViewDefinitions.Sort.Menu);
                var regionViewHex = m_worldLayerHex.GetRegionViewHex(field.RegionPosition);
                var menueLayer = regionViewHex.GetTileMap().LayerNamed(Client.Common.Constants.ClientConstants.LAYER_MENU);
                var cell = field.CellPosition;
                menueLayer.SetTileGID(gid, new CCTileMapCoordinates(cell.CellX, cell.CellY));
                m_definitions.Add(field, types[index]);
                m_worldLayerHex.GetRegionViewHex(field.RegionPosition).UglyDraw();
            }
        }

        /// <summary>
        /// Gets the selected definition.
        /// </summary>
        /// <returns>The selected definition.</returns>
        /// <param name="coord">Coordinate which was selected.</param>
        public Core.Models.Definitions.Definition GetSelectedDefinition(PositionI positionI)
        {
            Definition def;
            m_definitions.TryGetValue(positionI, out def);
            return def;
        }

        /// <summary>
        /// Closes the menu.
        /// </summary>
        public void CloseMenu(PositionI positionI)
        {
            foreach (var field in m_definitions.Keys)
            {
                var regionViewHex = m_worldLayerHex.GetRegionViewHex(field.RegionPosition);
                var menueLayer = regionViewHex.GetTileMap().LayerNamed(Client.Common.Constants.ClientConstants.LAYER_MENU);
                var cell = field.CellPosition;
                menueLayer.RemoveTile(new CCTileMapCoordinates(cell.CellX, cell.CellY));
            }
            m_definitions.Clear();
        }

        /// The m_menu layer.
        /// </summary>
        private WorldLayerHex m_worldLayerHex;

        private Dictionary<PositionI, Definition> m_definitions;
    }
}