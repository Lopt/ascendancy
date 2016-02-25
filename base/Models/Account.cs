namespace Core.Models
{
    using System;
    using System.Collections.Generic;
    using Resources;

    /// <summary>
    /// Account model which represents an user and information about the user.
    /// </summary>
    public class Account : ModelEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Account"/> class.
        /// </summary>
        /// <param name="id">Account Identifier.</param>
        public Account(int id)
            : base()
        {
            ID = id;
            UserName = "???";
            TerritoryBuildings = new Dictionary<PositionI, long>();
            Buildings = new Dictionary<PositionI, long>();
            Units = new LinkedList<PositionI>();
            BuildableBuildings = new Dictionary<long, List<long>>();

            Scrap = new Scrap();
            Population = new Population();
            Technology = new Technology();
            Energy = new Energy();
            Plutonium = new Plutonium();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Account"/> class.
        /// </summary>
        /// <param name="id">Account Identifier.</param>
        /// <param name="userName">User name.</param>
        public Account(int id, string userName)
        {
            ID = id;
            UserName = userName;
            TerritoryBuildings = new Dictionary<PositionI, long>();
            Units = new LinkedList<PositionI>();
            Buildings = new Dictionary<PositionI, long>();
            BuildableBuildings = new Dictionary<long, List<long>>();

            Scrap = new Scrap();
            Population = new Population();
            Technology = new Technology();
            Energy = new Energy();
            Plutonium = new Plutonium();
        }

        /// <summary>
        /// Tests two accounts if they are the same.
        /// </summary>
        /// <returns>boolean if both accounts are the same or same ID. Otherwise false.</returns>
        /// <param name="first">First Position.</param>
        /// <param name="second">Second Position.</param>
        public static bool operator ==(Account first, Account second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)first == null) || ((object)second == null))
            {
                return false;
            }

            return first.ID == second.ID;
        }

        /// <summary>
        /// Tests two accounts if they are NOT the same.
        /// </summary>
        /// <returns>boolean if both accounts are NOT the same or NOT the same ID. Otherwise false.</returns>
        /// <param name="first">First position.</param>
        /// <param name="second">Second position.</param>
        public static bool operator !=(Account first, Account second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Gets the Id
        /// </summary>
        /// <value>The Id.</value>
        public int ID
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the territory buildings.
        /// </summary>
        /// <value>The territory buildings.</value>
        public Dictionary<PositionI, long> TerritoryBuildings
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the buildings.
        /// </summary>
        /// <value>The buildings.</value>
        public Dictionary<PositionI, long> Buildings
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <value>The units.</value>
        public LinkedList<PositionI> Units
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the buildable buildings.
        /// </summary>
        /// <value>The buildable buildings.</value>
        public Dictionary<long, List<long>> BuildableBuildings;

        /// <summary>
        /// The scrap.
        /// </summary>
        public Scrap Scrap;

        /// <summary>
        /// The plutonium.
        /// </summary>
        public Plutonium Plutonium;

        /// <summary>
        /// The energy.
        /// </summary>
        public Energy Energy;

        /// <summary>
        /// The technology.
        /// </summary>
        public Technology Technology;

        /// <summary>
        /// The population.
        /// </summary>
        public Population Population;
    }
}