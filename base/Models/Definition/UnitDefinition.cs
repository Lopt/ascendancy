using System;

namespace @base.model.definitions
{
    public class UnitDefinition : Definition
    {
        public enum UnitDefinitionType
        {
            Warrior, 
            Mage,
            Hero,
            Unknown1,
            Unknown2,
            Unknown3,

            Headquarter,
            Outposts,
            Houses,
            Wall,
            Barracks,
            RessourceHarvester
        }
      
        public UnitDefinition(Guid guid, DefinitionType type, 
            UnitDefinitionType unitType, control.action.Action[] actions, int attack, int defense, int health, int moves, Ressources ressource = Ressources.Gold) 
            : base(guid, type)
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
            get { return m_unitType; }
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

