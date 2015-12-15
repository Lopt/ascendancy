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
        public MenuView(WorldLayer worldlayer, PositionI center, Definition[] types)
        {
            m_center = center;
            m_types = types;
            m_worldLayer = worldlayer;
            m_extendedMenuPositions = new List<PositionI>();
            m_shownTypes = new Dictionary<CCTileMapCoordinates, Definition>();
        }

        /// <summary>
        /// Tiles in the Direction left Up
        /// for an even X
        /// </summary>
        public static readonly PositionI[] Leftupeven =
        {
            new PositionI(-1,  0),
            new PositionI(-1, -1),
            new PositionI(0, -1)
        };

        /// <summary>
        /// Tiles in the Direction left Up
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] Leftupodd =
        {
            new PositionI(-1,  1),
            new PositionI(-1,  0),
            new PositionI(0, -1)
        };

        /// <summary>
        /// Tiles in the Direction left down
        /// for an even X
        /// </summary>
        public static readonly PositionI[] Leftdowneven =
        {
            new PositionI(0,  1),
            new PositionI(-1,  0),
            new PositionI(-1, -1)
        };

        /// <summary>
        /// Tiles in the Direction left down
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] Leftdownodd =
        {
            new PositionI(0,  1),
            new PositionI(-1,  1),
            new PositionI(-1,  0)
        };

        /// <summary>
        /// Tiles in the Direction Up
        /// for an even X
        /// </summary>
        public static readonly PositionI[] Upeven =
        {
            new PositionI(-1, -1),
            new PositionI(0, -1),
            new PositionI(1, -1)
        };

        /// <summary>
        /// Tiles in the Direction Up
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] Upodd =
        {
            new PositionI(-1,  0),
            new PositionI(0, -1),
            new PositionI(1,  0)
        };

        /// <summary>
        /// Tiles in the Direction right Up
        /// for an even X
        /// </summary>
        public static readonly PositionI[] Rightupeven =
        {
            new PositionI(0, -1),
            new PositionI(1, -1),
            new PositionI(1,  0)
        };
           
        /// <summary>
        /// Tiles in the Direction right Up
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] Rightupodd =
        {
            new PositionI(0, -1),
            new PositionI(1,  0),
            new PositionI(1,  1)
        };

        /// <summary>
        /// Tiles in the Direction right down
        /// for an even X
        /// </summary>
        public static readonly PositionI[] Rightdowneven =
        {
            new PositionI(1, -1),
            new PositionI(1,  0),
            new PositionI(0,  1)
        };

        /// <summary>
        /// Tiles in the Direction right down
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] Rightdownodd =
        {
            new PositionI(1,  0),
            new PositionI(1,  1),
            new PositionI(0,  1)
        };

        /// <summary>
        /// Tiles in the Direction down
        /// for an even X
        /// </summary>
        public static readonly PositionI[] Downeven =
        {   
            new PositionI(1,  0),
            new PositionI(0,  1),
            new PositionI(-1,  0)
        };

        /// <summary>
        /// Tiles in the Direction down
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] Downodd =
        {
            new PositionI(1,  1),
            new PositionI(0,  1),
            new PositionI(-1,  1)
        };

        /// <summary>
        /// The odd coordinates.
        /// </summary>
        public static readonly PositionI[][] Odd =
        {
            Upodd,
            Downodd,
            Leftupodd,
            Rightupodd,
            Leftdownodd,
            Rightdownodd
        };

        /// <summary>
        /// The even coordinates.
        /// </summary>
        public static readonly PositionI[][] Even =
        {
            Upeven,
            Downeven,
            Leftupeven,
            Rightupeven,
            Leftdowneven,
            Rightdowneven
        };

        /// <summary>
        /// Gets one iteration of the expanding menu with the coordinate and the old coordinate as needed parameters to calculate the direction.
        /// </summary>
        /// <param name="coord">The new Coordinate.</param>
        /// <param name="oldcoord">The old Coordinate.</param>
        /// <returns>Returns the second coordinate of the expanding menu.</returns>
        public Core.Models.PositionI Gettiles(PositionI coord, PositionI oldcoord)
        {
            var extMenu = Even[0];
            var tmp = Odd;
            if (coord.X % 2 == 0)
            {
                tmp = Even;
            }
            var vector = new PositionI(coord.X - oldcoord.X, coord.Y - oldcoord.Y);
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
            // right
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
                m_extendedMenuPositions.Add(coord + item);
            }
            return extMenu[1];
        }

        /// <summary>
        /// Gets all extended coordinates for a given needed count.
        /// </summary>
        /// <param name="coord">The pressed Coordinate to know in which direction the menu will expand.</param>
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
            // foreach (var item in m_extendedMenuPositions)
            // {
            //    var coordt = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, item);
            //    var gid = new CCTileGidAndFlags(53);
            //    m_worldLayer.MenuLayer.SetTileGID(gid, coordt);
            // }
        }
                
        /// <summary>
        /// Draws the major menu.
        /// </summary>
        /// <param name="majorgids">The GIDs.</param>
        public void DrawMajorMenu(short[] majorgids)
        {
            var gidhelper = majorgids;
            var surroundedCoords = LogicRules.GetSurroundedFields(m_center);
            for (var index = 0; index < surroundedCoords.Length; ++index)
            {
                var coord = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, surroundedCoords[index]);
                var gid = new CCTileGidAndFlags(gidhelper[index]);
                m_worldLayer.MenuLayer.SetTileGID(gid, coord);
            }
        }

        /// <summary>
        /// Extends the menu.
        /// </summary>
        /// <param name="gid">GID to switch between Military, Storage buildings etc..</param>
        /// <param name="coord">Coordinate to get the direction.</param>
        public void ExtendMenu(short gid, PositionI coord)
        {
            // clear the menu to enable a cleaner experience
            if (m_extendedMenuPositions.Count != 0)
            {
                ClearExtendedMenu();
            }
            var types = new Core.Models.Definitions.Definition[0];
            var defM = Core.Models.World.Instance.DefinitionManager;
            var count = 0;
            switch (gid)
            {
                case Common.Constants.BuildingMenuGid.MILITARY:
                    types = new Core.Models.Definitions.Definition[4];
                    types[0] = defM.GetDefinition(EntityType.Barracks);
                    types[1] = defM.GetDefinition(EntityType.Attachment);
                    types[2] = defM.GetDefinition(EntityType.Factory);
                    types[3] = defM.GetDefinition(EntityType.GuardTower);
                    break;
                case Common.Constants.BuildingMenuGid.ZIVIL:
                    types = new Core.Models.Definitions.Definition[3];
                    types[0] = defM.GetDefinition(EntityType.Hospital);
                    types[1] = defM.GetDefinition(EntityType.Tent); 
                    types[2] = defM.GetDefinition(EntityType.TradingPost);
                    break;
                case Common.Constants.BuildingMenuGid.RESOURCES:
                    types = new Core.Models.Definitions.Definition[3];
                    types[0] = defM.GetDefinition(EntityType.Lab);
                    types[1] = defM.GetDefinition(EntityType.Furnace);
                    types[2] = defM.GetDefinition(EntityType.Transformer);
                    break;
                case Common.Constants.BuildingMenuGid.STORAGE:
                    types = new Core.Models.Definitions.Definition[1];
                    types[0] = defM.GetDefinition(EntityType.Scrapyard);
                    break;
            }
            count = types.Length;
            GetExtendedCoords(coord, count);
            DrawExtendedMenu(types);
        }

        /// <summary>
        /// Draws the extended menu.
        /// </summary>
        /// <param name="types">The definition Types.</param>
        public void DrawExtendedMenu(Core.Models.Definitions.Definition[] types)
        {
            m_shownTypes.Clear();
            for (var index = 0; index < types.Length; ++index)
            {
                var mapCoordinate = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, m_extendedMenuPositions[index]);
                var gid = ViewDefinitions.Instance.DefinitionToTileGid(types[index], ViewDefinitions.Sort.Menu);
                m_worldLayer.MenuLayer.SetTileGID(gid, mapCoordinate);
                m_shownTypes[mapCoordinate] = types[index];
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
                m_shownTypes[mapCoordinate] = m_types[index];
            }
        }

        /// <summary>
        /// Gets the selected definition.
        /// </summary>
        /// <returns>The selected definition.</returns>
        /// <param name="coord">Coordinate which was selected.</param>
        public Core.Models.Definitions.Definition GetSelectedDefinition(CCTileMapCoordinates coord)
        {
            Core.Models.Definitions.Definition def;
            m_shownTypes.TryGetValue(coord, out def);
            return def;
        }
            
        /// <summary>
        /// Clears the extended menu Tiles.
        /// </summary>
        public void ClearExtendedMenu()
        {
            foreach (var coord in m_extendedMenuPositions)
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
        private Dictionary<CCTileMapCoordinates, Core.Models.Definitions.Definition> m_shownTypes;

        /// <summary>
        /// The definition m_types.
        /// </summary>
        private Core.Models.Definitions.Definition[] m_types;

        /// <summary>
        /// The world layer.
        /// </summary>
        private WorldLayer m_worldLayer;

        /// <summary>
        /// A list to hold all the additional Tile Positions.
        /// </summary>
        private List<PositionI> m_extendedMenuPositions;
    }
}