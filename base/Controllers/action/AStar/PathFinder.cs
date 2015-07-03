using @base.control.action;
using @base.model;
using @base.Models.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar
{

    /// <summary>
    ///  http://blog.two-cats.com/2014/06/a-star-example/ founded by Mike Clift
    /// </summary>
    public class PathFinder
    {
        private int width;
        private int height;
        private Dictionary<PositionI, Node> m_nodes;
        private Node startNode;
        private Node endNode;
        private SearchParameters searchParameters;

        /// <summary>
        /// Create a new instance of PathFinder
        /// </summary>
        /// <param name="searchParameters"></param>
        public PathFinder(SearchParameters searchParameters)
        {
            m_nodes = new Dictionary<PositionI, Node>();
            startNode = new Node(searchParameters.StartLocation, searchParameters.EndLocation);
            this.searchParameters = searchParameters;
            m_nodes[searchParameters.StartLocation] = startNode;
        }

        /// <summary>
        /// Attempts to find a path from the start location to the end location based on the supplied SearchParameters
        /// </summary>
        /// /// <param name="moves"></param>
        /// <returns>A List of Points representing the path. If no path was found, the returned list is empty.</returns>
        public List<PositionI> FindPath(int moves)
        {
            // The start node is the first entry in the 'open' list
            List<PositionI> path = new List<PositionI>();
            bool success = Search(startNode, moves);
            if (success)
            {
                // If a path was found, follow the parents from the end node to build a list of locations
                Node node = this.endNode;
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
        /// <param name="currentNode"></param>
        /// <param name="moves"></param>
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
                    if (nextNode.Location == this.endNode.Location)
                    {
                        return true;
                    }
                    else
                    {
                        // If not, check the next set of nodes
                        if (Search(nextNode, moves)) // Note: Recurses back into Search(Node)
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
            foreach (var addPosition in LogicRules.SurroundTiles)
            {  
                // gather environment information
                var newPosition = fromNode.Location + addPosition;
                var region = World.Instance.RegionManager.GetRegion(newPosition.RegionPosition);
                var terrainDefinition = region.GetTerrain(newPosition.CellPosition);
                // check terrai for walkable and other units in the path
                if (terrainDefinition.Walkable)
                {
                    var unit = region.GetEntity(newPosition.CellPosition);

                    if (unit != null)
                    {
                        if (!m_nodes.ContainsKey(newPosition))
                        {
                            // use the dictionary and get the Node at the positonI
                            var node = m_nodes[newPosition];
                            if (node.State == NodeState.Open)
                            {
                                // calculate the travel cost to the next tile
                                double traversalCost = terrainDefinition.TravelCost;
                                double gTemp = node.G + traversalCost;
                                if (gTemp < node.G)
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
    }
}
