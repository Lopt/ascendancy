namespace Core.Models
{
    using System;
    using Core.Models;
    using Core.Models.Definitions;
    using Newtonsoft.Json;

    /// <summary>
    /// Entity which represents an "object" in the game world: units, terrain, buildings... etc.
    /// </summary>
    public class Entity : ModelEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Entity"/> class.
        /// </summary>
        /// <param name="id">Identifier of entity.</param>
        /// <param name="definition">Entity Type Definition.</param>
        /// <param name="owner">Entity Owner.</param>
        /// <param name="position">Entity Position.</param>
        /// <param name="health">Entity Health.</param>
        public Entity(int id, Definition definition, Account owner, PositionI position, int health, int move)
            : base()
        {
            ID = id; 
            Definition = definition;
            Position = position;
            Owner = owner;
            Health = health;           
            Move = move;
            ModifiedDefenseValue = ((UnitDefinition)Definition).Defense;
            ModfifedAttackValue = ((UnitDefinition)Definition).Attack;
        }

        /// <summary>
        /// Gets the ID
        /// </summary>
        /// <value>The ID</value>
        public int ID
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the definition ID.
        /// </summary>
        /// <value>The definition ID.</value>
        public int DefinitionID
        {
            get
            {
                return Definition.ID;
            }

            set
            {
                Definition = World.Instance.DefinitionManager.GetDefinition((EntityType)value);
            }
        }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        [JsonIgnore]
        public Definition Definition
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public PositionI Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the account.
        /// </summary>
        /// <value>The account.</value>
        [JsonIgnore]
        public Account Owner
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the account I.
        /// </summary>
        /// <value>The account ID.</value>
        public int OwnerID
        {
            get
            {
                return Owner.ID;
            }

            set
            {
                Owner = World.Instance.AccountManager.GetAccountOrEmpty(value);
            }
        }

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        /// <value>The health.</value>
        public int Health
        {
            get;
            set;                
        }

        /// <summary>
        /// Gets or sets the move.
        /// </summary>
        /// <value>The move.</value>
        public int Move
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the modfifed attack value.
        /// </summary>
        /// <value>The modfifed attack value.</value>
        public int ModfifedAttackValue
        {
            get;
            //{
                // TODO: Add Weather, clocktime and terrain modifier 
                //var ModAttack = ((UnitDefinition)Definition).Attack;
                //return ModAttack /* Weather + CLocktime + Terrain */; 
            //}
            set;
            //{}
        }

        /// <summary>
        /// Gets the modified defense value.
        /// </summary>
        /// <value>The modified defense value.</value>
        public int ModifiedDefenseValue
        {
            get;
            //{
                //var ModDefense = World.Instance.RegionManager.GetRegion(Position.RegionPosition).GetTerrain(Position.CellPosition).DefenseModifier;
                //var UnitDefense = ((UnitDefinition)Definition).Defense;
                //return UnitDefense * ModDefense;
            //}
            set;
            //{}
        }
    }
}