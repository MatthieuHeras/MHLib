namespace MHLib
{
    internal class AStarNode<TNode>
    {
        public AStarNode(TNode parent, float distanceFromStart, float heuristicDistanceToTarget)
        {
            this.Parent = parent;
            this.DistanceFromStart = distanceFromStart;
            this.heuristicDistanceToTarget = heuristicDistanceToTarget;
            this.TotalEstimatedDistance = distanceFromStart + heuristicDistanceToTarget;
        }

        private readonly float heuristicDistanceToTarget;
        
        public TNode Parent { get; private set; }
        public float DistanceFromStart { get; private set; }
        public float TotalEstimatedDistance { get; private set; }

        public void UpdateDistanceFromStart(float distanceFromStart, TNode newParent)
        {
            this.DistanceFromStart = distanceFromStart;
            this.TotalEstimatedDistance = distanceFromStart + this.heuristicDistanceToTarget;
                
            this.Parent = newParent;
        }
    }
}