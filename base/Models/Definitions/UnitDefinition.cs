namespace Core.Models.Definitions
{
    using System;
    using Resources;

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
        /// <param name="attackRange">AttackRange value.</param>
        /// <param name="defense">Defense value.</param>
        /// <param name="health">Maximum Health.</param>
        /// <param name="moves">Moves per turn.</param>
        /// <param name="attackRange">Attack range.</param>
        /// <param name="population">Population.</param>
        /// <param name="scrapcost">Scrap cost.</param>
        /// <param name="energycost">Energy cost.</param>
        /// <param name="plutoniumcost">Plutonium cost.</param>
        /// <param name="techcost">Tech cost.</param>
        public UnitDefinition(
            EntityType unitType,
            string[] actions,
            int attack,
            int defense,
            int health,
            int moves,
            int attackRange,
            int population,
            int scrapcost,
            int energycost,
            int plutoniumcost,
            int techcost)
            : base((int)unitType)
        {
            Actions = actions;
            Attack = attack;
            Defense = defense;
            Health = health;
            Moves = moves;
            AttackRange = attackRange;
            Population = population;
            Scrapecost = scrapcost;
            Energycost = energycost;
            Plutoniumcost = plutoniumcost;
            Techcost = techcost;
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

        /// <summary>
        /// Gets the attack range.
        /// </summary>
        /// <value>The attack range.</value>
        public int AttackRange
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the population.
        /// </summary>
        /// <value>The population.</value>
        public int Population
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the scrap cost.
        /// </summary>
        /// <value>The scrap cost.</value>
        public int Scrapecost
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the energy cost.
        /// </summary>
        /// <value>The energy cost.</value>
        public int Energycost
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the plutonium cost.
        /// </summary>
        /// <value>The plutonium cost.</value>
        public int Plutoniumcost
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the tech cost.
        /// </summary>
        /// <value>The tech cost.</value>
        public int Techcost
        {
            get;
            private set;
        }
    }
}
