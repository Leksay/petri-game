using UnityEngine;

namespace Petri.Infrostructure
{
    public class SceneData : MonoBehaviour
    {
        [field: SerializeField] public Transform PetriTransform { get; private set; }
        [field: SerializeField] public Camera MainCamera { get; private set; }

        [field: SerializeField, Range(0.1f, 3f)] public float DistanceFromCamera { get; private set; }
    }
}