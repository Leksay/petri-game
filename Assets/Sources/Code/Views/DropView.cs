using UnityEngine;

namespace Petri.Views
{
    public class DropView : MonoBehaviour
    {
        private static readonly int MaxAlpha = Shader.PropertyToID("_MaxAlpha");
        public Transform Transform;
        public SkinnedMeshRenderer SkinnedMeshRenderer;
        private MaterialPropertyBlock _propertyBlock;

        #if UNITY_EDITOR
        public float AlphaDebug;
        #endif
        
        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void SetOpacity(float opacity)
        {
            SkinnedMeshRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(MaxAlpha, opacity);
            SkinnedMeshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}