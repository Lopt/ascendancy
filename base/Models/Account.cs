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
            TerritoryBuildings = new Dictionary<long, PositionI>();
            Buildings = new LinkedList<PositionI>();
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
            TerritoryBuildings = new Dictionary<long, PositionI>();
            Units = new LinkedList<PositionI>();
            Buildings = new LinkedList<PositionI>();
            BuildableBuildings = new Dictionary<long, List<long>>();

            Scrap = new Scrap();
            Population = new Population();
            Technology = new Technology();
            Energy = new Energy();
            Plutonium = new Plutonium();
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
        public Dictionary<long, PositionI> TerritoryBuildings
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the buildings.
        /// </summary>
        /// <value>The buildings.</value>
        public LinkedList<PositionI> Buildings
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