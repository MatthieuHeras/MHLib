namespace MHLib.Extensions
{
    using System.Collections.Generic;

    public static class LinkedListExtensions
    {
        public static void RemoveAllAfter<T>(this LinkedListNode<T> node)
        {
            while (node.Next != null)
                node.List.Remove(node.Next);
        }
        
        public static void RemoveAllBefore<T>(this LinkedListNode<T> node)
        {
            while (node.Previous != null)
                node.List.Remove(node.Previous);
        }
    }
}