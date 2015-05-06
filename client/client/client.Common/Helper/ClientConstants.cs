using System;
using CocosSharp;

namespace client.Common.helper
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
		public const string IMAGES_HD = "images/hd";
		public const string IMAGES_LD = "images/ld";
			   
		public const string REGION_SERVER_PATH = "http://derfalke.no-ip.biz/world/$MajorRegionX/$MajorRegionY/germany-$MinorRegionX-$MinorRegionY.json";
		public const string TERRAIN_TYPES_SERVER_PATH = "http://derfalke.no-ip.biz/terrain.json";
        
		// became Numbers from client/data/tiles/Tile_zuordnung.ods
		public const short WATER_GID = 9;
		public const short BUILDINGS_GID = 94;
		public const short WOODS_GID = 41;
		public const short GRASSLAND_GID = 46;
		public const short FIELDS_GID = 29;
		public const short STREETS_GID = 68;
		public const short NOTDEFINED_GID = 55;
		public const short FORBIDDEN_GID = 55;
		public const short TOWN_GID = 22;
		public const short GLACIER_GID = 55;
		public const short BEACH_GID = 1;
		public const short PARK_GID = 46;
		public const short INVALID_GID = 55;
	}

}

