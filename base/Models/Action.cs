namespace Core.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Core.Models;
    using Core.Models.Definitions;
    using Newtonsoft.Json;

    /// <summary>
    /// Action model which should contains all needed data for the action control.
    /// </summary>
    public class Action : ModelEntity
    {
        /// <summary>
        /// Action comparer.
        /// </summary>
        public class ActionComparer : Comparer<Action>
        {
            /// <summary>
            /// Compares by Length, Height, and Width.
            /// </summary>
            /// <param name="first">First Action.</param>
            /// <param name="second">Second Action.</param>
            /// <returns>greater integer, depending which action comes first</returns>
            public override int Compare(Action first, Action second)
            {
                return first.ID - second.ID;
            }
        }

        /// <summary>
        /// Action type.
        /// </summary>
        public enum ActionType
        {
            TestAction,
            CreateTerritoryBuilding,
            CreateBuilding,
            CreateUnit,
            MoveUnit,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Action"/> class.
        /// </summary>
        /// <param name="account">Account which wants to execute this action.</param>
        /// <param name="type">Action Type.</param>
        /// <param name="parameters">Parameters which should contain all needed data by the action control.</param>
        public Action(
            Account account,
            ActionType type,
            Dictionary<string, object> parameters,
            DateTime? actionTime = null)
            : base()
        {
            Account = account;
            Parameters = parameters;
            Type = type;
            if (actionTime == null)
            {
                ActionTime = DateTime.Now;
            }
            else
            {
                ActionTime = actionTime.Value;
            }

            switch (type)
            {   
                case ActionType.CreateUnit:
                    Control = new Controllers.Actions.CreateUnit(this);
                    break;
                case ActionType.MoveUnit:
                    Control = new Controllers.Actions.MoveUnit(this);
                    break;
                case ActionType.CreateBuilding:
                    Control = new Controllers.Actions.CreateBuilding(this);
                    break;
                case ActionType.CreateTerritoryBuilding:
                    Control = new Controllers.Actions.CreateTerritoryBuilding(this);
                    break;
            }


        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public Dictionary<string, object> Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public ActionType Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or Sets the ID
        /// </summary>
        public int ID;

        /// <summary>
        /// The action time.
        /// </summary>
        public DateTime ActionTime;

        /// <summary>
        /// The account.
        /// </summary>
        [JsonIgnore]
        public Account Account;

        /// <summary>
        /// Gets or sets the account ID.
        /// </summary>
        /// <value>The account ID.</value>
        public int AccountID
        {
            get
            {
                return Account.ID;
            }

            set
            {
                Account = World.Instance.AccountManager.GetAccountOrEmpty(value);
            }
        }

        /// <summary>
        /// tests if the given object is equal to this object
        /// </summary>
        /// <returns>true, if it is equal, otherwise false.</returns>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Core.Models.Action"/>.</param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Action))
            {
                var other = (Action)obj;

                return other.ID == ID;
            }
            return false;
        }

        /// <summary>
        /// standard hash function
        /// </summary>
        /// <returns>hash code.</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return ID;
        }
    }
}