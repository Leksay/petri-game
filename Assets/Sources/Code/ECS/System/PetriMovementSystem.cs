using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Petri.Infrostructure;
using Petri.Services;
using UnityEngine;

namespace Petri.ECS
{
    public class PetriMovementSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsCustomInject<SceneData> _sceneData;

        private readonly EcsFilterInject<Inc<Petri>> _petriFilter;
        private readonly IInputService _inputService = ServiceLocator.Get<IInputService>();

        private Camera _camera;

        public void Init(IEcsSystems systems)
        {
            var petriId = systems.GetWorld().NewEntity();
            ref var petri = ref _petriFilter.Pools.Inc1.Add(petriId);
            petri.PetriTransform = _sceneData.Value.PetriTransform;

            _camera = _sceneData.Value.MainCamera;
        }
        
        public void Run(IEcsSystems systems)
        {
            var mousePosition = _inputService.Pointer;
            foreach (var entity in _petriFilter.Value)
            {
                ref var petri = ref _petriFilter.Pools.Inc1.Get(entity);
                var petriXPos = _camera.ScreenToViewportPoint(mousePosition).x;
                petri.PetriTransform.position = _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, _sceneData.Value.DistanceFromCamera));

                var value = Mathf.Clamp((petriXPos - 0.5f) * 2, -0.5f, 0.5f);
                petri.PetriTransform.rotation = Quaternion.Euler(0f, 0f, -value  * 70f);
            }
        }
    }
}