namespace Core.Models.Definitions
{
    using System;

    /// <summary>
    /// Unit definition which contains information about a unit type.
    /// </summary>
    public class UnitDefinition : Definition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Definitions.UnitDefinition"/> class.
        /// </summary>
        /// <param name="unitType">Unit type.</param>
        /// <param name="actions">The actions which can be used by this kind of units.</param>
        /// <param name="attack">Attack value.</param>
        /// <param name="defense">Defense value.</param>
        /// <param name="health">current Health.</param>
        /// <param name="moves">Moves per turn.</param>
        public UnitDefinition(
            EntityType unitType,
            string[] actions,
            int attack,
            int defense,
            int health,
            int moves)
            : base((int)unitType)
        {
            Actions = actions;
            Attack = attack;
            Defense = defense;
            Health = health;
            Moves = moves;
        }

        /// <summary>
        /// Gets the main resource of the unit (fire, gold, etc.) .
        /// </summary>
        /// <value>The resource.</value>
        public Resources Resource
        {
            get
            {
                return (Resources)(ID % 6);
            }
        }

        /// <summary>
        /// Gets the actions.
        /// </summary>
        /// <value>The actions which can be used by this kind of units.</value>
        public string[] Actions
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the attack value.
        /// </summary>
        /// <value>The attack value.</value>
        public int Attack
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the defense value.
        /// </summary>
        /// <value>The defense value.</value>
        public int Defense
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current health.
        /// </summary>
        /// <value>The current health.</value>
        public int Health
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the amount of moves per turn for this kind of units.
        /// </summary>
        /// <value>The moves per turn.</value>
        public int Moves
        {
            get;
            private set;
        }   
    }
}
