using Petri.Configs;
using Petri.Views;
using UnityEngine;

namespace Petri.Infrostructure
{
    public class SceneData : MonoBehaviour
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField, Range(0.1f, 3f)] public float DistanceFromCamera { get; private set; }

        [field: Header("Pipette")]
        [field: SerializeField] public PipetteView PipetteView { get; private set; }
        
        [field:Header("Drops")]
        [field: SerializeField] public DropConfig DropConfig { get; private set; }

        [field:Header("Petri")]
        [field: SerializeField] public PetriView PetriView { get; private set; }
    }
}