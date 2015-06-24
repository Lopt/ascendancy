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
        public const string TERRAIN_TYPES_SERVER_PATH = "http://derfalke.no-ip.biz/terrain.json";
        public const string LOGIC_SERVER = "http://derfalke.no-ip.biz:9000";
        public const string LOGIN_PATH = "/Login?json=$JSON";
        public const string LOAD_REGIONS_PATH = "/LoadRegions?json=$JSON";
        public const string DO_ACTIONS_PATH = "/DoActions?json=$JSON";
        public const string LOGIC_SERVER_JSON = "$JSON";

        #region TileMap

        public const int CELLMAP_160x160_SIZE = 160;

        public const string TILEMAP_FILE = "Worldmap-160x160(320x80)";
        public const string LAYER_TERRAIN = "Layer 0";
        public const string LAYER_BUILDING = "Layer 1";
        public const string LAYER_UNIT = "Layer 2";
        public const string LAYER_MENU = "Layer 3";


        public const float TILE_IMAGE_WIDTH = 83.0f;

        public const int TILEMAP_WIDTH = 80;
        public const int TILEMAP_HIGH = 320;

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

        public const short Bogenschütze = 46;
        public const short Krieger = 47;
        public const short Magier = 48;
        public const short Späher = 49;
        public const short unbekanteUnit = 50;

        #endregion

        #region MenueUnitsGid

        public const short MenueBogenschütze = 59;
        public const short MenueKrieger = 60;
        public const short MenueMagier = 61;
        public const short MenueSpäher = 62;
        public const short MenueEinheitenPlatzhalter = 63;


        #endregion

        #region BuildingsGid

        public const short Farm = 12;
        public const short Garnision = 13;
        public const short Hauptgebäude = 14;
        public const short Haus = 15;
        public const short Turm1 = 16;
        public const short Turm2 = 17;
        public const short Turm3 = 18;
        public const short Turm4 = 19;
        public const short Turm5 = 20;
        public const short Turm6 = 21;
        public const short Mauer1 = 22;
        public const short Mauer2 = 23;
        public const short Mauer3 = 24;
        public const short Mauer4 = 25;
        public const short Mauer5 = 26;
        public const short Mauer6 = 27;
        public const short KaserneErde = 34;
        public const short KaserneFeuer = 35;
        public const short KaserneGold = 36;
        public const short KaserneLuft = 37;
        public const short KaserneMagie = 38;
        public const short KaserneWasser = 39;
        public const short MineErde = 40;
        public const short MineFeuer = 41;
        public const short MineGold = 42;
        public const short MineLuft = 43;
        public const short MineMana = 44;
        public const short MineWasser = 45;

        #endregion

        #region MenueBuildingsGid

        public const short AuswahlRahmen = 51;
        public const short MenueErde = 52;
        public const short MenueFeuer = 53;
        public const short MenueGold = 54;
        public const short MenueLuft = 55;
        public const short MenueMana = 56;
        public const short MenueWasser = 57;
        public const short MenueGebäudePlatzhalter = 58;

        #endregion

    }
}
