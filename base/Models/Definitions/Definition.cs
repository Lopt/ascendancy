using System;

namespace Core.Models.Definitions
{
    public enum EntityType
    {
        // Terrain Range 0-99
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

    
        // ID modulo 6 = 0 -> Gold
        // ID modulo 6 = 1 -> Fire
        // ID modulo 6 = 2 -> Water
        // ID modulo 6 = 3 -> Earth
        // ID modulo 6 = 4 -> Air
        // ID modulo 6 = 5 -> Magic
        // example: Hero ID 60 is Gold, 61 is Hero-Fire,
        // 62 is Hero-Water...

        // Unit Range 60-275 Id's
        Hero = 60,
        Mage = 66,
        Warrior = 72,
        Archer = 78,
        Scout = 84,
        Unknown3 = 90,

        // Buildings Range 276-491 Id's
        Headquarter = 276,
        Outposts = 282,
        Houses = 288,
        Wall = 294,
        Barracks = 300,
        RessourceHarvester = 306
    }

    public enum Category
    {
        Invalid = -1,
        Terrain,
        Unit,
        Building
    }



    public class Definition
    {

        public Definition(int id)
        {
            ID = id;
        }

        public int ID
        {
            get;
            private set;
        }

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

        public EntityType SubType
        {
            get
            {
                return (EntityType)ID;
            }
        }
    }
}

