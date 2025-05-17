using Petri.Models;
using Petri.UI;
using Petri.ViewModels;
using UnityEngine;

namespace Petri
{
    public class PipetteBinder : MonoBehaviour
    {
        [SerializeField] private UIPipetteView _pipetteView;
        private PipetteModel _pipetteModel;
        private PipetteViewModel _pipetteViewModel;

        private void Start()
        {
            _pipetteModel = new PipetteModel(_pipetteView.SizeConfiguration);
            _pipetteViewModel = new PipetteViewModel(_pipetteView, _pipetteModel);
        }
    }
}