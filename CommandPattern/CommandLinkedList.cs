using System.Collections.Generic;
using MHLib.Extensions;

namespace MHLib.CommandPattern
{
    public class CommandLinkedList<TCommand>
        where TCommand : ACommand
    {
        private readonly LinkedList<TCommand> linkedList = new();
        private LinkedListNode<TCommand> currentNode;
        private LinkedListNode<TCommand> nextNode;
        
        public TCommand CurrentCommand => this.currentNode?.Value;
        public TCommand NextCommand => this.nextNode?.Value;
        
        public void Do(TCommand newCommand)
        {
            LinkedListNode<TCommand> newNode = new(newCommand);
            
            if (this.currentNode != null)
                this.linkedList.AddAfter(this.currentNode, newNode);
            else
                this.linkedList.AddFirst(newNode);
            
            this.currentNode = newNode;
            this.nextNode = null;
            
            // Clean the linked list tail when we Undo then Do (and not Redo)
            this.currentNode.RemoveAllAfter();
            
            newCommand.Do();
        }
        
        public void Undo()
        {
            if (this.currentNode == null)
                return;
            
            this.currentNode.Value.Undo();
            
            this.nextNode = this.currentNode;
            this.currentNode = this.currentNode.Previous;
        }
        
        public void Redo()
        {
            if (this.nextNode == null)
                return;
            
            this.nextNode.Value.Redo();
            
            this.currentNode = this.nextNode;
            this.nextNode = this.nextNode.Next;
        }
    }
}