using System;

namespace Client.Common.Helper
{
    /// <summary>
    /// Client constants.
    /// </summary>
    public class ClientConstants
    {
        public ClientConstants()
        {
        }

        public const string CONTENT = "Content";
        public const string ANIMATIONS = "animations";
        public const string FONTS = "fonts";
        public const string SOUNDS = "sounds";
        public const string TILES = "tiles";
        public const string IMAGES = "images";
        public const string IMAGES_HD = "images/hd";
        public const string IMAGES_LD = "images/ld";
			   
        public const string LOGIC_SERVER = "http://derfalke.no-ip.biz:9000";
        public const string DATA_SERVER = "http://derfalke.no-ip.biz";

        public const string REGION_SERVER_PATH = DATA_SERVER + "/world/$MajorRegionX/$MajorRegionY/germany-$MinorRegionX-$MinorRegionY.json";


        public const string UNIT_TYPES_SERVER_PATH = DATA_SERVER + "/unit.json";
        public const string TERRAIN_TYPES_SERVER_PATH = DATA_SERVER + "/terrain.json";
        public const string LOGIN_PATH = LOGIC_SERVER + "/Login?json=$JSON";
        public const string LOAD_REGIONS_PATH = LOGIC_SERVER + "/LoadRegions?json=$JSON";
        public const string DO_ACTIONS_PATH = LOGIC_SERVER + "/DoActions?json=$JSON";
        public const string LOGIC_SERVER_JSON = "$JSON";

        #region TileMap


        public const int CELLMAP_160x160_SIZE = 160;

        public const string TILEMAP_FILE = "Worldmap-160x160(80x320)_20150704";
        public const string LAYER_TERRAIN = "Layer 0";
        public const string LAYER_BUILDING = "Layer 1";
        public const string LAYER_UNIT = "Layer 2";
        public const string LAYER_MENU = "Layer 3";

        public const float TILE_IMAGE_WIDTH = 83.0f;

        public const int TILEMAP_WIDTH = 80;
        public const int TILEMAP_HIGH = 320;

        public const float TILEMAP_MIN_SCALE = 0.3f;
        public const float TILEMAP_NORM_SCALE = 0.5f;
        public const float TILEMAP_MAX_SCALE = 3.0f;

        public const short DRAW_REGIONS_X = 5;
        public const short DRAW_REGIONS_Y = 5;

        public const double REDRAW_REGIONS_START_X = 1.5 * Core.Models.Constants.REGION_SIZE_X;
        public const double REDRAW_REGIONS_END_X = 3.5 * Core.Models.Constants.REGION_SIZE_X;
        public const double REDRAW_REGIONS_START_Y = 1.5 * Core.Models.Constants.REGION_SIZE_Y;
        public const double REDRAW_REGIONS_END_Y = 3.5 * Core.Models.Constants.REGION_SIZE_Y;


        #endregion

        #region Action-Animations

        public const float MOVE_SPEED_PER_FIELD = 0.50f;

        #endregion

    }
}
