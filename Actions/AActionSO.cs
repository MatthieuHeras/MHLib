using MHLib.ConfigurableSO;

namespace MHLib.MHLib.Actions
{
    public abstract class AActionSO<TContext, TTarget, TResult> : AConfigurableSO
    {
        public TResult Trigger(TContext context, TTarget target, object param) => this.CreateAction(param).Trigger(context, target);

        protected abstract AAction<TContext, TTarget, TResult> CreateAction(object param);
    }
}