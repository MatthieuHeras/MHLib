namespace MHLib
{
    public class DijkstraNode<TNode>
    {
        public DijkstraNode(TNode node, DijkstraNode<TNode> parent, float distanceFromStart)
        {
            this.Node = node;
            this.Parent = parent;
            this.DistanceFromStart = distanceFromStart;
        }
        
        public TNode Node { get; private set; }
        public DijkstraNode<TNode> Parent { get; private set; }
        public float DistanceFromStart { get; private set; }
    }
}