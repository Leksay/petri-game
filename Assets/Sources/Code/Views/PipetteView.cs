using UnityEngine;

namespace Petri.Views
{
    public class PipetteView : MonoBehaviour
    {
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public Transform Pointer { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
    }
}