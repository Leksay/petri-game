using UnityEngine;

namespace Petri.Views
{
    public class PetriView : MonoBehaviour
    {
        /// <summary>
        /// Plane that define height of petri.
        /// </summary>
        [field: SerializeField] public Transform HeightPlaneTransform { get; private set; }
    }
}
