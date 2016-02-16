using Xamarin.Forms;

namespace Client.Common.Views
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Client.Common.Helper;
    using Client.Common.Models;
    using CocosSharp;
    using Core.Models;
    using Core.Models.Definitions;

    /// <summary>
    /// Menu view.
    /// </summary>
    public class MenuView
    {
        public enum MenuType
        {
            Unity,
            Headquarter,
            Major,
            Extended,
            Empty,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.MenuView"/> class.
        /// </summary>
        /// <param name="worldlayer">The WorldLayer.</param>
        /// <param name="center">PositionI where the menu should be drawn.</param>
        /// <param name="types">Which menu entries should be shown.</param>
        public MenuView(WorldLayerHex worldlayer, PositionI center, MenuType menuType)
        {
            m_center = center;
            var defM = Core.Models.World.Instance.DefinitionManager;
            if (menuType == MenuType.Headquarter)
            {
                m_types = new Core.Models.Definitions.Definition[6];
                m_types[0] = defM.GetDefinition(EntityType.Headquarter);
                m_types[1] = defM.GetDefinition(EntityType.Headquarter);
                m_types[2] = defM.GetDefinition(EntityType.Headquarter);
                m_types[3] = defM.GetDefinition(EntityType.Headquarter);
                m_types[4] = defM.GetDefinition(EntityType.Headquarter);
                m_types[5] = defM.GetDefinition(EntityType.Headquarter);
            }
            else if (menuType == MenuType.Major)
            {
                m_types = new Core.Models.Definitions.Definition[0];
            }
            else if (menuType == MenuType.Unity)
            {
                m_types = new Core.Models.Definitions.Definition[6];
                m_types[0] = defM.GetDefinition(EntityType.Mage);
                m_types[1] = defM.GetDefinition(EntityType.Fencer);
                m_types[2] = defM.GetDefinition(EntityType.Warrior);
                m_types[3] = defM.GetDefinition(EntityType.Scout);
                m_types[4] = defM.GetDefinition(EntityType.Archer);
                m_types[5] = defM.GetDefinition(EntityType.Archer);
            }
            else if (menuType == MenuType.Empty)
            {
                m_types = new Core.Models.Definitions.Definition[6];
            }
            else
            {
                m_types = new Core.Models.Definitions.Definition[0]; 
            }
            m_worldLayer = worldlayer;
            m_extendedMenuPositions = new Dictionary<PositionI, Core.Models.Definitions.Definition>();
            m_baseMenuPositions = new Dictionary<PositionI, CCTileGidAndFlags>();
            m_sprites = new Dictionary<PositionI, CCSprite>();
        }

        /// <summary>
        /// Tiles in the Direction left Up
        /// for an even X
        /// </summary>
        public static readonly PositionI[] Leftupeven =
            {
                new PositionI(-1, 0),
                new PositionI(-1, -1),
                new PositionI(0, -1)
            };

        /// <summary>
        /// Tiles in the Direction left Up
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] Leftupodd =
            {
                new PositionI(-1, 1),
                new PositionI(-1, 0),
                new PositionI(0, -1)
            };

        /// <summary>
        /// Tiles in the Direction left down
        /// for an even X
        /// </summary>
        public static readonly PositionI[] Leftdowneven =
            {
                new PositionI(0, 1),
                new PositionI(-1, 0),
                new PositionI(-1, -1)
            };

        /// <summary>
        /// Tiles in the Direction left down
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] Leftdownodd =
            {
                new PositionI(0, 1),
                new PositionI(-1, 1),
                new PositionI(-1, 0)
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
                new PositionI(-1, 0),
                new PositionI(0, -1),
                new PositionI(1, 0)
            };

        /// <summary>
        /// Tiles in the Direction right Up
        /// for an even X
        /// </summary>
        public static readonly PositionI[] Rightupeven =
            {
                new PositionI(0, -1),
                new PositionI(1, -1),
                new PositionI(1, 0)
            };
           
        /// <summary>
        /// Tiles in the Direction right Up
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] Rightupodd =
            {
                new PositionI(0, -1),
                new PositionI(1, 0),
                new PositionI(1, 1)
            };

        /// <summary>
        /// Tiles in the Direction right down
        /// for an even X
        /// </summary>
        public static readonly PositionI[] Rightdowneven =
            {
                new PositionI(1, -1),
                new PositionI(1, 0),
                new PositionI(0, 1)
            };

        /// <summary>
        /// Tiles in the Direction right down
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] Rightdownodd =
            {
                new PositionI(1, 0),
                new PositionI(1, 1),
                new PositionI(0, 1)
            };

        /// <summary>
        /// Tiles in the Direction down
        /// for an even X
        /// </summary>
        public static readonly PositionI[] Downeven =
            {   
                new PositionI(1, 0),
                new PositionI(0, 1),
                new PositionI(-1, 0)
            };

        /// <summary>
        /// Tiles in the Direction down
        /// for an odd X
        /// </summary>
        public static readonly PositionI[] Downodd =
            {
                new PositionI(1, 1),
                new PositionI(0, 1),
                new PositionI(-1, 1)
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
        public Core.Models.PositionI GetTiles(PositionI coord, PositionI oldcoord)
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
                m_extendedMenuPositions[coord + item] = null;
            }
            return extMenu[1];
        }

        /// <summary>
        /// Gets all extended coordinates for a given needed count.
        /// </summary>
        /// <param name="coord">The pressed Coordinate to know in which direction the menu will expand.</param>
        /// <param name="count">The count of needed Tiles.</param>
        public void GetExtendedCoords(PositionI coord, Core.Models.Definitions.Definition[] definitions)
        {
            var tmpcoord1 = coord;
            var tmpcoord2 = m_center;
            var counter = definitions.Length;
            while (counter > 0)
            {
                var newCenterAdd = GetTiles(tmpcoord1, tmpcoord2);
                tmpcoord2 = tmpcoord1;
                tmpcoord1 = tmpcoord1 + newCenterAdd;
                counter -= 3;
            }

            var keys = new List<PositionI>(m_extendedMenuPositions.Keys);
            for (var index = 0; index < definitions.Length; ++index)
            {
                m_extendedMenuPositions[keys[index]] = definitions[index];
            }
        }

        /// <summary>
        /// Set the major menu.
        /// </summary>
        /// <param name="majorgids">The GIDs.</param>
        private void SetMajorMenu(MenuType menuType)
        {
            var Gids = new short[6];
            Gids[5] = Client.Common.Constants.BuildingMenuGid.MILITARY;
            Gids[0] = Client.Common.Constants.BuildingMenuGid.RESOURCES;
            Gids[1] = Client.Common.Constants.BuildingMenuGid.STORAGE;
            Gids[2] = Client.Common.Constants.BuildingMenuGid.CIVIL;
            Gids[3] = Client.Common.Constants.BuildingMenuGid.BUILDINGPLACEHOLDER;
            Gids[4] = Client.Common.Constants.BuildingMenuGid.CANCEL;

            var surroundedPos = LogicRules.GetSurroundedFields(m_center);
            for (var index = 0; index < surroundedPos.Length; ++index)
            {
                var pos = surroundedPos[index];
                var gid = new CCTileGidAndFlags(Gids[index]);
                m_sprites.Add(pos, m_worldLayer.GetRegionViewHex(pos.RegionPosition).SetMenuTile(pos, gid));
                m_baseMenuPositions[pos] = gid;
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
            switch (gid)
            {
                case Common.Constants.BuildingMenuGid.MILITARY:
                    types = new Core.Models.Definitions.Definition[4];
                    types[0] = defM.GetDefinition(EntityType.Barracks);
                    types[1] = defM.GetDefinition(EntityType.Attachment);
                    types[2] = defM.GetDefinition(EntityType.Factory);
                    types[3] = defM.GetDefinition(EntityType.GuardTower);
                    break;
                case Common.Constants.BuildingMenuGid.CIVIL:
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
            GetExtendedCoords(coord, types);
            DrawMenu(MenuType.Extended);
        }

        /// <summary>
        /// Set the extended menu.
        /// </summary>
        /// <param name="types">The definition Types.</param>
        private void SetExtendedMenu(MenuType menuType)
        {
            foreach (var pair in m_extendedMenuPositions)
            {
                var pos = pair.Key;
                if (pair.Value != null)
                {
                    var gid = ViewDefinitions.Instance.DefinitionToTileGid(pair.Value, ViewDefinitions.Sort.Menu);
                    m_sprites.Add(pos, m_worldLayer.GetRegionViewHex(pos.RegionPosition).SetMenuTile(pos, gid, IsPossibleToCreate(m_center, pair.Value)));
                }
            }
        }

        /// <summary>
        /// Draws the menu for each type.
        /// </summary>
        /// <param name="menuType">Menu type.</param>
        public void DrawMenu(MenuType menuType)
        {
            switch (menuType)
            {
                case MenuType.Headquarter:
                    this.SetMenu(menuType);
                    break;
                case MenuType.Extended:
                    this.SetExtendedMenu(menuType);
                    break;
                case MenuType.Major:
                    SetMajorMenu(menuType);
                    break;
                case MenuType.Unity:
                    this.SetMenu(menuType);
                    break;
            }
        }

        /// <summary>
        /// Sets the menu.
        /// </summary>
        /// <param name="menuType">Menu type.</param>
        private void SetMenu(MenuType menuType)
        {
            var surroundedCoords = LogicRules.GetSurroundedFields(m_center);
            for (var index = 0; index < surroundedCoords.Length; ++index)
            {
                var pos = surroundedCoords[index];
                var gid = ViewDefinitions.Instance.DefinitionToTileGid(m_types[index], ViewDefinitions.Sort.Menu);
                m_sprites.Add(pos, m_worldLayer.GetRegionViewHex(pos.RegionPosition).SetMenuTile(pos, gid, IsPossibleToCreate(m_center, m_types[index])));
                m_extendedMenuPositions[pos] = m_types[index];
            }
        }

        /// <summary>
        /// Gets the selected definition.
        /// </summary>
        /// <returns>The selected definition.</returns>
        /// <param name="coord">Coordinate which was selected.</param>
        public Core.Models.Definitions.Definition GetSelectedDefinition(PositionI pos)
        {
            Core.Models.Definitions.Definition def = null;
            m_extendedMenuPositions.TryGetValue(pos, out def);
            return def;
        }

        /// <summary>
        /// Gets the selected definition.
        /// </summary>
        /// <returns>The selected definition.</returns>
        /// <param name="coord">Coordinate which was selected.</param>
        public CCTileGidAndFlags GetSelectedKategory(PositionI pos)
        {
            CCTileGidAndFlags gid = CCTileGidAndFlags.EmptyTile;
            m_baseMenuPositions.TryGetValue(pos, out gid);
            return gid;
        }

        /// <summary>
        /// Clears the extended menu Tiles.
        /// </summary>
        public void ClearExtendedMenu()
        {
            foreach (var coord in m_extendedMenuPositions)
            {
                CCSprite sprite;
                if (m_sprites.TryGetValue(coord.Key, out sprite))
                {
                    sprite.Parent.RemoveChild(sprite);
                    m_sprites.Remove(coord.Key);
                }
            }
            m_extendedMenuPositions.Clear();
        }

        /// <summary>
        /// Closes the menu.
        /// </summary>
        public void CloseMenu()
        {
            RemoveAllMenu();
            if (m_extendedMenuPositions.Count != 0)
            {
                ClearExtendedMenu();
            }
        }

        /// <summary>
        /// Removes the Menu.
        /// </summary>
        public void RemoveAllMenu()
        {     
            foreach (var sprite in m_sprites)
            {
                sprite.Value.Parent.RemoveChild(sprite.Value);
            }
            m_sprites.Clear();
        }

        /// <summary>
        /// Determines whether this instance is extended.
        /// </summary>
        /// <returns><c>true</c> if this instance is extended; otherwise, <c>false</c>.</returns>
        public bool IsExtended()
        {
            return m_types.Length == 0;
        }

        /// <summary>
        /// Gets the center position.
        /// </summary>
        /// <returns>The center position.</returns>
        public PositionI GetCenterPosition()
        {
            return new PositionI(m_center.X, m_center.Y);
        }

        /// <summary>
        /// Determines whether this instance is possible to create the specified type on the position.
        /// </summary>
        /// <returns><c>true</c> if this instance is possible to create the specified type on the position; otherwise, <c>false</c>.</returns>
        /// <param name="positionI">Position i.</param>
        /// <param name="type">Type.</param>
        private bool IsPossibleToCreate(PositionI positionI, Core.Models.Definitions.Definition type)
        {
            var action = ActionHelper.CreateEntity(positionI, type, GameAppDelegate.Account);
            var actionC = (Core.Controllers.Actions.Action)action.Control;
            return actionC.Possible();
        }

        /// <summary>
        /// The Center Position of the current menu.
        /// </summary>
        private PositionI m_center;

        /// <summary>
        /// The definition types.
        /// </summary>
        private Core.Models.Definitions.Definition[] m_types;

        /// <summary>
        /// The world layer.
        /// </summary>
        private WorldLayerHex m_worldLayer;

        /// <summary>
        /// The sprites.
        /// </summary>
        private Dictionary<PositionI, CCSprite> m_sprites;

        /// <summary>
        /// A list to hold all the additional Tile Positions.
        /// </summary>
        private Dictionary<PositionI, Core.Models.Definitions.Definition> m_extendedMenuPositions;

        /// <summary>
        /// The base menu positions.
        /// </summary>
        private Dictionary<PositionI, CCTileGidAndFlags> m_baseMenuPositions;

    }
}