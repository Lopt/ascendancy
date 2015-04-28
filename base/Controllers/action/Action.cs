﻿using System;
using System.Collections.Concurrent;
using @base.model;
using @base.model.definitions;

namespace @base.control.action
{
    public class Action
    {
        public enum ActionType 
        {
            Create,
            Move,
            Attack,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="base.control.action.Action"/> class.
        /// </summary>
        /// <param name="actionType">Action type.</param>
        /// <param name="regions">Affected Regions of this action.</param>
        /// <param name="parameters">Parameters.</param>
        public Action(Account account, ActionType actionType,
            ConcurrentDictionary<string, object> parameters)
        {
            m_account = account;
            m_parameters = parameters;
            m_actionType = actionType;
            m_actionTime = DateTime.Now;
        }

        virtual public Region GetMainRegion()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        virtual public bool Possible()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns True if everything worked, otherwise False
        /// </summary>
        virtual public bool Do()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary>
        virtual public bool Catch()
        {
            throw new NotImplementedException();
        }

        public ConcurrentDictionary<string, object> Parameters
        {
            get { return m_parameters; }
        }
           
        public ActionType Type
        {
            get { return m_actionType; }
        }

        public DateTime ActionTime
        {
            get { return m_actionTime; }
        }

        public Account Account
        {
            get { return m_account; }
        }

        private ConcurrentDictionary<string, object> m_parameters;
        private ActionType m_actionType;
        private DateTime m_actionTime; 
        private Account m_account;

    }
}

