namespace MHLib.MHLib.Actions
{
    public abstract class AAction<TContext, TTarget, TResult> : IAction<TContext, TTarget, TResult>
    {
        public abstract TResult Trigger(TContext context, TTarget target);
    }
}