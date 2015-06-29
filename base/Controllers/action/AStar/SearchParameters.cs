using @base.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar
{
    /// <summary>
    /// Defines the parameters which will be used to find a path across a section of the map
    /// </summary>
    public class SearchParameters
    {
        public PositionI StartLocation { get; set; }

        public PositionI EndLocation { get; set; }

        public SearchParameters(PositionI startLocation, PositionI endLocation)
        {
            this.StartLocation = startLocation;
            this.EndLocation = endLocation;     
        }
    }
}
