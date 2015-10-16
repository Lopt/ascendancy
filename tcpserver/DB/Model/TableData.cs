using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DB.Models
{
    /// <summary>
    /// Collection of all databank entrys. s
    /// </summary>
    public class TableData
    {
        public int Id;
        public IList m_buildings = new List<TableBuilding>();
        public IList m_units = new List<TableUnit>();
        public IList m_ressources = new List<TableRessource>(); 
    }
}
