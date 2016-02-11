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
        // to be replaced with TERRAIN_GID + 10 etc

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
        /// GID for tiles where the building Factory is placed.
        /// </summary>
        public const short FACTORY = ClientConstants.BUILDING_GIDS + 1;
        // to be replaced with BUILDING_GID + 1 etc

        /// <summary>
        /// GID for tiles where the building Farm is placed.
        /// </summary>
        public const short FARM = ClientConstants.BUILDING_GIDS + 2;

        /// <summary>
        /// GID for tiles where the building Garrison is placed.
        /// </summary>
        public const short GARRISON = ClientConstants.BUILDING_GIDS + 3;

        /// <summary>
        /// GID for tiles where the building Headquarter is placed.
        /// </summary>
        public const short HEADQUARTER = ClientConstants.BUILDING_GIDS + 4;

        /// <summary>
        /// GID for tiles where the building House is placed.
        /// </summary>
        public const short HOUSE = ClientConstants.BUILDING_GIDS + 5;

        /// <summary>
        /// GID for tiles where the building Furnace is placed.
        /// </summary>
        public const short FURNACE = ClientConstants.BUILDING_GIDS + 6;

        /// <summary>
        /// GID for tiles where the building Barracks is placed.
        /// </summary>
        public const short BARRACKS = ClientConstants.BUILDING_GIDS + 7;

        /// <summary>
        /// GID for tiles where the building Public building is placed.
        /// </summary>
        public const short LABORATORY = ClientConstants.BUILDING_GIDS + 8;

        /// <summary>
        /// GID for tiles where the building Tower1 is placed.
        /// </summary>
        public const short TOWER1 = ClientConstants.BUILDING_GIDS + 9;

        /// <summary>
        /// GID for tiles where a building tile with the trait tower4 is placed.
        /// </summary>
        public const short HOSPITAL = ClientConstants.BUILDING_GIDS + 10;

        /// <summary>
        /// GID for tiles where a building tile with the trait Scrapyard is placed.
        /// </summary>
        public const short SCRAPYARD = ClientConstants.BUILDING_GIDS + 11;

        /// <summary>
        /// GID for tiles where a building tile with the trait tower3 is placed.
        /// </summary>
        public const short TRADINGPOST = ClientConstants.BUILDING_GIDS + 12;

        /// <summary>
        /// GID for tiles where a building tile with the trait tower5 is placed.
        /// </summary>
        public const short TRANSFORMER = ClientConstants.BUILDING_GIDS + 13;

        /// <summary>
        /// GID for tiles where a building tile with the trait tower6 is placed.
        /// </summary>
        public const short TOWER6 = ClientConstants.BUILDING_GIDS + 14;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall1 is placed.
        /// </summary>
        public const short WALL1 = ClientConstants.BUILDING_GIDS + 15;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall2 is placed.
        /// </summary>
        public const short WALL2 = ClientConstants.BUILDING_GIDS + 16;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall3 is placed.
        /// </summary>
        public const short WALL3 = ClientConstants.BUILDING_GIDS + 17;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall4 is placed.
        /// </summary>
        public const short WALL4 = ClientConstants.BUILDING_GIDS + 18;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall5 is placed.
        /// </summary>
        public const short WALL5 = ClientConstants.BUILDING_GIDS + 19;

        /// <summary>
        /// GID for tiles where a building tile with the trait wall6 is placed.
        /// </summary>
        public const short WALL6 = ClientConstants.BUILDING_GIDS + 20;

        /// <summary>
        /// GID for tiles where the building Water tower is placed.
        /// </summary>
        public const short WATERTOWER = ClientConstants.BUILDING_GIDS + 21;

        /// <summary>
        /// GID for tiles where the building Tent is placed.
        /// </summary>
        public const short TENT = ClientConstants.BUILDING_GIDS + 22;

        /// <summary>
        /// GID for tiles where a building tile with the trait Barrack which use earth as a resource is placed.
        /// </summary>
        public const short BARRACKSEARTH = ClientConstants.BUILDING_GIDS + 23;

        /// <summary>
        /// GID for tiles where a building tile with trait Barrack which use fire as a resource is placed.
        /// </summary>
        public const short BARRACKSFIRE = ClientConstants.BUILDING_GIDS + 24;

        /// <summary>
        /// GID for tiles where a building tile with the trait Barrack which use gold as a resource is placed.
        /// </summary>
        public const short BARRACKSGOLD = ClientConstants.BUILDING_GIDS + 25;

        /// <summary>
        /// GID for tiles where a building tile with the trait Barrack which use air as a resource is placed.
        /// </summary>
        public const short BARRACKSAIR = ClientConstants.BUILDING_GIDS + 26;

        /// <summary>
        /// GID for tiles where a building tile with the trait Barrack which use magic as a resource is placed.
        /// </summary>
        public const short BARRACKSMAGIC = ClientConstants.BUILDING_GIDS + 27;

        /// <summary>
        /// GID for tiles where a building tile with the trait Barrack which use water as resource is placed.
        /// </summary>
        public const short BARRACKSWATER = ClientConstants.BUILDING_GIDS + 28;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce earth as a resource is placed.
        /// </summary>
        public const short MINEEARTH = ClientConstants.BUILDING_GIDS + 29;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce fire as a resource is placed.
        /// </summary>
        public const short MINEFIRE = ClientConstants.BUILDING_GIDS + 30;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce gold as a resource is placed.
        /// </summary>
        public const short MINEGOLD = ClientConstants.BUILDING_GIDS + 31;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce air as a resource is placed.
        /// </summary>
        public const short MINEAIR = ClientConstants.BUILDING_GIDS + 32;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce magic as a resource is placed.
        /// </summary>
        public const short MINEMANA = ClientConstants.BUILDING_GIDS + 33;

        /// <summary>
        /// GID for tiles where a building tile with the trait mine which produce water as a resource is placed.
        /// </summary>
        public const short MINEWATER = ClientConstants.BUILDING_GIDS + 34;
    }

    /// <summary>
    /// Unit GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class UnitGid
    {
        /// <summary>
        /// GID for the unit tile with the trait soldier.
        /// </summary>
        public const short SOLDIER = ClientConstants.UNIT_GIDS + 1;
        // to be replaced with UNIT_GID + 3

        /// <summary>
        /// GID for the unit tile with the trait hero.
        /// </summary>
        public const short HERO = ClientConstants.UNIT_GIDS + 2;

        /// <summary>
        /// GID for the unit tile with the trait warrior.
        /// </summary>
        public const short WARRIOR = ClientConstants.UNIT_GIDS + 3;

        /// <summary>
        /// GID for the unit tile with the trait mage.
        /// </summary>
        public const short MAGE = ClientConstants.UNIT_GIDS + 4;

        /// <summary>
        /// GID for the unit tile with the trait scout.
        /// </summary>
        public const short SCOUT = ClientConstants.UNIT_GIDS + 5;

        /// <summary>
        /// GID for the unit tile with the trait unknown.
        /// </summary>
        public const short UNKNOWN = ClientConstants.UNIT_GIDS + 6;
    }

    /// <summary>
    /// Enemy Unit GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class EnemyUnitGid
    {
        /// <summary>
        /// GID for the enemy unit tile with the trait archer.
        /// </summary>
        public const short SOLDIER = ClientConstants.ENEMY_GIDS + 1;

        /// <summary>
        /// GID for the enemy unit tile with the trait hero.
        /// </summary>
        public const short HERO = ClientConstants.ENEMY_GIDS + 2;

        /// <summary>
        /// GID for the enemy unit tile with the trait warrior.
        /// </summary>
        public const short WARRIOR = ClientConstants.ENEMY_GIDS + 3;

        /// <summary>
        /// GID for the enemy unit tile with the trait mage.
        /// </summary>
        public const short MAGE = ClientConstants.ENEMY_GIDS + 4;

        /// <summary>
        /// GID for the enemy unit tile with the trait scout.
        /// </summary>
        public const short SCOUT = ClientConstants.ENEMY_GIDS + 5;
    }

    /// <summary>
    /// Enemy Building GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class EnemyBuildingGid
    {
        /// <summary>
        /// GID for the enemy building tile with the trait headquarter.
        /// </summary>
        public const short HEADQUARTER = ClientConstants.ENEMY_GIDS + 6;
        // to be replaced with ENEMY_GID + ENEMYOFFSET + 3

        /// <summary>
        /// GID for tiles where the building Barracks is placed.
        /// </summary>
        public const short BARRACKS = ClientConstants.ENEMY_GIDS + 7;

        /// <summary>
        /// GID for tiles where the building Public building is placed.
        /// </summary>
        public const short LABORATORY = ClientConstants.ENEMY_GIDS + 8;

        /// <summary>
        /// GID for tiles where the building Tower1 is placed.
        /// </summary>
        public const short TOWER1 = ClientConstants.ENEMY_GIDS + 9;

        /// <summary>
        /// GID for tiles where a building tile with the trait EnemyHospital is placed.
        /// </summary>
        public const short HOSPITAL = ClientConstants.ENEMY_GIDS + 10;

        /// <summary>
        /// GID for tiles where a building tile with the trait Scrapyard is placed.
        /// </summary>
        public const short SCRAPYARD = ClientConstants.ENEMY_GIDS + 11;

        /// <summary>
        /// GID for tiles where a building tile with the trait EnemyTradingpost is placed.
        /// </summary>
        public const short TRADINGPOST = ClientConstants.ENEMY_GIDS + 12;

        /// <summary>
        /// GID for tiles where a building tile with the trait EnemyTransformer is placed.
        /// </summary>
        public const short TRANSFORMER = ClientConstants.ENEMY_GIDS + 13;

        /// <summary>
        /// GID for tiles where a building tile with the trait EnemyWatertower is placed.
        /// </summary>
        public const short WATERTOWER = ClientConstants.ENEMY_GIDS + 14;

        /// <summary>
        /// GID for tiles where a building tile with the trait EnemyTent is placed.
        /// </summary>
        public const short TENT = ClientConstants.ENEMY_GIDS + 15;

        /// <summary>
        /// GID for tiles where the building EnemyFurnace is placed.
        /// </summary>
        public const short FURNACE = ClientConstants.ENEMY_GIDS + 16;

        /// <summary>
        /// GID for tiles where the building EnemyFactory is placed.
        /// </summary>
        public const short FACTORY = ClientConstants.ENEMY_GIDS + 17;


    }

    /// <summary>
    /// Building Menu GID assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class BuildingMenuGid
    {
        public const short MILITARY = ClientConstants.MENU_GIDS + 1;
        // to be replaced with MENU_GID + 3

        /// <summary>
        /// The STORAGE GID.
        /// </summary>
        public const short STORAGE = ClientConstants.MENU_GIDS + 2;

        /// <summary>
        /// The ZIVIL GID.
        /// </summary>
        public const short CIVIL = ClientConstants.MENU_GIDS + 3;

        /// <summary>
        /// The RESOURCE GID.
        /// </summary>
        public const short RESOURCES = ClientConstants.MENU_GIDS + 4;

        /// <summary>
        /// The CANCEL GID.
        /// </summary>
        public const short CANCEL = ClientConstants.MENU_GIDS + 5;

        /// <summary>
        /// The UPGRADE GID.
        /// </summary>
        public const short UPGRADE = ClientConstants.MENU_GIDS + 6;

        /// <summary>
        /// GID for the building menu tile with the trait earth.
        /// </summary>
        public const short EARTH = ClientConstants.MENU_GIDS + 7;

        /// <summary>
        /// GID for the building menu tile with the trait fire.
        /// </summary>
        public const short FIRE = ClientConstants.MENU_GIDS + 8;

        /// <summary>
        /// GID for the building menu tile with the trait gold.
        /// </summary>
        public const short GOLD = ClientConstants.MENU_GIDS + 9;

        /// <summary>
        /// GID for the building menu tile with the trait air.
        /// </summary>
        public const short AIR = ClientConstants.MENU_GIDS + 10;

        /// <summary>
        /// GID for the building menu tile with the trait magic.
        /// </summary>
        public const short MANA = ClientConstants.MENU_GIDS + 11;

        /// <summary>
        /// GID for the building menu tile with the trait water.
        /// </summary>
        public const short WATER = ClientConstants.MENU_GIDS + 12;

        /// <summary>
        /// GID for the building menu tile with the trait placeholder.
        /// </summary>
        public const short BUILDINGPLACEHOLDER = ClientConstants.MENU_GIDS + 13;

        /// <summary>
        /// GID for the building menu tile with the trait headquarter.
        /// </summary>
        public const short HEADQUARTER = ClientConstants.MENU_GIDS + 14;

        /// <summary>
        /// The BARRACKS GID.
        /// </summary>
        public const short BARRACKS = ClientConstants.MENU_GIDS + 15;

        /// <summary>
        /// The FACTORY GID.
        /// </summary>
        public const short FACTORY = ClientConstants.MENU_GIDS + 16;

        /// <summary>
        /// The TOWER GID.
        /// </summary>
        public const short TOWER = ClientConstants.MENU_GIDS + 17;

        /// <summary>
        /// The WALL GID.
        /// </summary>
        public const short WALL = ClientConstants.MENU_GIDS + 18;

        /// <summary>
        /// The LABORATORY GID.
        /// </summary>
        public const short LABORATORY = ClientConstants.MENU_GIDS + 19;

        /// <summary>
        /// The HOSPITAL.
        /// </summary>
        public const short HOSPITAL = ClientConstants.MENU_GIDS + 20;

        /// <summary>
        /// The SCRAPYARD GID.
        /// </summary>
        public const short SCRAPYARD = ClientConstants.MENU_GIDS + 21;

        /// <summary>
        /// The TRADINGPOST GID.
        /// </summary>
        public const short TRADINGPOST = ClientConstants.MENU_GIDS + 22;

        /// <summary>
        /// The FURNACE GID.
        /// </summary>
        public const short FURNACE = ClientConstants.MENU_GIDS + 23;

        /// <summary>
        /// The TRANSFORMER GID.
        /// </summary>
        public const short TRANSFORMER = ClientConstants.MENU_GIDS + 24;

        /// <summary>
        /// The TENT GID.
        /// </summary>
        public const short TENT = ClientConstants.MENU_GIDS + 25;
    }

    /// <summary>
    /// Unit Menu GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class UnitMenuGid
    {
        /// <summary>
        /// GID for the unit menu tiles with the trait archer.
        /// </summary>
        public const short SOLDIER = ClientConstants.MENU_GIDS + 26;
        // to be replaced with MENU_GID + UNITOFFSET + 1

        /// <summary>
        /// GID for the unit menu tiles with the trait hero.
        /// </summary>
        public const short HERO = ClientConstants.MENU_GIDS + 27;

        /// <summary>
        /// GID for the unit menu tiles with the trait warrior.
        /// </summary>
        public const short WARRIOR = ClientConstants.MENU_GIDS + 28;

        /// <summary>
        /// GID for the unit menu tiles with the trait mage.
        /// </summary>
        public const short MAGE = ClientConstants.MENU_GIDS + 29;

        /// <summary>
        /// GID for the unit menu tiles with the trait scout.
        /// </summary>
        public const short SCOUT = ClientConstants.MENU_GIDS + 30;

        /// <summary>
        /// GID for the unit menu tiles with the trait unknown.
        /// </summary>
        public const short UNKNOWN = ClientConstants.MENU_GIDS + 31;
    }

    /// <summary>
    /// Helper sprites GIDs assigned according to 'client/data/tiles/Tile assignment.PDF'.
    /// </summary>
    public class HelperSpritesGid
    {
        /// <summary>
        /// GID for the green indicator.
        /// </summary>
        public const short GREENINDICATOR = ClientConstants.HELPER_GIDS + 1;
        // to be replaced with HELPER_GID + 1

        /// <summary>
        /// GID for the red indicator.
        /// </summary>
        public const short REDINDICATOR = ClientConstants.HELPER_GIDS + 2;
        // to be replaced with HELPER_GID + 2

        /// <summary>
        /// GID for the white indicator.
        /// </summary>
        public const short WHITEINDICATOR = ClientConstants.HELPER_GIDS + 3;
        // to be replaced with HELPER_GID + 3

        /// <summary>
        /// GID for the white indicator.
        /// </summary>
        public const short BLUEINDICATOR = ClientConstants.HELPER_GIDS + 4;
        // to be replaced with HELPER_GID + 4
    }
}