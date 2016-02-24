namespace Client.Common.Constants
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
        /// The unit folder.
        /// </summary>
        public const string UNITS = "units";

        /// <summary>
        /// The scrap PNG.
        /// </summary>
        public const string SCRAP = "Scrap";

        /// <summary>
        /// The plutonium PNG.
        /// </summary>
        public const string PLUTONIUM = "Plutonium";

        /// <summary>
        /// The population PNG.
        /// </summary>
        public const string POPULATION = "Population";

        /// <summary>
        /// The technology PNG.
        /// </summary>
        public const string TECHNOLOGY = "Tech";

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
        /// the server which contains world data
        /// </summary>
        public const string DATA_SERVER = "http://derfalke.no-ip.biz";

        /// <summary>
        /// The path to the game world data.
        /// </summary>
        public const string ASCENDANCY_WORLD_PATH = "/ascendancy-world/world/";

        /// <summary>
        /// path to region file
        /// </summary>
        public const string REGION_SERVER_PATH = DATA_SERVER + ASCENDANCY_WORLD_PATH + "$MajorRegionX/$MajorRegionY/germany-$MinorRegionX-$MinorRegionY.json";

        /// <summary>
        /// path to unit definitions
        /// </summary>
        public const string UNIT_TYPES_SERVER_PATH = DATA_SERVER + "/unit.json";

        /// <summary>
        /// path to terrain definitions
        /// </summary>
        public const string TERRAIN_TYPES_SERVER_PATH = DATA_SERVER + "/terrain.json";

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
        /// Tile Map file hex name
        /// </summary>
        public const string TILEMAP_FILE_HEX = "Worldmap-2016222.tmx";

        /// <summary>
        /// name of terrain layer
        /// </summary>
        public const string LAYER_TERRAIN = "Layer 0";

        /// <summary>
        /// name of the indicator layer
        /// </summary>
        public const string LAYER_INDICATOR = "Layer 1";

        /// <summary>
        /// name of building layer
        /// </summary>
        public const string LAYER_BUILDING = "Layer 2";

        /// <summary>
        /// unit of menu layer
        /// </summary>
        public const string LAYER_MENU = "Layer 4";

        /// <summary>
        /// The Start of the Terrain GIDs
        /// </summary>
        public const short TERRAIN_GIDS = 0;

        /// <summary>
        /// The Start of the Building GIDs
        /// </summary>
        public const short BUILDING_GIDS = 20;

        /// <summary>
        /// The Start of the Menu GIDs
        /// </summary>
        public const short MENU_GIDS = 60;

        /// <summary>
        /// The Start of the Enemy GIDs
        /// </summary>
        public const short ENEMY_GIDS = 100;

        /// <summary>
        /// The Start of the Helper GIDs
        /// </summary>
        public const short HELPER_GIDS = 120;

        /// <summary>
        /// size of a tile (width)
        /// </summary>
        public const float TILE_IMAGE_WIDTH = 83.0f;

        /// <summary>
        /// size of a hex tile (width)
        /// </summary>
        public const float TILE_HEX_IMAGE_WIDTH = 84.0f;

        /// <summary>
        /// size of a hex tile (height)
        /// </summary>
        public const float TILE_HEX_IMAGE_HEIGHT = 72.0f;

        /// <summary>
        /// size of the tile map (width)
        /// </summary>
        public const int TILEMAP_WIDTH = 80;

        /// <summary>
        /// size of the hex tile map (width)
        /// </summary>
        public const int TILEMAP_HEX_WIDTH = 32;

        /// <summary>
        /// size of the tile map (height)
        /// </summary>
        public const int TILEMAP_HEIGHT = 320;

        /// <summary>
        /// size of the hex tile map (height)
        /// </summary>
        public const int TILEMAP_HEX_HEIGHT = 32;

        /// <summary>
        /// minimum scaling of tile map
        /// </summary>
        public const float TILEMAP_MIN_ZOOM = 0.2f;

        /// <summary>
        /// default scaling of tile map
        /// </summary>
        public const float TILEMAP_NORM_ZOOM = 2.0f;

        /// <summary>
        /// maximum scaling of tile map
        /// </summary>
        public const float TILEMAP_MAX_ZOOM = 8.0f;

        /// <summary>
        /// The height of the tile map hex content size.
        /// </summary>
        public const float TILEMAP_HEX_CONTENTSIZE_HEIGHT = 2304;

        /// <summary>
        /// The width of the tile map hex content size.
        /// </summary>
        public const float TILEMAP_HEX_CONTENTSIZE_WIDTH = 1998;

        /// <summary>
        /// The thickness of the territory building border.
        /// </summary>
        public const float TERRATORRY_BUILDING_BORDER_SIZE = 10.0f;

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

        /// <summary>
        /// The Debug Server Address.
        /// </summary>
        public static readonly string DEBUG_TCP_SERVER = "192.168.2.5";

        /// <summary>
        /// The Server Port
        /// </summary>
        public static readonly int TCP_PORT = 13000;

        /// <summary>
        /// The GPS position timeout.
        /// </summary>
        public static readonly int GPS_GET_POSITION_TIMEOUT = 10000;

        /// <summary>
        /// time in which period the data should be refreshed
        /// </summary>
        public static readonly int DATA_REFRESH_TIME = 10;
    }
}
