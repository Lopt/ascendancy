using System;

using @base.model;
using @base.model.definitions;
using Newtonsoft.Json;

namespace @base.model
{
    public class Entity : ModelEntity
    {
        public Entity(int id, Definition defintion, Account account, PositionI position)
            : base()
        {
            m_id = id; 
            m_definition = defintion;
            m_position = position;
            m_account = account;
        }

        public int ID
        {
            get { return m_id; }
        }

        public int DefinitionID
        {
            get { return m_definition.ID; }  
            set { m_definition = World.Instance.DefinitionManager.GetDefinition(value); }
        }


        [JsonIgnore]
        public Definition Definition
        {
            get { return m_definition; }
        }

        public PositionI Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        [JsonIgnore]
        public Account Account
        {
            get { return m_account; }
        }

        public int AccountID
        {
            get { return m_account.ID; }
            set { m_account = @base.control.Controller.Instance.AccountManagerController.GetAccountOrEmpty(value); }
        }


        private int m_id;
        private PositionI m_position;
        private Definition m_definition;
        private Account m_account;
    }
}

