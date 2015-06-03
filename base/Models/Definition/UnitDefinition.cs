using System;

namespace @base.model.definitions
{
    public class UnitDefinition : Definition
    {
        public enum UnitDefinitionType
        {
            // Unit Range 100-199
            Mage = 101,
            Hero = 102,
            Warrior = 103, 
            Unknown1 = 104,
            Unknown2 = 105,
            Unknown3 = 106,
           
            // Buildings Range 100-199
            Headquarter = 201,
            Outposts = 202,
            Houses = 203,
            Wall = 204,
            Barracks = 205,
            RessourceHarvester = 206
        }
      
        public UnitDefinition(UnitDefinitionType unitType,
                              control.action.Action[] actions,
                              int attack, int defense,
                              int health, int moves,
                              Ressources ressource = Ressources.Gold) 
            : base((int) unitType)
        {
            m_unitType = unitType;
            m_actions = actions;
            m_attack = attack;
            m_defense = defense;
            m_health = health;
            m_moves = moves;
            m_ressource = ressource;
        }

        public Ressources Ressource
        {
            get { return m_ressource; }
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

        private UnitDefinitionType m_unitType;
        private control.action.Action[] m_actions;
        private int m_attack;
        private int m_defense;
        private int m_health;
        private int m_moves;
        private Ressources m_ressource;

    }
}

