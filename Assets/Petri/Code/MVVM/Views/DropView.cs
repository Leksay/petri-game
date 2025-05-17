using UnityEngine;

namespace Petri.UI
{
    public class DropView : MonoBehaviour
    {
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");
        public Transform Transform;
        public SkinnedMeshRenderer SkinnedMeshRenderer;
        public float ScaleToRadius = 0.01f;

        private MaterialPropertyBlock _propertyBlock;

        #if UNITY_EDITOR
        public float AlphaDebug;
        public float ScaleDebug;
        #endif
        
        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void SetOpacity(float opacity)
        {
            SkinnedMeshRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(Alpha, opacity);
            SkinnedMeshRenderer.SetPropertyBlock(_propertyBlock);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ScaleToRadius * transform.localScale.x);
        }
    }
}