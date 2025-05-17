using UnityEngine;

namespace Petri.Views
{
    public class PetriMapView : MonoBehaviour
    {
        [field: SerializeField] public Vector3 Center { get; set; }
        [field: SerializeField] public float Radius { get; set; }

        private void OnDrawGizmosSelected()
        {
            var center = transform.TransformPoint(Center);
            Gizmos.DrawWireSphere(center, Radius);
        }
    }
}