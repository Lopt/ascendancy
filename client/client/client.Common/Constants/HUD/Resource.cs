using System;

namespace client.Common.Constants.HUD
{
    public enum Category
    {
        Scrap,
        Population,
        Technology,       
        Plutonium 
    }

    public class Resource
    {
        public Resource(int resource)
        {
            ID = resource;
        }


        public int ID
        {
            get;
            private set;
        }
    }
}



