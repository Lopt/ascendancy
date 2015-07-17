using System;
using CocosSharp;
using client.Common.Models;
using System.Collections;
using System.Collections.Generic;
using Core.Models.Definitions;


namespace client.Common.Views
{
    public class MenuView
    {


        public MenuView(CCTileMapLayer menuLayer, CCTileMapCoordinates center, Definition[] types)
        {
            m_center = center;
            m_types = types;
            m_menuLayer = menuLayer;
    
        }

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
        /// Shows the menu at a given Location.
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

        CCTileMapCoordinates m_center;
        Core.Models.Definitions.Definition[] m_types;
        CCTileMapLayer m_menuLayer;
    }
}

