using UnityEngine;

namespace Petri.Services
{
    public interface IInputService : IService
    {
        public Vector2 Pointer { get; }
    }
}