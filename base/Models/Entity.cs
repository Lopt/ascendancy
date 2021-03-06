﻿namespace Core.Models
{
    using System;
    using Core.Models;
    using Core.Models.Definitions;
    using Newtonsoft.Json;

    /// <summary>
    /// The possible diplomatic states for two players
    /// </summary>
    public enum Diplomatic
    {
        own,
        allied,
        enemy,
    }

    /// <summary>
    /// Entity which represents an "object" in the game world: units, terrain, buildings... etc.
    /// </summary>
    public class Entity : ModelEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Entity"/> class.
        /// </summary>
        /// <param name="id">Identifier of the entity.</param>
        /// <param name="definition">Definition of the entity.</param>
        /// <param name="owner">Owner of the entity.</param>
        /// <param name="position">Position of the entity.</param>
        /// <param name="health">Health of the entity.</param>
        /// <param name="move">Move of the entity.</param>
        public Entity(int id, Definition definition, Account owner, PositionI position, int health, int move)
            : base()
        {
            ID = id; 
            Definition = definition;
            Position = position;
            Owner = owner;
            Health = health;           
            Move = move;
            ModifiedDefenseValue = ((UnitDefinition)definition).Attack;
            ModfiedAttackValue = ((UnitDefinition)definition).Defense;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Entity"/> class.
        /// </summary>
        /// <param name="id">Identifier of the entity.</param>
        /// <param name="definitonID">Definiton ID.</param>
        /// <param name="ownerID">Owner ID.</param>
        /// <param name="position">Position as integer.</param>
        /// <param name="health">Health of the entity.</param>
        /// <param name="move">Move of the entity.</param>
        [JsonConstructor]
        public Entity(int id, int definitonID, int ownerID, PositionI position, int health, int move)
            : this(id, World.Instance.DefinitionManager.GetDefinition((EntityType)definitonID), World.Instance.AccountManager.GetAccountOrEmpty(ownerID), position, health, move)
        {            
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
        /// Gets the current health (in percent).
        /// </summary>
        /// <value>health (in percent).</value>
        public float HealthPercent
        {
            get
            {
                var maxHealth = ((UnitDefinition)Definition).Health;
                return Math.Max(Health / (float)maxHealth, 0.0f);
            }
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
        /// Gets or sets the modified attack value.
        /// </summary>
        /// <value>The modified attack value.</value>
        public int ModfiedAttackValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the modified defense value.
        /// </summary>
        /// <value>The modified defense value.</value>
        public int ModifiedDefenseValue
        {
            get;
            set;
        }

        /// <summary>
        /// Calculates and returns the diplomatic states between the owner of the unit and the given account
        /// </summary>
        /// <returns>The diplomacy.</returns>
        /// <param name="account">Other Account.</param>
        public Diplomatic GetDiplomacy(Account account)
        {
            if (account.ID == Owner.ID)
            {
                return Diplomatic.own;
            }
            return Diplomatic.enemy;
        }
    }
}