namespace Server.DB.Models
{
    using System;
    using SQLite;

    /// <summary>
    /// Table resource.
    /// </summary>
    [Table("Resources")]
    public class TableResource
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey, Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the fire.
        /// </summary>
        /// <value>The fire.</value>
        public int Fire { get; set; }

        /// <summary>
        /// Gets or sets the earth.
        /// </summary>
        /// <value>The earth.</value>
        public int Earth { get; set; }

        /// <summary>
        /// Gets or sets the water.
        /// </summary>
        /// <value>The water.</value>
        public int Water { get; set; }

        /// <summary>
        /// Gets or sets the air.
        /// </summary>
        /// <value>The air.</value>
        public int Air { get; set; }

        /// <summary>
        /// Gets or sets the magic.
        /// </summary>
        /// <value>The magic.</value>
        public int Magic { get; set; }

        /// <summary>
        /// Gets or sets the gold.
        /// </summary>
        /// <value>The gold.</value>
        public int Gold { get; set; }
    }
}
