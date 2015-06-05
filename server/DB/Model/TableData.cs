using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.DB.Model
{
    public class TableData
    {
        public IList m_buildings = new List<TableBuilding>();
        public IList m_units = new List<TableUnit>();
        public IList m_ressources = new List<TableRessource>(); 
    }
}
