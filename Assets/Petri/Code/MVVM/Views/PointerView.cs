using UnityEngine;

namespace Petri.Views
{
    public class PointerView : MonoBehaviour
    {
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public LineRenderer LineRenderer { get; private set; }

        [SerializeField] private bool _applyColorToLine = true;

        private void Start()
        {
            if (_applyColorToLine)
            {
                LineRenderer.sharedMaterial.color = GetComponent<MeshRenderer>().sharedMaterial.color;
            }
        }
    }
}
