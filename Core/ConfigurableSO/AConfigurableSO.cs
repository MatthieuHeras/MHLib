using System;
using UnityEngine;

namespace MHLib.ConfigurableSO
{
    public abstract class AConfigurableSO : ScriptableObject
    {
        // We hardcode the parameter type, set it to null if there's no parameter needed
        public abstract Type ParameterType { get; }
    }
}