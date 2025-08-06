using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MHLib.ConfigurableSO
{
    [Serializable]
    public abstract record AWrapper<T>
    {
        [field: SerializeField, HideLabel]
        public T Value { get; private set; }
    }
    
    [Serializable] public record IntWrapper : AWrapper<int>;
    [Serializable] public record UintWrapper : AWrapper<uint>;
    [Serializable] public record LongWrapper : AWrapper<long>;
    [Serializable] public record UlongWrapper : AWrapper<ulong>;
    [Serializable] public record FloatWrapper : AWrapper<float>;
    [Serializable] public record DoubleWrapper : AWrapper<double>;
}