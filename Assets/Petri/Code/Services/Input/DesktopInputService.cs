using UnityEngine;

namespace Petri.Services
{
    public class DesktopInputService : IInputService
    {
        public Vector2 Pointer => Input.mousePosition;
        public bool MousePressed => Input.GetMouseButton(0);
        public bool MouseDown => Input.GetMouseButtonDown(0);
        public bool MouseUp => Input.GetMouseButtonUp(0);
    }
}