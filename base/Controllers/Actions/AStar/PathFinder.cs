namespace Core.Controllers.AStar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Controllers.Actions;
    using Core.Models;

    /// <summary>
    ///  http://blog.two-cats.com/2014/06/a-star-example/ founded by Mike Clift
    /// </summary>
    public class PathFinder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.AStar.PathFinder"/> class.
        /// </summary>
        /// <param name="searchParameters">Search parameters.</param>
        public PathFinder(SearchParameters searchParameters)
        {
            m_nodes = new Dictionary<PositionI, Node>();
            this.searchParameters = searchParameters;
            startNode = new Node(searchParameters.StartLocation, searchParameters.EndLocation);            
            m_nodes[searchParameters.StartLocation] = startNode;
        }

        /// <summary>
        /// Attempts to find a path from the start location to the end location based on the supplied SearchParameters.
        /// </summary>
        /// <returns>The path.</returns>
        /// <param name="moves">Amount of moves from the unit.</param>
        public List<PositionI> FindPath(int moves)
        {
            // The start node is the first entry in the 'open' list
            List<PositionI> path = new List<PositionI>();
            bool success = Search(startNode, moves);
            if (success)
            {
                // If a path was found, follow the parents from the end node to build a list of locations
                Node node = m_nodes[searchParameters.EndLocation];
                while (node.ParentNode != null)
                {
                    path.Add(node.Location);
                    node = node.ParentNode;
                }

                // Reverse the list so it's in the correct order when returned
                path.Reverse();
            }
           
            return path;
        }

        /// <summary>
        /// Attempts to find a path to the destination node using <paramref name="currentNode"/> as the starting location with consideration of the moves from the current unit.
        /// </summary>
        /// <param name="currentNode"> Current code which is under discover.</param>
        /// <param name="moves">Amount of moves from the unit.</param>
        /// <returns> True if the path was found otherwise false.</returns>
        private bool Search(Node currentNode, int moves)
        {
            // Set the current node to Closed since it cannot be traversed more than once
            currentNode.State = NodeState.Closed;
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);

            // Sort by F-value so that the shortest possible routes are considered first
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            // decrement the unit move
            if (moves != 0)
            {
                --moves;
                foreach (var nextNode in nextNodes)
                {
                    // Check whether the end node has been reached
                    if (nextNode.Location == searchParameters.EndLocation)
                    {
                        return true;
                    }
                    else
                    {
                        // If not, check the next set of nodes
                        // Note: Recurses back into Search(Node)
                        if (Search(nextNode, moves)) 
                        {                            
                            return true;
                        }
                    }
                }
            }
            // The method returns false if this path leads to be a dead end
            return false;
        }

        /// <summary>
        /// Returns any nodes that are adjacent to <paramref name="fromNode"/> and may be considered to form the next step in the path
        /// </summary>
        /// <param name="fromNode">The node from which to return the next possible nodes in the path</param>
        /// <returns>A list of next possible nodes in the path</returns>
        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {
            List<Node> walkableNodes = new List<Node>();         

            // check surrounded tiles of the current position
            foreach (var newPosition in LogicRules.GetSurroundedFields(fromNode.Location))
            {  
                // gather environment information
                var region = World.Instance.RegionManager.GetRegion(newPosition.RegionPosition);
                var terrainDefinition = region.GetTerrain(newPosition.CellPosition);
                // check terrain for walkable and other units in the path
                if (terrainDefinition.Walkable)
                    {
                        var unit = region.GetEntity(newPosition.CellPosition);
                        // field is free from everything
                    if (unit == null)
                    {
                        if (m_nodes.ContainsKey(newPosition))
                        {
                            // use the dictionary and get the Node at the positonI
                            var node = m_nodes[newPosition];
                            if (node.State == NodeState.Open)
                            {
                                // calculate the travel cost to the next tile
                                double traversalCost = terrainDefinition.TravelCost;
                                double temp = node.G + traversalCost;
                                if (temp < node.G)
                                {
                                    node.ParentNode = fromNode;
                                    walkableNodes.Add(node);
                                }
                            }
                        }
                        else
                        {
                            // if the Node was not open insert a new one in the dictionary
                            var newNode = new Node(newPosition, searchParameters.EndLocation);
                            newNode.ParentNode = fromNode;
                            walkableNodes.Add(newNode);
                            m_nodes[newPosition] = newNode;
                        }                            
                    }
                    else if (unit != null && newPosition == searchParameters.EndLocation && unit.OwnerID != searchParameters.AccountID)                        
                    {
                        if (m_nodes.ContainsKey(newPosition))
                        {
                            // use the dictionary and get the Node at the positonI
                            var node = m_nodes[newPosition];
                            if (node.State == NodeState.Open)
                            {
                                // calculate the travel cost to the next tile
                                double traversalCost = terrainDefinition.TravelCost;
                                double temp = node.G + traversalCost;
                                if (temp < node.G)
                                {
                                    node.ParentNode = fromNode;
                                    walkableNodes.Add(node);
                                }
                            }
                        }
                        else
                        {
                            // if the Node was not open insert a new one in the dictionary
                            var newNode = new Node(newPosition, searchParameters.EndLocation);
                            newNode.ParentNode = fromNode;
                            walkableNodes.Add(newNode);
                            m_nodes[newPosition] = newNode;
                        }
                    }
                }
            }
            return walkableNodes;
        }

        /// <summary>
        /// The m nodes in a dictionary.
        /// </summary>
        private Dictionary<PositionI, Node> m_nodes;

        /// <summary>
        /// The start node.
        /// </summary>
        private Node startNode;

        /// <summary>
        /// The search parameters.
        /// </summary>
        private SearchParameters searchParameters;
    }
}
