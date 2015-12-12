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
            m_extendedMenuPositions = new List<PositionI>();
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

        /// <summary>
        /// The odd coordinates.
        /// </summary>
        public static readonly PositionI[][] odd =
        {
            upodd,
            downodd,
            leftupodd,
            rightupodd,
            leftdownodd,
            rightdownodd
        };

        /// <summary>
        /// The even coordinates.
        /// </summary>
        public static readonly PositionI[][] even =
        {
            upeven,
            downeven,
            leftupeven,
            rightupeven,
            leftdowneven,
            rightdowneven

        };

        /// <summary>
        /// Gets one iteration of the expanding menu with coord and oldcoord as needed parameters to calculate the direction.
        /// </summary>
        /// <param name="coord">The new Coordinate.</param>
        /// <param name="oldcoord">The old Coordinate.</param>
        /// <returns>Returns the middel coordinate of the expanding menu.</returns>
        public Core.Models.PositionI Gettiles(PositionI coord, PositionI oldcoord)
        {
            var extMenu = even[0];
            var tmp = odd;
            if (coord.X % 2 == 0)
            {
                tmp = even;
            }
            var vector = new PositionI( coord.X - oldcoord.X, coord.Y - oldcoord.Y );
            if (vector.X == 0)
            {
                if (vector.Y == -1)
                {
                    extMenu = tmp[0];
                }
                else
                {
                    extMenu = tmp[1];
                } 
            }
            //right
            if (vector.X == 1)
            {
                if ((vector.Y > 0) || (coord.X % 2 != 0 && vector.Y == 0))
                {
                    extMenu = tmp[5]; // down
                }
                else
                {
                    extMenu = tmp[3]; // up
                }
            }
            if (vector.X == -1)
            {
                if ((vector.Y > 0) || (coord.X % 2 != 0 && vector.Y == 0))
                {
                    extMenu = tmp[4]; // down
                }
                else
                {
                    extMenu = tmp[2]; // up
                }
            }
            foreach (var item in extMenu)
            {
                m_extendedMenuPositions.Add(coord+item);
            }
            return extMenu[1];
        }

        /// <summary>
        /// Gets all extended coords for a given needed tilecount.
        /// </summary>
        /// <param name="coord">The pressed Coordinate to know in wich direction the menu will expand.</param>
        /// <param name="count">The count of needed Tiles.</param>
        public void GetExtendedCoords(PositionI coord, int count)
        {
            var tmpcoord1 = coord;
            var tmpcoord2 = m_center;
            var counter = count;
            while (counter > 0)
            {
                var test = Gettiles(tmpcoord1, tmpcoord2);
                tmpcoord2 = tmpcoord1;
                tmpcoord1 = tmpcoord1 + test;
                counter -= 3;
            }

            // Testdraw to make sure it works properly
            //foreach (var item in m_extendedMenuPositions)
            //{
            //    var coordt = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, item);
            //    var gid = new CCTileGidAndFlags(53);
            //    m_worldLayer.MenuLayer.SetTileGID(gid, coordt);
            //}
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
            var surroundedCoords = LogicRules.GetSurroundedFields(m_center);
            for (var index = 0; index < surroundedCoords.Length; ++index)
            {
                var coord = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, surroundedCoords[index]);
                var gid = new CCTileGidAndFlags(52);
                m_worldLayer.MenuLayer.SetTileGID(gid, coord);
            }
        }

        public void ExtendMenu(short gid, Core.Models.Definitions.Definition[] types, PositionI coord)
        {
            //clear the menu to enable a cleaner experience
            if (m_extendedMenuPositions.Count != 0)
            {
                ClearExtendedMenu();
            }
            var count = types.Length;
            GetExtendedCoords(coord, count);
            switch(gid)
            {
                case 52:
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
        /// Clears the extended menu Tiles.
        /// </summary>
        public void ClearExtendedMenu()
        {
            foreach( var coord in m_extendedMenuPositions)
            {
                var ecoord = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, coord);
                m_worldLayer.MenuLayer.RemoveTile(ecoord);
            }
            m_extendedMenuPositions.Clear();
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
            if (m_extendedMenuPositions.Count != 0)
            {
                ClearExtendedMenu();
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

        /// <summary>
        /// A list to hold all the additional Tile Positions.
        /// </summary>
        private List<PositionI> m_extendedMenuPositions;
    }
}