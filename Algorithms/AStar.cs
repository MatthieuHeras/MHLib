using System;
using System.Collections.Generic;

namespace MHLib
{
    public static class AStar
    {
        /// <summary> Compute the path using AStar algorithm. </summary>
        /// <param name="originNode"> Where the path starts. </param>
        /// <param name="targetNode"> What the path should lead to. </param>
        /// <param name="getNeighbourDistance"> How to determine the distance between a tile and its neighbour (real distance). </param>
        /// <param name="getHeuristicDistance"> How to determine the distance between a tile and the targetTile (estimated distance). </param>
        /// <param name="getNeighbours"> How to get the neighbours from a tile (can return nulls for obstacles). </param>
        /// <param name="maxDistance"> The maximum distance the path can go to. </param>
        /// <typeparam name="TNode"> The type of nodes, can be anything (tiles, positions, etc.). </typeparam>
        /// <returns> A path made of TNodes. </returns>
        public static List<TNode> GetPath<TNode>(TNode originNode, TNode targetNode, Func<TNode, TNode, float> getNeighbourDistance, Func<TNode, TNode, float> getHeuristicDistance, Func<TNode, TNode[]> getNeighbours, float maxDistance = float.MaxValue)
        {
            Dictionary<TNode, AStarNode<TNode>> nodes = new() {{originNode, new AStarNode<TNode>(originNode, 0, getHeuristicDistance(originNode, targetNode))}};
            List<TNode> openList = new() {originNode};

            TNode currentNode = originNode;
            
            while (openList.Count > 0)
            {
                currentNode = GetClosestNode(nodes, openList, out int minIndex);

                if (currentNode.Equals(targetNode))
                    break;
                
                openList.RemoveAt(minIndex);
                
                TNode[] neighbours = getNeighbours(currentNode);

                foreach (TNode neighbour in neighbours)
                {
                    if (neighbour == null) // Obstacle
                        continue;
                    
                    float distanceFromStart = nodes[currentNode].DistanceFromStart + getNeighbourDistance(currentNode, neighbour);

                    if (distanceFromStart > maxDistance)
                        continue;
                    
                    if (!nodes.ContainsKey(neighbour))
                    {
                        // Unknown node yet: add it.
                        nodes.Add(neighbour, new AStarNode<TNode>(currentNode, distanceFromStart, getHeuristicDistance(neighbour, targetNode)));
                        openList.Add(neighbour);
                    }
                    else if (distanceFromStart < nodes[neighbour].DistanceFromStart)
                    {
                        // Known node, but the new distance is shorter: update it.
                        nodes[neighbour].UpdateDistanceFromStart(distanceFromStart, currentNode);
                        openList.Add(neighbour);
                    }
                }
            }

            // Path couldn't be found, tiles aren't connected or max distance is reached.
            if (!currentNode.Equals(targetNode))
                return new List<TNode>();
            
            List<TNode> path = new() {currentNode};

            while (!currentNode.Equals(originNode))
            {
                currentNode = nodes[currentNode].Parent;
                path.Add(currentNode);
            }
            
            path.Reverse();
            return path;
        }

        /// <summary> Compute the path using AStar algorithm. </summary>
        /// <param name="originNode"> Where the path starts. </param>
        /// <param name="targetNodes"> What the path should lead to. </param>
        /// <param name="getDistance"> How to determine the distance between a tile and its neighbour ("true" distance). </param>
        /// <param name="getHeuristicDistance"> How to determine the distance between a tile and the targetTile (estimated distance). </param>
        /// <param name="getNeighbours"> How to get the neighbours from a tile (can return nulls for obstacles). </param>
        /// <param name="maxDistance"> The maximum distance the path can go to. </param>
        /// <typeparam name="TNode"> The type of nodes, can be anything (tiles, positions, etc.). </typeparam>
        /// <returns> A path made of TNodes, with the end node being the closest target node. </returns>
        public static List<TNode> GetPath<TNode>(TNode originNode, List<TNode> targetNodes, Func<TNode, TNode, float> getDistance, Func<TNode, TNode, float> getHeuristicDistance, Func<TNode, TNode[]> getNeighbours, float maxDistance = float.MaxValue)
        {
            float GetMinHeuristicDistance(TNode neighbour)
            {
                float minHeuristicDistance = float.MaxValue;

                foreach (TNode targetNode in targetNodes)
                {
                    float targetNodeHeuristicDistance = getHeuristicDistance(neighbour, targetNode);

                    if (targetNodeHeuristicDistance < minHeuristicDistance)
                        minHeuristicDistance = targetNodeHeuristicDistance;
                }

                return minHeuristicDistance;
            }
            
            Dictionary<TNode, AStarNode<TNode>> nodes = new() {{originNode, new AStarNode<TNode>(originNode, 0, GetMinHeuristicDistance(originNode))}};
            List<TNode> openList = new() {originNode};

            TNode currentNode = originNode;
            
            while (openList.Count > 0)
            {
                currentNode = GetClosestNode(nodes, openList, out int minIndex);

                if (targetNodes.Contains(currentNode))
                    break;
                
                openList.RemoveAt(minIndex);
                
                TNode[] neighbours = getNeighbours(currentNode);

                foreach (TNode neighbour in neighbours)
                {
                    if (neighbour == null) // Obstacle
                        continue;
                    
                    float distanceFromStart = nodes[currentNode].DistanceFromStart + getDistance(currentNode, neighbour);

                    if (distanceFromStart > maxDistance)
                        continue;
                    
                    if (!nodes.ContainsKey(neighbour))
                    {
                        // Unknown node yet: add it.
                        nodes.Add(neighbour, new AStarNode<TNode>(currentNode, distanceFromStart, GetMinHeuristicDistance(neighbour)));
                        openList.Add(neighbour);
                    }
                    else if (distanceFromStart < nodes[neighbour].DistanceFromStart)
                    {
                        // Known node, but the new distance is shorter: update it.
                        nodes[neighbour].UpdateDistanceFromStart(distanceFromStart, currentNode);
                        openList.Add(neighbour);
                    }
                }
            }
            
            // Path couldn't be found, tiles aren't connected or max distance is reached.
            if (!targetNodes.Contains(currentNode))
                return new List<TNode>();
            
            List<TNode> path = new() {currentNode};
            
            while (!currentNode.Equals(originNode))
            {
                currentNode = nodes[currentNode].Parent;
                path.Add(currentNode);
            }
            
            path.Reverse();
            return path;
        }
        
        private static TNode GetClosestNode<TNode>(Dictionary<TNode, AStarNode<TNode>> nodes, List<TNode> openList, out int minIndex)
        {
            float minDistance = float.MaxValue;
            minIndex = -1;
            for (int i = openList.Count - 1; i >= 0; i--)
            {
                if (nodes[openList[i]].TotalEstimatedDistance < minDistance)
                {
                    minDistance = nodes[openList[i]].TotalEstimatedDistance;
                    minIndex = i;
                }
            }
            
            return openList[minIndex];
        }
    }
}