using UnityEngine;

namespace Petri.Services
{
    public interface IInputService : IService
    {
        public Vector2 Pointer { get; }

        public bool MousePressed { get; }
        public bool MouseDown { get; }
        public bool MouseUp { get; }
    }
}