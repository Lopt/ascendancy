namespace Client.Common.Constants
{
    /// <summary>
    /// Terrain GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class TerrainGid
    {
        /// <summary>
        /// GID for tiles with the trait water.
        /// </summary>
        public const short WATER = 10;

        /// <summary>
        /// GID for tiles with the trait building.
        /// </summary>
        public const short BUILDINGS = 7;

        /// <summary>
        /// GID for tiles with the trait woods.
        /// </summary>
        public const short WOODS = 9;

        /// <summary>
        /// GID for tiles with the trait grassland.
        /// </summary>
        public const short GRASSLAND = 5;

        /// <summary>
        /// GID for tiles with the trait field.
        /// </summary>
        public const short FIELDS = 3;

        /// <summary>
        /// GID for tiles with the trait street.
        /// </summary>
        public const short STREETS = 8;

        /// <summary>
        /// GID for tiles with the trait not defined.
        /// </summary>
        public const short NOTDEFINED = 1;

        /// <summary>
        /// GID for tiles with the trait forbidden.
        /// </summary>
        public const short FORBIDDEN = 1;

        /// <summary>
        /// GID for tiles with the trait town.
        /// </summary>
        public const short TOWN = 6;

        /// <summary>
        /// GID for tiles with the trait glacier.
        /// </summary>
        public const short GLACIER = 4;

        /// <summary>
        /// GID for tiles with the trait beach.
        /// </summary>
        public const short BEACH = 2;

        /// <summary>
        /// GID for tiles with the trait park.
        /// </summary>
        public const short PARK = 11;

        /// <summary>
        /// GID for tiles which are invalid.
        /// </summary>
        public const short INVALID = 1;
    }

    /// <summary>
    /// Building GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class BuildingGid
    {
        /// <summary>
        /// GID for tiles where a building tile with the trait farm is placed.
        /// </summary>
        public const short FARM = 12;

        /// <summary>
        /// GID for tiles where a building tile with the trait garrison is placed.
        /// </summary>
        public const short GARNISION = 13;

        /// <summary>
        /// GID for tiles where a building tile with the trait headquarter is placed.
        /// </summary>
        public const short HEADQUARTER = 14;

        /// <summary>
        /// GID for tiles where a building tile with the trait house is placed.
        /// </summary>
        public const short HOUSE = 15;

        /// <summary>
        /// GID for tiles where a building tile with the trait tower1 is placed.
        /// </summary>
        public const short TOWER1 = 16;

        /// <summary>
        /// GID for tiles where a building tile with the trait tower2 is placed.
        /// </summary>
        public const short TOWER2 = 17;

        /// <summary>
        /// GID for tiles where a building tile with the trait tower3 is placed.
        /// </summary>
        public const short TOWER3 = 18;

        /// <summary>
        /// GID for tiles where a building tile with the trait tower4 is placed.
        /// </summary>
        public const short TOWER4 = 19;

        /// <summary>
        /// GID for tiles where a building tile with the trait tower5 is placed.
        /// </summary>
        public const short TOWER5 = 20;

        /// <summary>
        /// GID for tiles where a building tile with the trait tower6 is placed.
        /// </summary>
        public const short TOWER6 = 21;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall1 is placed.
        /// </summary>
        public const short WALL1 = 22;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall2 is placed.
        /// </summary>
        public const short WALL2 = 23;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall3 is placed.
        /// </summary>
        public const short WALL3 = 24;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall4 is placed.
        /// </summary>
        public const short WALL4 = 25;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall5 is placed.
        /// </summary>
        public const short WALL5 = 26;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall6 is placed.
        /// </summary>
        public const short WALL6 = 27;

        /// <summary>
        /// GID for tiles where a building tile with the trait Barrack which use earth as a resource is placed.
        /// </summary>
        public const short BARRACKSEARTH = 34;

        /// <summary>
        /// GID for tiles where a building tile with trait Barrack which use fire as a resource is placed.
        /// </summary>
        public const short BARRACKSFIRE = 35;

        /// <summary>
        /// GID for tiles where a building tile with the trait Barrack which use gold as a resource is placed.
        /// </summary>
        public const short BARRACKSGOLD = 36;

        /// <summary>
        /// GID for tiles where a building tile with the trait Barrack which use air as a resource is placed.
        /// </summary>
        public const short BARRACKSAIR = 37;

        /// <summary>
        /// GID for tiles where a building tile with the trait Barrack which use magic as a resource is placed.
        /// </summary>
        public const short BARRACKSMAGIC = 38;

        /// <summary>
        /// GID for tiles where a building tile with the trait Barrack which use water as resource is placed.
        /// </summary>
        public const short BARRACKSWATER = 39;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce earth as a resource is placed.
        /// </summary>
        public const short MINEEARTH = 40;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce fire as a resource is placed.
        /// </summary>
        public const short MINEFIRE = 41;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce gold as a resource is placed.
        /// </summary>
        public const short MINEGOLD = 42;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce air as a resource is placed.
        /// </summary>
        public const short MINEAIR = 43;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce magic as a resource is placed.
        /// </summary>
        public const short MINEMANA = 44;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce water as a resource is placed.
        /// </summary>
        public const short MINEWATER = 45;
    }

    /// <summary>
    /// Unit GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class UnitGid
    {
        /// <summary>
        /// GID for the unit tile with the trait archer.
        /// </summary>
        public const short BOWMAN = 46;

        /// <summary>
        /// GID for the unit tile with the trait hero.
        /// </summary>
        public const short HERO = 47;

        /// <summary>
        /// GID for the unit tile with the trait warrior.
        /// </summary>
        public const short WARRIOR = 48;

        /// <summary>
        /// GID for the unit tile with the trait mage.
        /// </summary>
        public const short MAGE = 49;

        /// <summary>
        /// GID for the unit tile with the trait scout.
        /// </summary>
        public const short SCOUT = 50;

        /// <summary>
        /// GID for the unit tile with the trait unknown.
        /// </summary>
        public const short UNKNOWN = 51;
    }

    /// <summary>
    /// Enemy Unit GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class EnemyUnitGid
    {
        /// <summary>
        /// GID for the enemy unit tile with the trait archer.
        /// </summary>
        public const short BOWMAN = 68;

        /// <summary>
        /// GID for the enemy unit tile with the trait hero.
        /// </summary>
        public const short HERO = 69;

        /// <summary>
        /// GID for the enemy unit tile with the trait warrior.
        /// </summary>
        public const short WARRIOR = 70;

        /// <summary>
        /// GID for the enemy unit tile with the trait mage.
        /// </summary>
        public const short MAGE = 71;

        /// <summary>
        /// GID for the enemy unit tile with the trait scout.
        /// </summary>
        public const short SCOUT = 72;
    }

    /// <summary>
    /// Enemy Building GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class EnemyBuildingGid
    {
        /// <summary>
        /// GID for the enemy building tile with the trait headquarter.
        /// </summary>
        public const short HEADQUARTER = 73;
    }

    /// <summary>
    /// Building Menu GID assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class BuildingMenuGid
    {
        /// <summary>
        /// GID for the building menu tile with the trait earth.
        /// </summary>
        public const short EARTH = 52;

        /// <summary>
        /// GID for the building menu tile with the trait fire.
        /// </summary>
        public const short FIRE = 53;

        /// <summary>
        /// GID for the building menu tile with the trait gold.
        /// </summary>
        public const short GOLD = 54;

        /// <summary>
        /// GID for the building menu tile with the trait air.
        /// </summary>
        public const short AIR = 55;

        /// <summary>
        /// GID for the building menu tile with the trait magic.
        /// </summary>
        public const short MANA = 56;

        /// <summary>
        /// GID for the building menu tile with the trait water.
        /// </summary>
        public const short WATER = 57;

        /// <summary>
        /// GID for the building menu tile with the trait placeholder.
        /// </summary>
        public const short BUILDINGPLACEHOLDER = 58;

        /// <summary>
        /// GID for the building menu tile with the trait headquarter.
        /// </summary>
        public const short HEADQUARTER = 59;
    }

    /// <summary>
    /// Unit Menu GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class UnitMenuGid
    {
        /// <summary>
        /// GID for the unit menu tiles with the trait archer.
        /// </summary>
        public const short BOWMAN = 60;

        /// <summary>
        /// GID for the unit menu tiles with the trait hero.
        /// </summary>
        public const short HERO = 61;

        /// <summary>
        /// GID for the unit menu tiles with the trait warrior.
        /// </summary>
        public const short WARRIOR = 62;

        /// <summary>
        /// GID for the unit menu tiles with the trait mage.
        /// </summary>
        public const short MAGE = 63;

        /// <summary>
        /// GID for the unit menu tiles with the trait scout.
        /// </summary>
        public const short SCOUT = 64;

        /// <summary>
        /// GID for the unit menu tiles with the trait unknown.
        /// </summary>
        public const short UNKNOWN = 65;
    }

    /// <summary>
    /// Helper sprites GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class HelperSpritesGid
    {
        /// <summary>
        /// GID for the tile with the trait dot.
        /// </summary>
        public const short DOT = 66;

        /// <summary>
        /// GID for the tile with the trait cross.
        /// </summary>
        public const short CROSS = 67;
    }
}