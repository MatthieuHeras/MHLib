using MHLib.ConfigurableSO;
using System;

namespace MHLib.MHLib.Actions
{
    [Serializable]
    public abstract class AActionField<TContext, TTarget, TResult> : AConfigurableField<AActionSO<TContext, TTarget, TResult>>, IAction<TContext, TTarget, TResult>
    {
        public TResult Trigger(TContext context, TTarget target) => this.configurableSO.Trigger(context, target, this.parameter);
    }
}