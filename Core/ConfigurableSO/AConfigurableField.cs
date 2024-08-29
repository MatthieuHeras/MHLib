using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MHLib.ConfigurableSO
{
    [Serializable]
    public abstract class AConfigurableField<TConfigurableSO>
        where TConfigurableSO : AConfigurableSO
    {
        [SerializeField, OnValueChanged(nameof(OnConfigurableSOChanged)), BoxGroup("Field", showLabel: false)]
        protected TConfigurableSO configurableSO;
        
        [SerializeReference, HideLabel, HideReferenceObjectPicker, ShowIf(nameof(ShowParameter)), BoxGroup("Field", showLabel: false)]
        protected object parameter;
        
        protected Type parameterType => this.configurableSO != null ? this.configurableSO.ParameterType : null;
        
        // We hide the parameter if the configurable SO doesn't have one
        private bool ShowParameter() => this.parameterType != null;
        
        private void OnConfigurableSOChanged()
        {
            this.parameter = this.ComputeEmptyParameter();
        }

        private object ComputeEmptyParameter()
        {
            Type type = this.parameterType;
            
            if (type == null)
                return null;
            
            // Edge case with string, it has no empty constructor
            if (type == typeof(string))
                return string.Empty;
            
            return Activator.CreateInstance(type);
        }
    }
}