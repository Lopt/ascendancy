using @base.model;
using @base.model.definitions;
using System;

namespace AStar
{
    /// <summary>
    /// Represents a single node on a grid that is being searched for a path between two points
    /// </summary>
    public class Node
    {
        private Node parentNode;

        /// <summary>
        /// The node's location in the grid
        /// </summary>
        public PositionI Location { get; private set; }
        
        /// <summary>
        /// Cost from start to here
        /// </summary>
        public double G { get; private set; }

        /// <summary>
        /// Estimated cost from here to end
        /// </summary>
        public double H { get; private set; }

        /// <summary>
        /// Flags whether the node is open, closed or untested by the PathFinder
        /// </summary>
        public NodeState State { get; set; }

        /// <summary>
        /// Estimated total cost (F = G + H)
        /// </summary>
        public double F
        {
            get { return G + H; }
        }

        /// <summary>
        /// Gets or sets the parent node. The start node's parent is always null.
        /// </summary>
        public Node ParentNode
        {
            get { return parentNode; }
            set
            {
                // When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
                parentNode = value;
                G = parentNode.G + GetTraversalCost(Location, parentNode.Location);
            }
        }

       /// <summary>
       /// Constructor from a Node, it set the positionI of the Node, set the nodstate to open and calculate the travel cost to the destination tile.
       /// </summary>
       /// <param name="location"> Current postionI of the Node.</param>
       /// <param name="endLocation"> PositionI of the destination tile.</param>
        public Node(PositionI location, PositionI endLocation)
        {
           Location = location;
           State = NodeState.Open;
           H = GetTraversalCost(Location, endLocation);
           G = 0;
        }
        
        /// <summary>
        /// Gets the distance between two points.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="otherLocation"></param>
        /// <returns></returns>
        internal static double GetTraversalCost(PositionI location, PositionI otherLocation)
        {
            return location.Distance(otherLocation);            
            
        }
    }
}
