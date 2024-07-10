using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace MHLib.Utils
{
    public static class MouseUtils
    {
        public static bool IsMouseOverUI => IsDeviceOverLayer(Mouse.current.deviceId, 5);
        
        public static bool IsMousePositionValid(Camera camera)
        {
            return !IsMouseOverUI && IsMouseInsideView(camera);
        }
        
        public static bool IsMouseInsideView(Camera camera)
        {
            Vector3 view = camera.ScreenToViewportPoint(Input.mousePosition);
            
            return view.x is > 0 and < 1 && view.y is > 0 and < 1;
        }
        
        public static bool IsDeviceOverLayer(int deviceId, int layer)
        {
            if (EventSystem.current == null)
                return false;
            
            RaycastResult lastRaycastResult = ((InputSystemUIInputModule)EventSystem.current.currentInputModule).GetLastRaycastResult(deviceId);
            return lastRaycastResult.gameObject != null && lastRaycastResult.gameObject.layer == layer;
        }
    }
}