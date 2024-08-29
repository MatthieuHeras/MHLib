namespace MHLib.MHLib.Actions
{
    public interface IAction<in TContext, in TTarget, out TResult>
    {
        public TResult Trigger(TContext context, TTarget target);
    }
}