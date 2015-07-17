using System;

namespace Core.Models.Definitions
{
    /// <summary>
    /// Unit Definiton which contains informations about a unit type.
    /// </summary>
    public class UnitDefinition : Definition
    {

        public UnitDefinition(EntityType unitType,
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
            
    }
}

