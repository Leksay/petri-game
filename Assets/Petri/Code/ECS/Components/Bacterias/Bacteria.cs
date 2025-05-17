using Petri.Configs;
using UnityEngine;

namespace Petri.ECS
{
    public struct Bacteria
    {
        public Vector3 Position;
        public BacteriaType Type;
        public float Grow;
        public float Hp;
    }
}