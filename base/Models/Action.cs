using System;
using System.Collections.Generic;
using Core.Models;
using Core.Models.Definitions;
using Newtonsoft.Json;
using System.Collections;

namespace Core.Models
{
    public class Action : ModelEntity
    {
        public class ActionComparer : Comparer<Action>
        {
            // Compares by Length, Height, and Width.
            public override int Compare(Action first, Action second)
            {
                return first.ID - second.ID;
            }

        }

        public enum ActionType
        {
            TestAction,
            CreateHeadquarter,
            CreateBuilding,
            CreateUnit,
            MoveUnit,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.Actions.Action"/> class.
        /// </summary>
        /// <param name="actionType">Action type.</param>
        /// <param name="regions">Affected Regions of this action.</param>
        /// <param name="parameters">Parameters.</param>
        public Action(Account account, ActionType type,
                      Dictionary<string, object> parameters)
            : base()
        {
            Account = account;
            Parameters = parameters;
            Type = type;
            ActionTime = DateTime.Now;

            switch (type)
            {
                case(ActionType.CreateHeadquarter):
                    Control = new Controllers.Actions.CreateHeadquarter(this);
                    break;   
                case(ActionType.TestAction):
                    Control = new Controllers.Actions.TestAction(this);
                    break;   
                case(ActionType.CreateUnit):
                    Control = new Controllers.Actions.CreateUnit(this);
                    break;
                case(ActionType.MoveUnit):
                    Control = new Controllers.Actions.MoveUnit(this);
                    break;
                case (ActionType.CreateBuilding):
                    Control = new Controllers.Actions.CreatBuilding(this);
                    break;
            }
        }

        public Dictionary<string, object> Parameters
        {
            get;
            private set;
        }

        public ActionType Type
        {
            get;
            private set;
        }

        public int ID;

        [JsonIgnore]
        public DateTime ActionTime;

        [JsonIgnore]
        public Account Account;

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

        public override bool Equals(Object obj)
        {
            if (obj.GetType() == typeof(Action))
            {
                var other = (Action)obj;

                return other.ID == ID;
            }
            return false;
        }


        public override int GetHashCode()
        {
            return ID;
        }
    }
}

