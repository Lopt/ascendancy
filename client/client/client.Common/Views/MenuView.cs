namespace Client.Common.Views
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Client.Common.Models;
    using CocosSharp;
    using Core.Models.Definitions;
    using Core.Models;


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
        public MenuView(WorldLayer worldlayer, PositionI center, Definition[] types)
        {
            m_center = center;
            m_types = types;
            m_worldLayer = worldlayer;
        }

        /// <summary>
        /// Tiles in the Direction left Up
        /// for an even X
        /// </summary>
        public static readonly PositionI[] leftupeven =
        {
            new PositionI(-1,  0),
            new PositionI(-1, -1),
            new PositionI( 0, -1)
        };

        /// <summary>
        /// Tiles in the Direction left Up
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] leftupodd =
        {
            new PositionI(-1,  1),
            new PositionI(-1,  0),
            new PositionI( 0, -1)
        };

        /// <summary>
        /// Tiles in the Direction left down
        /// for an even X
        /// </summary>
        public static readonly PositionI[] leftdowneven =
        {
            new PositionI( 0,  1),
            new PositionI(-1,  0),
            new PositionI(-1, -1)
        };

        /// <summary>
        /// Tiles in the Direction left down
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] leftdownodd =
        {
            new PositionI( 0,  1),
            new PositionI(-1,  1),
            new PositionI(-1,  0)
        };

        /// <summary>
        /// Tiles in the Direction Up
        /// for an even X
        /// </summary>
        public static readonly PositionI[] upeven =
        {
            new PositionI(-1, -1),
            new PositionI( 0, -1),
            new PositionI( 1, -1)
        };

        /// <summary>
        /// Tiles in the Direction Up
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] upodd =
        {
            new PositionI(-1,  0),
            new PositionI( 0, -1),
            new PositionI( 1,  0)
        };

        /// <summary>
        /// Tiles in the Direction right Up
        /// for an even X
        /// </summary>
        public static readonly PositionI[] rightupeven =
        {
            new PositionI( 0, -1),
            new PositionI( 1, -1),
            new PositionI( 1,  0)
        };
           
        /// <summary>
        /// Tiles in the Direction right Up
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] rightupodd =
        {
            new PositionI( 0, -1),
            new PositionI( 1,  0),
            new PositionI( 1,  1)
        };

        /// <summary>
        /// Tiles in the Direction right down
        /// for an even X
        /// </summary>
        public static readonly PositionI[] rightdowneven =
        {
            new PositionI( 1, -1),
            new PositionI( 1,  0),
            new PositionI( 0,  1)
        };

        /// <summary>
        /// Tiles in the Direction right down
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] rightdownodd =
        {
            new PositionI( 1,  0),
            new PositionI( 1,  1),
            new PositionI( 0,  1)
        };

        /// <summary>
        /// Tiles in the Direction down
        /// for an even X
        /// </summary>
        public static readonly PositionI[] downeven =
        {   
            new PositionI( 1,  0),
            new PositionI( 0,  1),
            new PositionI(-1,  0)
        };

        /// <summary>
        /// Tiles in the Direction down
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] downodd =
        {
            new PositionI( 1,  1),
            new PositionI( 0,  1),
            new PositionI(-1,  1)
        };
                
        public void DrawMajorMenu()
        {
            var gidhelper = new short[6];
            gidhelper[0] = 54;
            gidhelper[1] = 55;
            gidhelper[2] = 56;
            gidhelper[3] = 57;
            gidhelper[4] = 52;
            gidhelper[5] = 53;
            var surroundedCoords = LogicRules.GetSurroundedFields(m_center);
            for (var index = 0; index < surroundedCoords.Length; ++index)
            {
                var coord = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, surroundedCoords[index]);
                var gid = new CCTileGidAndFlags(gidhelper[index]);
                m_worldLayer.MenuLayer.SetTileGID(gid, coord);
            }
        }

        public void ExtendMenu(short gid, Core.Models.Definitions.Definition[] types, CCTileMapCoordinates coord)
        {

            var testcoord = new CCTileMapCoordinates();
            testcoord.Column = -1;
            testcoord.Row = -1;
            //up vector(0, 1)
            //upleft vector(-1, 1)
            //upright vector(1, 1)
            //down vector(0, -1)
            //downleft vector(-1, -1)
            //downright vector(1, -1)
            var count = types.Length;
            var menu = new CCTileMapCoordinates[count];

            switch(gid)
            {
                case 52:
//                    for (int i = 0; i < count; ++i)
//                    {
//                        menu[i] = new CCTileMapCoordinates(coord.Column - 1, coord.Row - 1);
//                        var tgid = ViewDefinitions.Instance.DefinitionToTileGid(m_types[i], ViewDefinitions.Sort.Menu);
//                        m_menuLayer.SetTileGID(tgid, menu[i]);
//                    }
                    break;
                    //getdefinition(52(militärgebäude)
                case 53:
                    break;
                    //getdefinition(53(Zivil)
                case 54:
                    break;
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
            var surroundedCoords = LogicRules.GetSurroundedFields(m_center);
            for (var index = 0; index < surroundedCoords.Length; ++index)
            {
                var mapCoordinate = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, surroundedCoords[index]);
                var gid = ViewDefinitions.Instance.DefinitionToTileGid(m_types[index], ViewDefinitions.Sort.Menu);
                m_worldLayer.MenuLayer.SetTileGID(gid, mapCoordinate);
            }
        }

        /// <summary>
        /// Gets the selected definition.
        /// </summary>
        /// <returns>The selected definition.</returns>
        /// <param name="coord">Coordinate which was selected.</param>
        public Core.Models.Definitions.Definition GetSelectedDefinition(CCTileMapCoordinates coord)
        {
            var surroundedCoords = LogicRules.GetSurroundedFields(m_center);
            for (var index = 0; index < surroundedCoords.Length; ++index)
            {
                var mapCoordinate = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, surroundedCoords[index]);
                if (coord.Column == mapCoordinate.Column &&
                    coord.Row == mapCoordinate.Row)
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
            var surroundedCoords = LogicRules.GetSurroundedFields(m_center);
            foreach (var coord in surroundedCoords)
            {
                var mapCoordinate = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, coord);
                m_worldLayer.MenuLayer.RemoveTile(mapCoordinate);
            }
        }

        /// <summary>
        /// The Center Position of the current menu.
        /// </summary>
        private PositionI m_center;

        /// <summary>
        /// The definition m_types.
        /// </summary>
        private Core.Models.Definitions.Definition[] m_types;

        /// <summary>
        /// The Worldlayer.
        /// </summary>
        private WorldLayer m_worldLayer;
    }
}