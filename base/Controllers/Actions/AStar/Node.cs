namespace Core.Controllers.AStar
{
    using System;
    using Core.Models;
    using Core.Models.Definitions;

    /// <summary>
    /// Represents a single node on a grid that is being searched for a path between two points
    /// </summary>
    public class Node
    {    
        /// <summary>
        /// Flags whether the node is open, closed or untested by the PathFinder
        /// </summary>
        public NodeState State;

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        public PositionI Location
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the g value it stand for the costs from start to this position.
        /// </summary>
        /// <value>The g.</value>
        public double G
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the h value it stand for the estimated cost from this position to end destination.
        /// </summary>
        /// <value>The h.</value>
        public double H
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the f value it stand for estimated total cost (F = G + H).
        /// </summary>
        /// <value>The f.</value>
        public double F
        {
            get
            {
                return G + H;
            }
        }

        /// <summary>
        /// Gets or sets the parent node. The start node's parent is always null.
        /// </summary>
        /// <value>The parent node.</value>
        public Node ParentNode
        {
            get
            {
                return parentNode;
            }

            set
            {
                // When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
                parentNode = value;
                G = parentNode.G + GetTraversalCost(Location, parentNode.Location);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.AStar.Node"/> class. 
        /// It set the positionI of the Node, set the node state to open and calculate the travel cost to the destination tile.
        /// </summary>
        /// <param name="location">Current PositionI of the Node.</param>
        /// <param name="endLocation">PositionI of the destination tile.</param>
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
        /// <returns>The traversal cost.</returns>
        /// <param name="location">Current PositionI</param>
        /// <param name="otherLocation">Other PositionI.</param>
        internal static double GetTraversalCost(PositionI location, PositionI otherLocation)
        {
            return location.Distance(otherLocation);   
        }    

        /// <summary>
        /// The parent node.
        /// </summary>
        private Node parentNode;
    }
}
