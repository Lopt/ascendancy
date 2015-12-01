namespace Core.Models.Definitions
{
    using System;

    /// <summary>
    /// Contains which type a entity exactly is.
    /// <para></para>
    /// Terrain Range 0-59 Id's (6 * 10 = 60)
    /// Unit Range 60-275 Id's (6 * 6 * 6 = 216)
    /// Buildings Range 276-491 Id's (6 * 6 * 6 = 216)
    /// <para></para>
    /// ID modulo 6 = 0 -> Gold
    /// ID modulo 6 = 1 -> Fire
    /// ID modulo 6 = 2 -> Water
    /// ID modulo 6 = 3 -> Earth
    /// ID modulo 6 = 4 -> Air
    /// ID modulo 6 = 5 -> Magic
    /// example: Hero ID 60 is Gold, 61 is Hero-Fire,
    /// 62 is Hero-Water...
    /// </summary>
    public enum EntityType
    {
        // Terrain Range 0-59
        Water = 0,
        Buildings = 1,
        Woods = 2,
        Grassland = 3,
        Fields = 4,
        Streets = 5,
        NotDefined = 6,
        Forbidden = 7,
        Town = 8,
        Glacier = 9,
        Beach = 10,
        Park = 11,
        Invalid = 12,

        // Unit Range 60-275 Id's
        Hero = 60,
        Mage = 66,
        Warrior = 72,
        Archer = 78,
        Scout = 84,
        Unknown3 = 90,

        // Buildings Range 276-491 Id's
        Headquarter = 276,
        Barracks = 282,
        Factory = 288,
        Attachment = 294,
        GuardTower = 300,
        Hospital = 306,
        TradingPost = 312,

        // ressource buildings
        Lab = 300,
        Furnace = 282,
        Transformer = 306,
        Scrapyard = 312,
        Tent = 318,

    }

    /// <summary>
    /// Contains the category of a entity.
    /// </summary>
    public enum Category
    {
        Invalid = -1,
        Terrain,
        Unit,
        Building
    }

    /// <summary>
    /// Definition contains static information about entities.
    /// In which Category they belong and their id.
    /// Will be used as base-class for other definitions.
    /// </summary>
    public class Definition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Definitions.Definition"/> class.
        /// </summary>
        /// <param name="id">Identifier of the definition.</param>
        public Definition(int id)
        {
            ID = id;
        }

        /// <summary>
        /// Gets the Identifier.
        /// </summary>
        /// <value>The Identifier.</value>
        public int ID
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>The category.</value>
        public Category Category
        {
            get
            {
                if (ID < 60)
                {
                    return Category.Terrain;
                }
                if (ID < 276)
                {
                    return Category.Unit;
                }
                if (ID < 1000)
                {
                    return Category.Building;
                }
                return Category.Invalid;
            }
        }

        /// <summary>
        /// Gets the type of the sub. (also currently equal to the ID)
        /// </summary>
        /// <value>The type of the sub.</value>
        public EntityType SubType
        {
            get
            {
                return (EntityType)ID;
            }
        }
    }
}