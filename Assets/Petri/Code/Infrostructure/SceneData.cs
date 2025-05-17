using Petri.Configs;
using Petri.Rendering;
using Petri.UI;
using Petri.Views;
using UnityEngine;

namespace Petri.Infrostructure
{
    public class SceneData : MonoBehaviour
    {
        [field: SerializeField] public BacteriaRenderer BacteriaRenderer { get; private set; }
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField, Range(0.1f, 3f)] public float DistanceFromCamera { get; private set; }

        [field: Header("Pipette")]
        [field: SerializeField] public PipetteView PipetteView { get; private set; }
        
        [field:Header("Drops")]
        [field: SerializeField] public DropConfig DropConfig { get; private set; }

        [field:Header("Petri")]
        [field: SerializeField] public PetriView PetriView { get; private set; }
        [field: SerializeField] public PetriMapView PetriMapView { get; private set; }

        [field: Header("Pointer")]
        [field: SerializeField] public PointerView PointerView { get; private set; }

        [field: SerializeField] public LevelConfig LevelConfig { get; private set; }

        [field:Header("Constants"), Space(10)]
        [field: SerializeField] public float PetriRadius { get; private set; } = 0.33f;
    }
}