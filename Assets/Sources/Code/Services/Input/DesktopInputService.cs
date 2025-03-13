using UnityEngine;

namespace Petri.Services
{
    // Desktop Input Service
    public class DesktopInputService : IInputService
    {
        public Vector2 Pointer => Input.mousePosition;
    }
}