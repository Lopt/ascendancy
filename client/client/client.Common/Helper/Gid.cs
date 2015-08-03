using System;

namespace Client.Common.Helper
{
    /// <summary>
    /// Terrain GIDs assingment accordngly to client/data/tiles/Tile zuordnung.pdf.
    /// </summary>
    public class TerrainGid
    {
        public const short WATER = 10;
        public const short BUILDINGS = 7;
        public const short WOODS = 9;
        public const short GRASSLAND = 5;
        public const short FIELDS = 3;
        public const short STREETS = 8;
        public const short NOTDEFINED = 1;
        public const short FORBIDDEN = 1;
        public const short TOWN = 6;
        public const short GLACIER = 4;
        public const short BEACH = 2;
        public const short PARK = 11;
        public const short INVALID = 1;
    }

    /// <summary>
    /// Building GIDs assingment accordngly to client/data/tiles/Tile zuordnung.pdf.
    /// </summary>
    public class BuildingGid
    {
        public const short FARM = 12;
        public const short GARNISION = 13;
        public const short HEADQUARTER = 14;
        public const short HOUSE = 15;
        public const short TOWER1 = 16;
        public const short TOWER2 = 17;
        public const short TOWER3 = 18;
        public const short TOWER4 = 19;
        public const short TOWER5 = 20;
        public const short TOWER6 = 21;
        public const short WALL1 = 22;
        public const short WALL2 = 23;
        public const short WALL3 = 24;
        public const short WALL4 = 25;
        public const short WALL5 = 26;
        public const short WALL6 = 27;
        public const short BARRACKSEARTH = 34;
        public const short BARRACKSFIRE = 35;
        public const short BARRACKSGOLD = 36;
        public const short BARRACKSAIR = 37;
        public const short BARRACKSMAGIC = 38;
        public const short BARRACKSWATER = 39;
        public const short MINEEARTH = 40;
        public const short MINEFIRE = 41;
        public const short MINEGOLD = 42;
        public const short MINEAIR = 43;
        public const short MINEMANA = 44;
        public const short MINEWATER = 45;
    }

    /// <summary>
    /// Unit GIDs assingment accordngly to client/data/tiles/Tile zuordnung.pdf.
    /// </summary>
    public class UnitGid
    {
        public const short BOWMAN = 46;
        public const short HERO = 47;
        public const short WARRIOR = 48;
        public const short MAGE = 49;
        public const short SCOUT = 50;
        public const short UNKNOWN = 51;
    }

    /// <summary>
    /// Enemy Unit GIDs assingment accordngly to client/data/tiles/Tile zuordnung.pdf.
    /// </summary>
    public class EnemyUnitGid
    {
        public const short BOWMAN = 68;
        public const short HERO = 69;
        public const short WARRIOR = 70;
        public const short MAGE = 71;
        public const short SCOUT = 72;
    }

    /// <summary>
    /// Enemy Building GIDs assingment accordngly to client/data/tiles/Tile zuordnung.pdf.
    /// </summary>
    public class EnemyBuildingGid
    {
        public const short HEADQUARTER = 73;
    }

    /// <summary>
    /// Building Menu GID assingment accordngly to client/data/tiles/Tile zuordnung.pdf.
    /// </summary>
    public class BuildingMenuGid
    {
        public const short EARTH = 52;
        public const short FIRE = 53;
        public const short GOLD = 54;
        public const short AIR = 55;
        public const short MANA = 56;
        public const short WATER = 57;
        public const short BUILDINGPLACEHOLDER = 58;
        public const short HEADQUARTER = 59;
    }

    /// <summary>
    /// Unit Menu GIDs assingment accordngly to client/data/tiles/Tile zuordnung.pdf.
    /// </summary>
    public class UnitMenuGid
    {
        public const short BOWMAN = 60;
        public const short HERO = 61;
        public const short WARRIOR = 62;
        public const short MAGE = 63;
        public const short SCOUT = 64;
        public const short UNKNOWN = 65;
    }

    /// <summary>
    /// Helper sprites GIDs assingment accordngly to client/data/tiles/Tile zuordnung.pdf.
    /// </summary>
    public class HelperSpritesGid
    {
        public const short DOT = 66;
        public const short CROSS = 67;
    }
}

