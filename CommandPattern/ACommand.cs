namespace MHLib.CommandPattern
{
    public abstract class ACommand
    {
        public abstract void Do();
        public abstract void Undo();
        public virtual void Redo() => this.Do();
    }
}