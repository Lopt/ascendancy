using @base.model;
using @base.Models.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace @base.Controllers.action.AStar
{
    public class Node
    {
        public PositionI Location { get; private set; }
        public bool IsWalkable { get; set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F { get { return this.G + this.H; } }
        public NodeState State { get; set; }
        public Node ParentNode { get { return this} set {} }

        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {
            List<Node> walkableNodes = new List<Node>();
            IEnumerable<PositionI> nextLocations = GetAdjacentLocations(fromNode.Location);                     

            foreach (var location in nextLocations)
            {
                int x = location.X;
                int y = location.Y;

                // Stay within the grid's boundaries
                // if (x < 0 || x >= this.width || y < 0 || y >= this.height)
                //    continue;

                Node node = this.nodes[x, y];
                // Ignore non-walkable nodes
                if (!node.IsWalkable)
                    continue;

                // Ignore already-closed nodes
                if (node.State == NodeState.Closed)
                    continue;

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == NodeState.Open)
                {
                    float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                    float gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = fromNode;
                    node.State = NodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
        }

        private bool Search(Node currentNode)
        {
            currentNode.State = NodeState.Closed;
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (var nextNode in nextNodes)
            {
                if (nextNode.Location == this.endNode.Location)
                {
                    return true;
                }
                else
                {
                    if (Search(nextNode)) // Note: Recurses back into Search(Node)
                        return true;
                }
            }
            return false;
        }

        private IEnumerable<PositionI> GetAdjacentLocations(PositionI position)
        {
            var result = new List<PositionI>();

            foreach (var surpos in LogicRules.SurroundTiles)
            {
                result.Add(position + surpos);
            }

            return result;
        }
}
 
public enum NodeState { Untested, Open, Closed }
}
