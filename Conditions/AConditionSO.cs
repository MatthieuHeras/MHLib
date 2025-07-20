using MHLib.ConfigurableSO;
using System;

namespace MHLib.Conditions
{
    [Serializable]
    public abstract class AConditionSO<TCondition, TContext> : AConfigurableSO
        where TCondition : ACondition<TContext>
    {
        public abstract TCondition CreateCondition(object parameter, TContext context);
    }
}