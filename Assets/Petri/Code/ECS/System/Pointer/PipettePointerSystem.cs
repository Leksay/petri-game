using Leopotam.EcsLite;
using Petri.Infrostructure;
using Petri.UI;
using Petri.Views;
using UnityEngine;

namespace Petri.ECS
{
    public class PipettePointerSystem : IEcsRunSystem
    {
        private readonly PipetteView _pipetteView;
        private readonly Transform _petriPlane;
        private readonly PointerView _pointerView;

        private readonly LineRenderer _lineRenderer;
        
        public PipettePointerSystem(SceneData sceneData)
        {
            _pipetteView = sceneData.PipetteView;
            _petriPlane = sceneData.PetriView.HeightPlaneTransform;
            _pointerView = sceneData.PointerView;
            _lineRenderer = _pointerView.LineRenderer;

            _pointerView.gameObject.SetActive(true);
        }

        public void Run(IEcsSystems systems)
        {
            var projectOnPlane = Vector3.ProjectOnPlane(_pipetteView.Pointer.position, _petriPlane.up);
            projectOnPlane.y = _petriPlane.position.y + 0.001f;
            _pointerView.Transform.position = projectOnPlane;

            _lineRenderer.SetPosition(1, _pipetteView.Pointer.position);
            _lineRenderer.SetPosition(0, _pointerView.Transform.position);
        }
    }
}