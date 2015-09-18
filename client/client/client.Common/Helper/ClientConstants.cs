﻿namespace Client.Common.Helper
{
    using System;

    /// <summary>
    /// Client constants.
    /// </summary>
    public class ClientConstants
    {
        /// <summary>
        /// The content folder
        /// </summary>
        public const string CONTENT = "Content";

        /// <summary>
        /// The animations folder.
        /// </summary>
        public const string ANIMATIONS = "animations";

        /// <summary>
        /// The fonts folder.
        /// </summary>
        public const string FONTS = "fonts";

        /// <summary>
        /// The sounds folder.
        /// </summary>
        public const string SOUNDS = "sounds";

        /// <summary>
        /// The tile images folder.
        /// </summary>
        public const string TILES = "tiles";

        /// <summary>
        /// The IMAGE.
        /// </summary>
        public const string IMAGES = "images";

        /// <summary>
        /// The images folder in high definition
        /// </summary>
        public const string IMAGES_HD = "images/hd";

        /// <summary>
        /// The images folder in low definition
        /// </summary>
        public const string IMAGES_LD = "images/ld";
  
        /// <summary>
        /// the server which contains game logic
        /// </summary>
        public const string LOGIC_SERVER = "http://derfalke.no-ip.biz:9000";

        /// <summary>
        /// the server which contains world data
        /// </summary>
        public const string DATA_SERVER = "http://derfalke.no-ip.biz";

        /// <summary>
        /// path to region file
        /// </summary>
        public const string REGION_SERVER_PATH = DATA_SERVER + "/world/$MajorRegionX/$MajorRegionY/germany-$MinorRegionX-$MinorRegionY.json";

        /// <summary>
        /// path to unit definitions
        /// </summary>
        public const string UNIT_TYPES_SERVER_PATH = DATA_SERVER + "/unit.json";

        /// <summary>
        /// path to terrain definitions
        /// </summary>
        public const string TERRAIN_TYPES_SERVER_PATH = DATA_SERVER + "/terrain.json";

        /// <summary>
        /// login path
        /// </summary>
        public const string LOGIN_PATH = LOGIC_SERVER + "/Login?json=$JSON";

        /// <summary>
        /// load regions path
        /// </summary>
        public const string LOAD_REGIONS_PATH = LOGIC_SERVER + "/LoadRegions?json=$JSON";

        /// <summary>
        /// do actions path
        /// </summary>
        public const string DO_ACTIONS_PATH = LOGIC_SERVER + "/DoActions?json=$JSON";

        /// <summary>
        /// what must be replaced 
        /// </summary>
        public const string LOGIC_SERVER_JSON = "$JSON";

        #region TileMap

        /// <summary>
        /// Tile Map size
        /// </summary>
        public const int CELLMAP_160x160_SIZE = 160;

        /// <summary>
        /// Tile Map file name
        /// </summary>
        public const string TILEMAP_FILE = "Worldmap-160x160(80x320)_20150704";

        /// <summary>
        /// name of terrain layer
        /// </summary>
        public const string LAYER_TERRAIN = "Layer 0";

        /// <summary>
        /// name of building layer
        /// </summary>
        public const string LAYER_BUILDING = "Layer 1";

        /// <summary>
        /// name of unit layer
        /// </summary>
        public const string LAYER_UNIT = "Layer 2";

        /// <summary>
        /// unit of menu layer
        /// </summary>
        public const string LAYER_MENU = "Layer 3";

        /// <summary>
        /// size of a tile (width)
        /// </summary>
        public const float TILE_IMAGE_WIDTH = 83.0f;

        /// <summary>
        /// size of the tile map (width)
        /// </summary>
        public const int TILEMAP_WIDTH = 80;

        /// <summary>
        /// size of the tile map (height)
        /// </summary>
        public const int TILEMAP_HEIGHT = 320;

        /// <summary>
        /// minimum scaling of tile map
        /// </summary>
        public const float TILEMAP_MIN_SCALE = 0.3f;

        /// <summary>
        /// default scaling of tile map
        /// </summary>
        public const float TILEMAP_NORM_SCALE = 0.5f;

        /// <summary>
        /// maximum scaling of tile map
        /// </summary>
        public const float TILEMAP_MAX_SCALE = 3.0f;

        /// <summary>
        /// number of regions which should be drawn (X)
        /// </summary>
        public const short DRAW_REGIONS_X = 5;

        /// <summary>
        /// number of regions which should be drawn (Y)
        /// </summary>
        public const short DRAW_REGIONS_Y = 5;

        /// <summary>
        /// position where new regions should be drawn (min x)
        /// </summary>
        public const double REDRAW_REGIONS_START_X = 1.5 * Core.Models.Constants.REGION_SIZE_X;

        /// <summary>
        /// position where new regions should be drawn (max y)
        /// </summary>
        public const double REDRAW_REGIONS_END_X = 3.5 * Core.Models.Constants.REGION_SIZE_X;

        /// <summary>
        /// position where new regions should be drawn (min y)
        /// </summary>
        public const double REDRAW_REGIONS_START_Y = 1.5 * Core.Models.Constants.REGION_SIZE_Y;

        /// <summary>
        /// position where new regions should be drawn (max y)
        /// </summary>
        public const double REDRAW_REGIONS_END_Y = 3.5 * Core.Models.Constants.REGION_SIZE_Y;

        #endregion

        #region Action-Animations

        /// <summary>
        /// speed of unit move animation per field
        /// </summary>
        public const float MOVE_SPEED_PER_FIELD = 0.50f;

        #endregion

        /// <summary>
        /// The Server Address (which he should listen).
        /// </summary>
        public static readonly string TCP_SERVER = "derfalke.no-ip.biz";
        public static readonly string DEBUG_TCP_SERVER = "192.168.43.90";

        /// <summary>
        /// The Server Port
        /// </summary>
        public static readonly int TCP_PORT = 13000;
    }
}
