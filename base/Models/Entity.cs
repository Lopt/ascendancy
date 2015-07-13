using System;

using Core.Models;
using Core.Models.Definitions;
using Newtonsoft.Json;

namespace Core.Models
{
    public class Entity : ModelEntity
    {
        public Entity(int id, Definition defintion, Account account, PositionI position)
            : base()
        {
            ID = id; 
            Definition = defintion;
            Position = position;
            Account = account;
        }

        public int ID
        {
            get;
            private set;
        }

        public int DefinitionID
        {
            get
            {
                return Definition.ID;
            }  
            set
            {
                Definition = World.Instance.DefinitionManager.GetDefinition((EntityType) value);
            }
        }


        [JsonIgnore]
        public Definition Definition
        {
            get;
            private set;
        }

        public PositionI Position
        {
            get;
            set;

        }

        [JsonIgnore]
        public Account Account
        {
            get;
            private set;
        }

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

    }
}

