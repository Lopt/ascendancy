namespace Client.Common.Views
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Client.Common.Models;
    using CocosSharp;
    using Core.Models.Definitions;

    /// <summary>
    /// Menu view.
    /// </summary>
    public class MenuView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.MenuView"/> class.
        /// </summary>
        /// <param name="menuLayer">Layer where the menu should be drawn.</param>
        /// <param name="center">TileMap Coordinates where the menu should be drawn.</param>
        /// <param name="types">Which menu entries should be shown.</param>
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

            coordHelper[0] = new CCTileMapCoordinates(
                m_center.Column + (m_center.Row % 2),
                m_center.Row - 1);

            coordHelper[1] = new CCTileMapCoordinates(
                m_center.Column + (m_center.Row % 2),
                m_center.Row + 1);

            coordHelper[2] = new CCTileMapCoordinates(
                m_center.Column,
                m_center.Row + 2);

            coordHelper[3] = new CCTileMapCoordinates(
                m_center.Column - ((m_center.Row + 1) % 2),
                m_center.Row + 1);

            coordHelper[4] = new CCTileMapCoordinates(
                m_center.Column - ((m_center.Row + 1) % 2),
                m_center.Row - 1);

            coordHelper[5] = new CCTileMapCoordinates(
                m_center.Column,
                m_center.Row - 2);

            return coordHelper;
        }

        public void DrawMajorMenu()
        {
            var gidhelper = new short[6];
            gidhelper[0] = 54;
            gidhelper[1] = 55;
            gidhelper[2] = 56;
            gidhelper[3] = 57;
            gidhelper[4] = 52;
            gidhelper[5] = 53;
            var surroundedCoords = GetSurroundedTiles();
            for (var index = 0; index < surroundedCoords.Length; ++index)
            {
                var coord = surroundedCoords[index];
                var gid = new CCTileGidAndFlags(gidhelper[index]);
                m_menuLayer.SetTileGID(gid, coord);
            }
        }

        public void ExtendMenu(short gid)
        {
            //up vector(0, 1)
            //upleft vector(-1, 1)
            //upright vector(1, 1)
            //down vector(0, -1)
            //downleft vector(-1, -1)
            //downright vector(1, -1)

            switch(gid)
            {
                case 52:
                    //getdefinition(52(militärgebäude)
                case 53:
                    //getdefinition(53(Zivil)
                case 54:
                    //getdefinition(54(Resourcen)
                case 55:
                    //getdefinition(55(storage)
                    /*
                     * case xx:
                     *     getdefinition(meele)
                     * case xx:
                     *     getdefinition(range)
                     * etc.
                     */


                    break;
            }
        }

        /// <summary>
        /// Draws the menu
        /// </summary>
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
        /// <param name="coord">Coordinate which was selected.</param>
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
        }

        /// <summary>
        /// The m_enter.
        /// </summary>
        private CCTileMapCoordinates m_center;

        /// <summary>
        /// The definition m_types.
        /// </summary>
        private Core.Models.Definitions.Definition[] m_types;

        /// <summary>
        /// The m_menu layer.
        /// </summary>
        private CCTileMapLayer m_menuLayer;
    }
}