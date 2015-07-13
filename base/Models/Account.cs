using System;
using SQLite;
using System.Collections.Generic;

namespace Core.Models
{
    public class Account : ModelEntity
    {
       
        public Account(int id)
            : base()
        {
            ID = id;
            UserName = "???";
            Headquarters = new LinkedList<PositionI>();
            Buildings = new LinkedList<PositionI>();
            Units = new LinkedList<PositionI>();
        }

        public Account(int id, string userName)
        {
            ID = id;
            UserName = userName;
            Headquarters = new LinkedList<PositionI>();
            Units = new LinkedList<PositionI>();
            Buildings = new LinkedList<PositionI>();
        }

        public int ID
        {

            get;
            private set;
        }

        public string UserName
        {

            get;
            set;
        }

        public LinkedList<PositionI> Headquarters
        {

            get;
            private set;
        }

        public LinkedList<PositionI> Units
        {

            get;
            private set;
        }

        public LinkedList<PositionI> Buildings
        {
            get;
            private set;
        }

    }
}

