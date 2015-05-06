using System;

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
		public const string IMAGES_HD = "images/hd";
		public const string IMAGES_LD = "images/ld";
		public const string IMAGES_TERRAIN = "images/terrain";
			   
		public const string REGION_SERVER_PATH = "http://derfalke.no-ip.biz/world/$MajorRegionX/$MajorRegionY/germany-$MinorRegionX-$MinorRegionY.json";
		public const string TERRAIN_TYPES_SERVER_PATH = "http://derfalke.no-ip.biz/terrain.json";
        
		// TODO set correct numbers from tiles.png
		public const short WATER_GID = 0;
		public const short BUILDINGS_GID = 1;
		public const short WOODS_GID = 2;
		public const short GRASSLAND_GID = 3;
		public const short FIELDS_GID = 4;
		public const short STREETS_GID = 5;
		public const short NOTDEFINED_GID = 6;
		public const short FORBIDDEN_GID = 7;
		public const short TOWN_GID = 8;
		public const short GLACIER_GID = 9;
		public const short BEACH_GID = 10;
		public const short PARK_GID = 11;
		public const short INVALID_GID = 12;
	}

}

