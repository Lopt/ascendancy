namespace Server.DB
{
    using System;
    using Server.DB.Models;
    using SQLite;

    /// <summary>
    /// DB resource.
    /// </summary>
    public class DBResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Server.DB.DBResource"/> class.
        /// </summary>
        /// <param name="con">Connection to the DB.</param>
        public DBResource(SQLiteConnection con)           
        {
            db = con;
            db.CreateTable<TableResource>();
        }

        /// <summary>
        /// Tables the resource.
        /// </summary>
        /// <param name="resourceFire">Resource fire.</param>
        /// <param name="resourceEarth">Resource earth.</param>
        /// <param name="resourceWater">Resource water.</param>
        /// <param name="resourceAir">Resource air.</param>
        /// <param name="resourceMagic">Resource magic.</param>
        /// <param name="resourceGold">Resource gold.</param>
        /// <param name="id">Identifier for the account.</param>
        public void TableResource(int resourceFire, int resourceEarth, int resourceWater, int resourceAir, int resourceMagic, int resourceGold, int id)
        {
                var newData = new TableResource();
                newData.Id = id;
                newData.Fire = resourceFire;
                newData.Earth = resourceEarth;
                newData.Water = resourceWater;
                newData.Air = resourceAir;
                newData.Gold = resourceGold;
                newData.Magic = resourceMagic;

                db.InsertOrReplace(newData);    
        }

        /// <summary>
        /// The DB.
        /// </summary>
        private SQLiteConnection db;
    }
}
