using Petri.UI;
using TLab.UI.SDF;
using UnityEngine;

namespace Petri.Configs
{
    [CreateAssetMenu(fileName = "DropsConfig", menuName = "Petri/DropsConfig", order = 0)]
    public class DropConfig : ScriptableObject
    {
        [field: SerializeField] public DropView Prefab { get; set; }
        [field: SerializeField] public float FallSpeed { get; set; } = 10f;
        [field: SerializeField] public float AnimationSpeed { get; set; } = 10f;
        [field: SerializeField] public float BaseScale { get; set; } = 2.6125f;
        [field: SerializeField] public int PreloadCount { get; set; } = 25;
        [field: SerializeField] public float ScaleToRadius { get; set; } = 0.01f;
        
        [MinMaxRange(0.05f, 3f)]
        public Vector2 MinMaxRadius = new Vector2(0.5f, 2f);
        
        [MinMaxRange(1f, 10f)]
        public Vector2 MinMaxLifetime = new Vector2(1f, 10f);
    }
}