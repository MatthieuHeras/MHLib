namespace MHLib
{
    using System;
    using System.Collections.Generic;
    
    public static class Dijkstra
    {
        /// <summary> Computes the shortest path to a condition fulfilling node using Dijkstra algorithm. </summary>
        /// <param name="originNode"> Where the path starts. </param>
        /// <param name="getDistance"> How to determine the distance between two <b>neighbour</b> nodes. </param>
        /// <param name="getNeighbours"> How to get the neighbours from a tile (can return nulls for obstacles). </param>
        /// <param name="condition"></param>
        /// <param name="maxDistance"> The maximum distance the path can go to. </param>
        /// <typeparam name="TNode"> The type of nodes, can be anything (tiles, positions, etc.). </typeparam>
        /// <returns>A path made of TNodes</returns>
        public static List<TNode> GetClosestNode<TNode>(TNode originNode,
                                                        Func<TNode, TNode, float> getDistance,
                                                        Func<DijkstraNode<TNode>, TNode[]> getNeighbours,
                                                        Func<DijkstraNode<TNode>, bool> condition,
                                                        float maxDistance = float.MaxValue)
        {
            HashSet<DijkstraNode<TNode>> openList = new() {new DijkstraNode<TNode>(originNode, null, 0f)};
            HashSet<TNode> closedList = new() {originNode};
            
            DijkstraNode<TNode> currentNode = null;
            
            while (openList.Count > 0)
            {
                // Get shortest distance node
                float minDistance = float.MaxValue;
                
                foreach (DijkstraNode<TNode> node in openList)
                {
                    if (node.DistanceFromStart < minDistance)
                    {
                        minDistance = node.DistanceFromStart;
                        currentNode = node;
                    }
                }
                
                // Check condition
                if (condition(currentNode))
                    return ComputePath(currentNode);
                
                openList.Remove(currentNode);
                
                // Add neighbours
                TNode[] neighbours = getNeighbours(currentNode);
                foreach (TNode neighbour in neighbours)
                {
                    if (neighbour == null || !closedList.Add(neighbour))
                        continue;
                    
                    float distance = getDistance(currentNode!.Node, neighbour) + currentNode.DistanceFromStart;
                    if (distance <= maxDistance)
                        openList.Add(new DijkstraNode<TNode>(neighbour, currentNode, distance));
                }
            }
            
            return new List<TNode>();
        }
        
        private static List<TNode> ComputePath<TNode>(DijkstraNode<TNode> currentNode)
        {
            List<TNode> path = new();
            while (currentNode != null)
            {
                path.Add(currentNode.Node);
                currentNode = currentNode.Parent;
            }
            
            path.Reverse();
            return path;
        }
    }
}