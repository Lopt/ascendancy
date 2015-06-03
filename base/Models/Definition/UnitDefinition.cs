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

            // Unit Range 60-275
            Hero = 60,
            Mage = 66,
            Warrior = 72, 
            Unknown1 = 78,
            Unknown2 = 84,
            Unknown3 = 90,
           
            // Buildings Range 276-491
            Headquarter = 276,
            Outposts = 282,
            Houses = 288,
            Wall = 294,
            Barracks = 300,
            RessourceHarvester = 306
        }
      
        public UnitDefinition(UnitDefinitionType unitType,
                              control.action.Action[] actions,
                              int attack, int defense,
                              int health, int moves) 
            : base((int) unitType)
        {
            m_actions = actions;
            m_attack = attack;
            m_defense = defense;
            m_health = health;
            m_moves = moves;
        }

        public Ressources Ressource
        {
            get { return (Ressources) (ID % 6); }
        }

        public @base.control.action.Action[] Actions
        {
            get { return m_actions; }
        }

        public int Attack
        {
            get { return m_attack; }
        }

        public int Defense
        {
            get { return m_defense; }
        }

        public int Health
        {
            get { return m_health; }
        }

        public int Moves
        {
            get { return m_moves; }
        }

        public UnitDefinitionType UnitType
        {
            get { return (UnitDefinitionType) ID; }
        }

        private control.action.Action[] m_actions;
        private int m_attack;
        private int m_defense;
        private int m_health;
        private int m_moves;

    }
}

