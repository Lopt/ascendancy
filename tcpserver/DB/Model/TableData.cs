namespace Server.DB.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Table data collection of all data in the DB.
    /// </summary>
    public class TableData
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        public int Id;

        /// <summary>
        /// The buildings.
        /// </summary>
        public IList Buildings = new List<TableBuilding>();

        /// <summary>
        /// The units.
        /// </summary>
        public IList Units = new List<TableUnit>();

        /// <summary>
        /// The resources.
        /// </summary>
        public IList Resources = new List<TableResource>(); 
    }
}
