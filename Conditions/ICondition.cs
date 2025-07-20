namespace MHLib.Conditions
{
    public interface ICondition<in TContext>
    {
        public bool Value { get; }
        public bool CheckOnce();
        public void Enable();
        public void Disable();
    }
}