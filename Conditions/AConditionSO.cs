using MHLib.ConfigurableSO;
using System;

namespace MHLib.Conditions
{
    [Serializable]
    public abstract class AConditionSO<TContext> : AConfigurableSO
    {
        public abstract ACondition<TContext> CreateCondition(object parameter, TContext context);
    }
}