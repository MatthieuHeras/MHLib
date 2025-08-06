using System;
using UnityEngine;

namespace MHLib.ConfigurableSO
{
    public abstract class AConfigurableSO : ScriptableObject
    {
        /// <summary>
        /// We hardcode the parameter type, set it to null if there's no parameter needed.<br/>
        /// If you need a value type (int, long, etc.), please use the wrappers, as SerializeReference can't support value types.
        /// </summary>
        public abstract Type ParameterType { get; }
    }
}