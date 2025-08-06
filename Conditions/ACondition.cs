using System;

namespace MHLib.Conditions
{
    public abstract class ACondition<TContext> : ICondition<TContext>
    {
        protected ACondition(object parameter, TContext context)
        {
            this.parameter = parameter;
            this.context = context;
        }
        
        protected readonly object parameter;
        protected readonly TContext context;
        
        public bool Value { get; private set; }
        
        public event Action<bool> ValueChangedEvent = delegate { };
        
        /// <summary> If the condition is enabled, simply check Value. This is intended for disabled - or once called - conditions. </summary>
        public bool CheckOnce()
        {
            this.Enable();
            bool value = this.Value;
            this.Disable();
            
            return value;
        }
        
        public virtual void Enable()
        {
            this.HookToContext();
            this.Value = this.Check();
        }
        
        public virtual void Disable()
        {
            this.UnhookToContext();
            this.Value = false;
        }
        
        protected abstract bool Check();
        protected abstract void HookToContext();
        protected abstract void UnhookToContext();
        
        /// <summary> To be called when the context changes (in response to hooked events). </summary>
        protected void UpdateValue() => this.UpdateValue(this.Check());
        
        /// <summary> To be called when the context changes (in response to hooked events). </summary>
        protected void UpdateValue(bool newValue)
        {
            if (newValue == this.Value)
                return;
            
            this.Value = newValue;
            this.ValueChangedEvent.Invoke(this.Value);
        }
        
        public static implicit operator bool(ACondition<TContext> condition) => condition.Value;
    }
}