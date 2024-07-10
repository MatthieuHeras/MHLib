using UnityEngine;

namespace MHLib
{
    public static class Math
    {
        public static float Remap(float inputMin, float inputMax, float outputMin, float outputMax, float value)
        {
            float t = Mathf.InverseLerp(inputMin, inputMax, value);
            return Mathf.Lerp(outputMin, outputMax, t);
        }
        
        public static Vector3 Remap(float inputMin, float inputMax, Vector3 outputMin, Vector3 outputMax, float value)
        {
            float t = Mathf.InverseLerp(inputMin, inputMax, value);
            return Vector3.Lerp(outputMin, outputMax, t);
        }
        
        public static Vector3 Remap(ulong inputMin, ulong inputMax, Vector3 outputMin, Vector3 outputMax, ulong value)
        {
            float t = InverseLerp(inputMin, inputMax, value);
            return Vector3.Lerp(outputMin, outputMax, t);
        }

        public static ulong Lerp(ulong outputMin, ulong outputMax, float t)
        {
            t = Mathf.Clamp01(t);
            
            return (ulong)((1 - t) * outputMin) + (ulong)(t * outputMax);
        }

        public static float InverseLerp(ulong outputMin, ulong outputMax, ulong value)
        {
            float t;
            if (outputMin > outputMax)
                t = 1f - ((value - outputMax) / (float)(outputMin - outputMax));
            else
                t = (float)(value - outputMin) / (float)(outputMax - outputMin);
            return Mathf.Clamp01(t);
        }
    }
}