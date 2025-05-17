using UnityEngine;

namespace Petri.Helpers
{
    public static class Vector3Utils
    {
        private static System.Random _random = new System.Random();
        
        public static Vector3 RandomDirectionXZ() => Random.insideUnitCircle.ToXZ();

        public static Vector3 ToXZ(this Vector2 vector) => new(vector.x, 0f, vector.y);
    }
}