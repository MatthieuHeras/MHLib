using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MHLib.ConfigurableSO
{
    [Serializable]
    public abstract class AConfigurableField<TConfigurableSO> where TConfigurableSO : AConfigurableSO
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
            // Create a new instance of the parameter (so it's not null, and displays properly in the inspector)
            this.parameter = this.parameterType != null ? Activator.CreateInstance(this.configurableSO.ParameterType) : null;
        }
    }
}