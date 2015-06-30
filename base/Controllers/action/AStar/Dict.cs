using AStar;
using @base.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace @base.Controllers.action.AStar
{
    class Dict
    {
        Dictionary<PositionI, Node> FieldNode = new Dictionary<PositionI, Node>();

        public Dictionary<PositionI, Node> GetDictionary()
        {
            return FieldNode;
        }
    }
}
