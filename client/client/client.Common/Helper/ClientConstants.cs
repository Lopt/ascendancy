using System;
using CocosSharp;

namespace client.Common.Helper
{
    public class ClientConstants
    {
        public ClientConstants ()
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
			   
        public const string REGION_SERVER_PATH = "http://derfalke.no-ip.biz/world/$MajorRegionX/$MajorRegionY/germany-$MinorRegionX-$MinorRegionY.json";


        public const string ENTITY_TYPES_SERVER_PATH = "http://derfalke.no-ip.biz/unit.json";
        public const string TERRAIN_TYPES_SERVER_PATH = "http://derfalke.no-ip.biz/terrain.json";
        public const string LOGIC_SERVER = "http://derfalke.no-ip.biz:9000";
        public const string LOGIN_PATH = "/Login?json=$JSON";
        public const string LOAD_REGIONS_PATH = "/LoadRegions?json=$JSON";
        public const string DO_ACTIONS_PATH = "/DoActions?json=$JSON";
        public const string LOGIC_SERVER_JSON = "$JSON";

        #region TileMap


        public const int CELLMAP_160x160_SIZE = 160;

        public const string TILEMAP_FILE = "Worldmap-160x160(80x320)_20150601";
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

        #endregion

        #region TerainsGid

        // became Numbers from client/data/tiles/Tile_zuordnung.ods
        public const short WATER_GID = 10;
        public const short BUILDINGS_GID = 7;
        public const short WOODS_GID = 9;
        public const short GRASSLAND_GID = 5;
        public const short FIELDS_GID = 3;
        public const short STREETS_GID = 8;
        public const short NOTDEFINED_GID = 1;
        public const short FORBIDDEN_GID = 1;
        public const short TOWN_GID = 6;
        public const short GLACIER_GID = 4;
        public const short BEACH_GID = 2;
        public const short PARK_GID = 11;
        public const short INVALID_GID = 1;

        #endregion

        #region UnitsGid

        public const short BOWMAN_GID  = 46;
        public const short HERO_GID          = 47;
        public const short WARRIOR_GID       = 48;
        public const short MAGE_GID        = 49;
        public const short SCOUT_GID        = 50;
        public const short UNKNOWN_GID = 51;

        #endregion

        #region MenueUnitsGid

        public const short MENUEBOWMAN_GID = 59;
        public const short MENUEWARRIOR_GID = 60;
        public const short MENUEMAGE_GID = 61;
        public const short MENUESCOUT_GID = 62;
        public const short MENUEUNKNOWN_GID = 63;


        #endregion

        #region BuildingsGid

        public const short FARM_GID = 12;
        public const short GARNISION_GID = 13;
        public const short HEADQUARTER_GID = 14;
        public const short HOUSE_GID = 15;
        public const short TOWER1_GID = 16;
        public const short TOWER2_GID = 17;
        public const short TOWER3_GID = 18;
        public const short TOWER4_GID = 19;
        public const short TOWER5_GID = 20;
        public const short TOWER6_GID = 21;
        public const short WALL1_GID = 22;
        public const short WALL2_GID = 23;
        public const short WALL3_GID = 24;
        public const short WALL4_GID = 25;
        public const short WALL5_GID = 26;
        public const short WALL6_GID = 27;
        public const short BARRACKSEARTH_GID = 34;
        public const short BARRACKSFIRE_GID = 35;
        public const short BARRACKSGOLD_GID = 36;
        public const short BARRACKSAIR_GID = 37;
        public const short BARRACKSMAGIC_GID = 38;
        public const short BARRACKSWATER_GID = 39;
        public const short MINEEARTH_GID = 40;
        public const short MINEFIRE_GID = 41;
        public const short MINEGOLD_GID = 42;
        public const short MINEAIR_GID = 43;
        public const short MINEMANA_GID = 44;
        public const short MINEWATER_GID = 45;

        #endregion

        #region MenueBuildingsGid

        public const short MENUEEARTH_GID = 52;
        public const short MENUEFIRE_GID = 53;
        public const short MENUEGOLD_GID = 54;
        public const short MENUEAIR_GID = 55;
        public const short MENUEMANA_GID = 56;
        public const short MENUEWATER_GID = 57;
        public const short MENUEBUILDINGPLACEHOLDER_GID = 58;

        #endregion

        #region HelperGid

        public const short DOT_GID = 64;
        public const short CROSS_GID = 65;

        #endregion


        #region Action-Animations
        public const float MOVE_SPEED_PER_FIELD = 3f;

        #endregion

    }
}
