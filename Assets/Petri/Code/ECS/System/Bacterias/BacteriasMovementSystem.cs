using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Petri.Helpers;
using Petri.Infrostructure;
using UnityEngine;

namespace Petri.ECS
{
    public class BacteriasMovementSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<Bacteria, Movement>> _bacteriasFilter;
        private readonly EcsCustomInject<SceneData> _sceneData;
        private Vector3 _center;
        private float _radius;
        private float _sqrRadius;

        public void Init(IEcsSystems systems)
        {
            _center = _sceneData.Value.PetriMapView.transform.TransformPoint(_sceneData.Value.PetriMapView.Center);
            _radius = _sceneData.Value.PetriMapView.Radius;
            _sqrRadius = _radius * _radius;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _bacteriasFilter.Value)
            {
                ref var bacteria = ref _bacteriasFilter.Pools.Inc1.Get(entity);
                ref var movement = ref _bacteriasFilter.Pools.Inc2.Get(entity);

                bacteria.Position += movement.Direction * (movement.Speed * Time.deltaTime);
                var vector = bacteria.Position - _center;
                if (vector.sqrMagnitude > _sqrRadius)
                {
                    var normalizedVector = vector.normalized;
                    bacteria.Position = _center + normalizedVector * (_radius);
                    movement.Direction = (Vector3.Reflect(movement.Direction, normalizedVector) + Vector3Utils.RandomDirectionXZ() * 0.45f).normalized;
                }
            }
        }
    }
}