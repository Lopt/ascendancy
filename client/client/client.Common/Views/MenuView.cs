using System;
using CocosSharp;
using Client.Common.Models;
using System.Collections;
using System.Collections.Generic;
using Core.Models.Definitions;


namespace Client.Common.Views
{
    /// <summary>
    /// Menu view.
    /// </summary>
    public class MenuView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.MenuView"/> class.
        /// </summary>
        /// <param name="menuLayer">Menu layer.</param>
        /// <param name="center">Center.</param>
        /// <param name="types">Types.</param>
        public MenuView(CCTileMapLayer menuLayer, CCTileMapCoordinates center, Definition[] types)
        {
            m_center = center;
            m_types = types;
            m_menuLayer = menuLayer;
    
        }

        /// <summary>
        /// Gets the surrounded tiles.
        /// </summary>
        /// <returns>The surrounded tiles.</returns>
        public CCTileMapCoordinates[] GetSurroundedTiles()
        {
            var coordHelper = new CCTileMapCoordinates[6];

            coordHelper[0] = new CCTileMapCoordinates(m_center.Column + (m_center.Row) % 2,
                m_center.Row - 1);

            coordHelper[1] = new CCTileMapCoordinates(m_center.Column + (m_center.Row) % 2,
                m_center.Row + 1);

            coordHelper[2] = new CCTileMapCoordinates(m_center.Column,
                m_center.Row + 2);

            coordHelper[3] = new CCTileMapCoordinates(m_center.Column - (m_center.Row + 1) % 2,
                m_center.Row + 1);

            coordHelper[4] = new CCTileMapCoordinates(m_center.Column - (m_center.Row + 1) % 2,
                m_center.Row - 1);

            coordHelper[5] = new CCTileMapCoordinates(m_center.Column,
                m_center.Row - 2);

            return coordHelper;
        }


        /// <summary>
        /// Draws the menu at a given Location.
        /// </summary>
        /// <param name="location">Touch Location.</param>
        /// <param name="menutype">Menutype.</param>
        public void DrawMenu()
        {
            var surroundedCoords = GetSurroundedTiles();
            for (var index = 0; index < surroundedCoords.Length; ++index)
            {
                var coord = surroundedCoords[index];
                var gid = ViewDefinitions.Instance.DefinitionToTileGid(m_types[index], ViewDefinitions.Sort.Menu);
                m_menuLayer.SetTileGID(gid, coord);
            }
        }

        /// <summary>
        /// Gets the selected definition.
        /// </summary>
        /// <returns>The selected definition.</returns>
        /// <param name="coord">Coordinate.</param>
        public Core.Models.Definitions.Definition GetSelectedDefinition(CCTileMapCoordinates coord)
        {
            var surroundedCoords = GetSurroundedTiles();
            for (var index = 0; index < surroundedCoords.Length; ++index)
            {
                if (coord.Column == surroundedCoords[index].Column &&
                    coord.Row == surroundedCoords[index].Row)
                {
                    return m_types[index];
                }
            }
            return null;
        }

        /// <summary>
        /// Closes the menu.
        /// </summary>
        public void CloseMenu()
        {
            var surroundedCoords = GetSurroundedTiles();
            foreach (var coord in surroundedCoords)
            {
                m_menuLayer.RemoveTile(coord);
            }
            //UglyDraw();
        }

        /// <summary>
        /// The m_enter.
        /// </summary>
        CCTileMapCoordinates m_center;
        /// <summary>
        /// The definition m_types.
        /// </summary>
        Core.Models.Definitions.Definition[] m_types;
        /// <summary>
        /// The m_menu layer.
        /// </summary>
        CCTileMapLayer m_menuLayer;
    }
}

