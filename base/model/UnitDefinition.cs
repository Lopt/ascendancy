using System;

namespace @base.model.definitions
{
    public class UnitDefinition : Definition
    {
        public enum UnitDefinitionType
        {
            Swordfighter,
            Babarian,
            Spearbearer,
            HeavyInfantry,
            Hero
        }
      
        public UnitDefinition(Guid guid, DefinitionType type, 
            UnitDefinitionType unitType, @base.control.Action[] actions, int attack, int defense, int health, int moves) 
            : base(guid, type)
        {

            m_unitType = unitType;
            m_actions = actions;
            m_attack = attack;
            m_defense = defense;
            m_health = health;
            m_moves = moves;
        }

        public @base.control.Action[] Actions
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
        private @base.control.Action[] m_actions;
        private int m_attack;
        private int m_defense;
        private int m_health;
        private int m_moves;

    }
}

