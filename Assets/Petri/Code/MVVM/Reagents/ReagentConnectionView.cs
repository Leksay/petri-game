using Petri.Formula;
using UnityEngine;

namespace Petri.UI
{
    public class ReagentConnectionView : MonoBehaviour
    {
        [SerializeField] private GameObject _up, _down, _left, _right;

        public void SetConnections(ConnectionTypes connectionTypes)
        {
            _up.SetActive((connectionTypes & ConnectionTypes.Up) > 0);
            _down.SetActive((connectionTypes & ConnectionTypes.Down) > 0);
            _left.SetActive((connectionTypes & ConnectionTypes.Left) > 0);
            _right.SetActive((connectionTypes & ConnectionTypes.Right) > 0);
        }
    }
}