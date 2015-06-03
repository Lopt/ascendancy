using System;
using System.Collections.Concurrent;
using @base.model;
using @base.model.definitions;
using Newtonsoft.Json;

namespace @base.model
{
    public class Action : ModelEntity
    {
        public enum ActionType 
        {
            CreateHeadquarter,
            Move,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="base.control.action.Action"/> class.
        /// </summary>
        /// <param name="actionType">Action type.</param>
        /// <param name="regions">Affected Regions of this action.</param>
        /// <param name="parameters">Parameters.</param>
        public Action(Account account, ActionType actionType,
            ConcurrentDictionary<string, object> parameters)
            : base()
        {
            m_account = account;
            m_parameters = parameters;
            m_actionType = actionType;
            m_actionTime = DateTime.Now;

            switch (actionType)
            {
                case(ActionType.CreateHeadquarter):
                    Control = new control.action.CreateHeadquarter(this);
                    break;   
            }
        }

        public ConcurrentDictionary<string, object> Parameters
        {
            get { return m_parameters; }
        }
           
        public ActionType Type
        {
            get { return m_actionType; }
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

        private ConcurrentDictionary<string, object> m_parameters;
        private ActionType m_actionType;
        private DateTime m_actionTime; 
        private Account m_account;

    }
}

