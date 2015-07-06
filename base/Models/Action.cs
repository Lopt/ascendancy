using System;
using System.Collections.Generic;
using @base.model;
using @base.model.definitions;
using Newtonsoft.Json;
using System.Collections;

namespace @base.model
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
        /// Initializes a new instance of the <see cref="base.control.action.Action"/> class.
        /// </summary>
        /// <param name="actionType">Action type.</param>
        /// <param name="regions">Affected Regions of this action.</param>
        /// <param name="parameters">Parameters.</param>
        public Action(Account account, ActionType type,
            Dictionary<string, object> parameters)
            : base()
        {
            m_account = account;
            m_parameters = parameters;
            m_actionType = type;
            m_actionTime = DateTime.Now;

            switch (type)
            {
                case(ActionType.CreateHeadquarter):
                    Control = new control.action.CreateHeadquarter(this);
                    break;   
                case(ActionType.TestAction):
                    Control = new control.action.TestAction(this);
                    break;   
                case(ActionType.CreateUnit):
                    Control = new control.action.CreateUnit(this);
                    break;
                case(ActionType.MoveUnit):
                    Control = new control.action.MoveUnit(this);
                    break;
                case (ActionType.CreateBuilding):
                    Control = new control.action.CreatBuilding(this);
                    break;
            }
        }

        public Dictionary<string, object> Parameters
        {
            get { return m_parameters; }
        }
           
        public ActionType Type
        {
            get { return m_actionType; }
        }

        public int ID
        {
            get;
            set;
        }

        [JsonIgnore]
        public DateTime ActionTime
        {
            get { return m_actionTime; }
            set { m_actionTime = value; }
        }

        [JsonIgnore]
        public Account Account
        {
            get { return m_account; }
            set { m_account = value; }
        }

        public int AccountID
        {
            get { return m_account.ID; }
            set { m_account = @base.control.Controller.Instance.AccountManagerController.GetAccountOrEmpty(value); }
        }

        public override bool Equals(Object obj)
        {
            if (obj.GetType() == typeof(Action))
            {
                var other = (Action)obj;

                return other.ID == ID
            }
            return false;
        }


        public override int GetHashCode()
        {
            return ID;
        }


        private Dictionary<string, object> m_parameters;
        private ActionType m_actionType;
        private DateTime m_actionTime; 
        private Account m_account;

    }
}

