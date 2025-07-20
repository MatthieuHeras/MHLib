using MHLib.ConfigurableSO;
using System;

namespace MHLib.Conditions
{
    [Serializable]
    public abstract class AConditionField<TContext> : AConfigurableField<AConditionSO<TContext>>
    {
        public ACondition<TContext> CreateCondition(TContext context) => this.configurableSO != null ? this.configurableSO.CreateCondition(this.parameter, context) : null;
        
        public bool CheckOnce(TContext context)
        {
            if (this.configurableSO == null)
                return true;
            
            ACondition<TContext> condition = this.CreateCondition(context);
            
            return condition.CheckOnce();
        }

        protected override string configurableSOLabel => "Condition";
    }
}