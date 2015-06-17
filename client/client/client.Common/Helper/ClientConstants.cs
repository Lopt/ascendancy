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

        #region TileMap

        public const string TILEMAP_FILE = "Worldmap-160x160(320x80)";
        public const string LAYER_TERRAIN       = "Layer 0";
        //public const string LAYER_BUILDING    = "Layer 1";
        //public const string LAYER_UNIT        = "Layer 2";
        //public const string LAYER_MENU        = "Layer 3";


        public const float TILE_IMAGE_WIDTH = 83.0f;

        public const int TILEMAP_WIDTH = 80;
        public const int TILEMAP_HIGH = 320;

        #endregion

        #region TilesGid

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
    }
}
