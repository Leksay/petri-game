using Petri.UI;
using UnityEngine;

namespace Petri.ECS
{
    public struct Drop
    {
        public DropView View;

        public Vector3 Position;
        public bool ReachPetri;

        public float TargetSpread;
        public float CurrentSpread;
        public bool Spreading;
        private float _blendShapeValue;
        private float _opacity;

        public float BlendShapeValue
        {
            readonly get => _blendShapeValue;
            set
            {
                _blendShapeValue = value;
                View.SkinnedMeshRenderer.SetBlendShapeWeight(1, value);
            }
        }

        public float Opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                View.SetOpacity(_opacity);

#if UNITY_EDITOR
                View.AlphaDebug = value;
#endif
            }
        }
    }
}