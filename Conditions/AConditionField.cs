using MHLib.ConfigurableSO;
using System;

namespace MHLib.Conditions
{
    [Serializable]
    public abstract class AConditionField<TConditionSO, TCondition, TContext> : AConfigurableField<TConditionSO>, ICondition<TContext>
        where TConditionSO : AConditionSO<TCondition, TContext>
        where TCondition : ACondition<TContext>
    {
        public TCondition CreateCondition(TContext context) => this.configurableSO != null ? this.configurableSO.CreateCondition(this.parameter, context) : null;
        
        public bool CheckOnce(TContext context)
        {
            if (this.configurableSO == null)
                return true;
            
            TCondition condition = this.CreateCondition(context);
            
            return condition.CheckOnce();
        }
    }
}