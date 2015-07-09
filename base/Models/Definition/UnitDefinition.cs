using System;

namespace @base.model.definitions
{
    public class UnitDefinition : Definition
    {
        public enum UnitDefinitionType
        {
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

        public UnitDefinition(UnitDefinitionType unitType,
                              string[] actions,
                              int attack, int defense,
                              int health, int moves)
            : base((int)unitType)
        {
            Actions = actions;
            Attack = attack;
            Defense = defense;
            Health = health;
            Moves = moves;
        }

        public Ressources Ressource
        {
            get
            {
                return (Ressources)(ID % 6);
            }
        }

        public string[] Actions
        {
            get;
            private set;
        }

        public int Attack
        {
            get;
            private set;

        }

        public int Defense
        {
            get;
            private set;
        }

        public int Health
        {
            get;
            private set;
        }

        public int Moves
        {
            get;
            private set;
        }

        public UnitDefinitionType UnitType
        {
            get
            {
                return (UnitDefinitionType)ID;
            }
        }

    }
}

